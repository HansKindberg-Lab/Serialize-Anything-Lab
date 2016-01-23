using HansKindberg.Serialization.Formatters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.IntegrationTests.Serialization.Formatters
{
	[TestClass]
	public class BinaryMemoryFormatterTest
	{
		#region Fields

		private const int _integer = 393580;
		private const string _serializedInteger = "AAEAAAD/////AQAAAAAAAAAEAQAAAAxTeXN0ZW0uSW50MzIBAAAAB21fdmFsdWUACGwBBgAL";

		#endregion

		#region Methods

		[TestMethod]
		public void Deserialize_ToInteger_Test()
		{
			Assert.AreEqual(_integer, new BinaryMemoryFormatter().Deserialize(_serializedInteger));
		}

		[TestMethod]
		public void Serialize_Integer_Test()
		{
			Assert.AreEqual(_serializedInteger, new BinaryMemoryFormatter().Serialize(_integer));
		}

		#endregion
	}
}