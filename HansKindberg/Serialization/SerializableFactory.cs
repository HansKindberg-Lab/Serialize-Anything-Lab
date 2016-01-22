using System;

namespace HansKindberg.Serialization
{
	[Serializable]
	public class SerializableFactory : ISerializableFactory
	{
		#region Fields

		private const bool _defaultIncludeStaticFields = false;

		#endregion

		#region Constructors

		public SerializableFactory(ISerializationResolver serializationResolver)
		{
			if(serializationResolver == null)
				throw new ArgumentNullException("serializationResolver");

			this.SerializationResolver = serializationResolver;
		}

		#endregion

		#region Properties

		protected internal virtual bool DefaultIncludeStaticFields => _defaultIncludeStaticFields;
		protected internal virtual ISerializationResolver SerializationResolver { get; }

		#endregion

		#region Methods

		public virtual ISerializable Create(object instance)
		{
			return this.Create(instance, this.DefaultIncludeStaticFields);
		}

		public virtual ISerializable Create(object instance, bool includeStaticFields)
		{
			return new Serializable(instance, includeStaticFields, this);
		}

		public virtual ISerializable<T> Create<T>(T instance)
		{
			return this.Create(instance, this.DefaultIncludeStaticFields);
		}

		public virtual ISerializable<T> Create<T>(T instance, bool includeStaticFields)
		{
			return new Serializable<T>(instance, includeStaticFields, this);
		}

		#endregion
	}
}