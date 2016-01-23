using System;
using System.IO;
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

		public virtual object Deserialize(string value)
		{
			if(value == null)
				throw new ArgumentNullException(nameof(value));

			using(var memoryStream = this.StringToMemoryStream(value))
			{
				return this.BinaryFormatter.Deserialize(memoryStream);
			}
		}

		protected internal virtual string MemoryStreamToString(MemoryStream memoryStream)
		{
			if(memoryStream == null)
				throw new ArgumentNullException(nameof(memoryStream));

			return Convert.ToBase64String(memoryStream.ToArray());
		}

		public virtual string Serialize(object instance)
		{
			if(instance == null)
				throw new ArgumentNullException(nameof(instance));

			using(var memoryStream = new MemoryStream())
			{
				this.BinaryFormatter.Serialize(memoryStream, instance);
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