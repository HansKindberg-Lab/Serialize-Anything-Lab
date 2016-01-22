namespace HansKindberg.Serialization
{
	public interface ISerializable
	{
		#region Properties

		object Instance { get; }

		#endregion
	}

	public interface ISerializable<T> : ISerializable
	{
		#region Properties

		new T Instance { get; }

		#endregion
	}
}