using System;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace HansKindberg.Serialization.Formatters
{
	public class BinaryMemoryFormatter : IMemoryFormatter
	{
		#region Constructors

		public BinaryMemoryFormatter()
		{
			this.BinaryFormatter = new BinaryFormatter();
		}

		public BinaryMemoryFormatter(ISurrogateSelector selector, StreamingContext context)
		{
			this.BinaryFormatter = new BinaryFormatter(selector, context);
		}

		#endregion

		#region Properties

		protected internal virtual BinaryFormatter BinaryFormatter { get; }

		public virtual SerializationBinder Binder
		{
			get { return this.BinaryFormatter.Binder; }
			set { this.BinaryFormatter.Binder = value; }
		}

		public virtual StreamingContext Context
		{
			get { return this.BinaryFormatter.Context; }
			set { this.BinaryFormatter.Context = value; }
		}

		public virtual ISurrogateSelector SurrogateSelector
		{
			get { return this.BinaryFormatter.SurrogateSelector; }
			set { this.BinaryFormatter.SurrogateSelector = value; }
		}

		#endregion

		#region Methods

		public virtual object Deserialize(Stream serializationStream)
		{
			return this.BinaryFormatter.Deserialize(serializationStream);
		}

		public virtual object Deserialize(Stream serializationStream, HeaderHandler handler)
		{
			return this.BinaryFormatter.Deserialize(serializationStream, handler);
		}

		public virtual object Deserialize(string serializationString)
		{
			if(serializationString == null)
				throw new ArgumentNullException(nameof(serializationString));

			using(var memoryStream = this.StringToMemoryStream(serializationString))
			{
				return this.BinaryFormatter.Deserialize(memoryStream);
			}
		}

		public virtual object Deserialize(string serializationString, HeaderHandler handler)
		{
			if(serializationString == null)
				throw new ArgumentNullException(nameof(serializationString));

			using(var memoryStream = this.StringToMemoryStream(serializationString))
			{
				return this.BinaryFormatter.Deserialize(memoryStream, handler);
			}
		}

		protected internal virtual string MemoryStreamToString(MemoryStream memoryStream)
		{
			if(memoryStream == null)
				throw new ArgumentNullException(nameof(memoryStream));

			return Convert.ToBase64String(memoryStream.ToArray());
		}

		public virtual void Serialize(Stream serializationStream, object graph)
		{
			this.BinaryFormatter.Serialize(serializationStream, graph);
		}

		public virtual void Serialize(Stream serializationStream, object graph, Header[] headers)
		{
			this.BinaryFormatter.Serialize(serializationStream, graph, headers);
		}

		public virtual string Serialize(object graph)
		{
			if(graph == null)
				throw new ArgumentNullException(nameof(graph));

			using(var memoryStream = new MemoryStream())
			{
				this.BinaryFormatter.Serialize(memoryStream, graph);
				return this.MemoryStreamToString(memoryStream);
			}
		}

		public virtual string Serialize(object graph, Header[] headers)
		{
			if(graph == null)
				throw new ArgumentNullException(nameof(graph));

			using(var memoryStream = new MemoryStream())
			{
				this.BinaryFormatter.Serialize(memoryStream, graph, headers);
				return this.MemoryStreamToString(memoryStream);
			}
		}

		protected internal virtual MemoryStream StringToMemoryStream(string value)
		{
			if(value == null)
				throw new ArgumentNullException(nameof(value));

			return new MemoryStream(Convert.FromBase64String(value));
		}

		#endregion
	}
}