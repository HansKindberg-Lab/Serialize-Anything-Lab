using System;

namespace HansKindberg.Serialization
{
	[Serializable]
	public class Serializable : ISerializable
	{
		#region Fields

		[NonSerialized] private object _instance;

		#endregion

		#region Constructors

		public Serializable(object instance, bool includeStaticFields, ISerializableFactory serializableFactory)
		{
			if(serializableFactory == null)
				throw new ArgumentNullException(nameof(serializableFactory));

			this.IncludeStaticFields = includeStaticFields;
			this._instance = instance;
			this.SerializableFactory = serializableFactory;
		}

		#endregion

		#region Properties

		protected internal virtual bool IncludeStaticFields { get; }

		public virtual object Instance
		{
			get { return this._instance; }
			protected internal set { this._instance = value; }
		}

		protected internal virtual ISerializableFactory SerializableFactory { get; }

		#endregion
	}

	/// <summary>
	/// Generic serializable wrapper to be able to serialize/deserialize theoretically any type of object.
	/// The idea is originally from: <see href="http://www.codeproject.com/KB/cs/AnonymousSerialization.aspx">Anonymous Method Serialization, by Fredrik Norén, 12 Feb 2009</see>
	/// </summary>
	[Serializable]
	public class Serializable<T> : Serializable, ISerializable<T>
	{
		#region Constructors

		public Serializable(T instance, bool includeStaticFields, ISerializableFactory serializableFactory) : base(instance, includeStaticFields, serializableFactory) {}

		#endregion

		#region Properties

		public new virtual T Instance
		{
			get { return (T) base.Instance; }
			protected internal set { base.Instance = value; }
		}

		#endregion
	}
}