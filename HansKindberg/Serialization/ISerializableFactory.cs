namespace HansKindberg.Serialization
{
	public interface ISerializableFactory
	{
		#region Methods

		ISerializable Create(object instance);
		ISerializable Create(object instance, bool includeStaticFields);
		ISerializable<T> Create<T>(T instance);
		ISerializable<T> Create<T>(T instance, bool includeStaticFields);

		#endregion
	}
}