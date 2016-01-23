using System.Linq;
using Castle.DynamicProxy;
using HansKindberg.Serialization;
using HansKindberg.Serialization.Formatters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.IntegrationTests.Serialization
{
	[TestClass]
	public class SerializationResolverTest
	{
		#region Fields

		private static readonly SerializationResolver _serializationResolver = new SerializationResolver(new BinaryMemoryFormatter(), new DefaultProxyBuilder());

		#endregion

		#region Properties

		protected internal virtual SerializationResolver SerializationResolver => _serializationResolver;

		#endregion

		#region Methods

		[TestMethod]
		public void GetFields_IfTheInstanceIsAnIntegerAndIncludeStaticFieldsIsSetToFalse_ShouldReturnOneField()
		{
			Assert.AreEqual(1, this.SerializationResolver.GetFields(2, false).Count());
		}

		[TestMethod]
		public void GetFields_IfTheInstanceIsAnIntegerAndIncludeStaticFieldsIsSetToTrue_ShouldReturnThreeFields()
		{
			Assert.AreEqual(3, this.SerializationResolver.GetFields(200, true).Count());
		}

		#endregion
	}
}