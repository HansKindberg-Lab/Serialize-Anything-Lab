namespace HansKindberg.Serialization.Formatters
{
	public interface IMemoryFormatter
	{
		#region Methods

		object Deserialize(string value);
		string Serialize(object instance);

		#endregion
	}
}