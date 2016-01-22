using HansKindberg.Serialization.Formatters;

namespace HansKindberg.Serialization
{
	public class MemoryFormatterFactory : IMemoryFormatterFactory
	{
		#region Methods

		public virtual IMemoryFormatter Create()
		{
			return new BinaryMemoryFormatter();
		}

		#endregion
	}
}