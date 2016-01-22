namespace HansKindberg.Serialization
{
	public interface IMemoryFormatterFactory
	{
		#region Methods

		IMemoryFormatter Create();

		#endregion
	}
}