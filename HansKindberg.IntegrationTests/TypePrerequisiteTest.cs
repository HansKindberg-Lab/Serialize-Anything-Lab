using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using HansKindberg.IntegrationTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.IntegrationTests
{
	[TestClass]
	public class TypePrerequisiteTest
	{
		#region Methods

		[TestMethod]
		[SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flag")]
		public void GetFields_IfBothBindingFlagDeclaredOnlyAndFlattenHierarchyIsSet_TheBindingFlagDeclaredOnlyIsApplied()
		{
			var fields = typeof(MainClass).GetFields(BindingFlags.DeclaredOnly | BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

			Assert.AreEqual(12, fields.Count());

			Assert.AreEqual(12, fields.Count(field => field.DeclaringType == typeof(MainClass)));
		}

		[TestMethod]
		public void GetFields_ShouldReturnPublicInstanceAndStaticFieldsInTheRequestedTypeAndPublicInstanceFieldsInTheDerivedClass()
		{
			var fields = typeof(MainClass).GetFields();

			Assert.AreEqual(4, fields.Count());

			Assert.AreEqual(3, fields.Count(field => field.DeclaringType == typeof(MainClass)));
			Assert.AreEqual(1, fields.Count(field => field.DeclaringType == typeof(BaseClass)));

			Assert.IsTrue(fields.FirstOrDefault(field => field.DeclaringType == typeof(BaseClass) && !field.IsStatic) != null);

			Assert.IsTrue(fields.All(field => field.IsPublic));
		}

		[TestMethod]
		public void GetFields_Test_1()
		{
			var fields = typeof(MainClass).GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

			Assert.AreEqual(12, fields.Count());

			Assert.IsTrue(fields.All(field => field.DeclaringType == typeof(MainClass)));
		}

		[TestMethod]
		public void GetFields_Test_2()
		{
			var fields = typeof(MainClass).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

			Assert.AreEqual(15, fields.Count());

			Assert.AreEqual(12, fields.Count(field => field.DeclaringType == typeof(MainClass)));

			Assert.AreEqual(3, fields.Count(field => field.DeclaringType == typeof(BaseClass)));
		}

		[TestMethod]
		public void GetFields_Test_3()
		{
			var fields = typeof(MainClass).GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

			Assert.AreEqual(21, fields.Count());

			Assert.AreEqual(3, fields.Count(field => field.IsPrivate));

			Assert.AreEqual(12, fields.Count(field => field.DeclaringType == typeof(MainClass)));

			Assert.AreEqual(9, fields.Count(field => field.DeclaringType == typeof(BaseClass)));

			Assert.AreEqual(9, fields.Count(field => field.DeclaringType == typeof(BaseClass) && !field.IsPrivate));
		}

		[SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "a")]
		[TestMethod]
		public void InMscorlibThereAre308ValueTypesThatAreNotSerializable()
		{
			var assembly = typeof(string).Assembly;

			Assert.AreEqual("mscorlib", assembly.GetName().Name);

			Assert.AreEqual(308, assembly.GetTypes().Count(type => type.IsValueType && !type.IsSerializable));
		}

		[TestMethod]
		public void InMscorlibThereAre3233Types()
		{
			var assembly = typeof(string).Assembly;

			Assert.AreEqual("mscorlib", assembly.GetName().Name);

			Assert.AreEqual(3233, assembly.GetTypes().Count());
		}

		[TestMethod]
		public void InMscorlibThereAre869ValueTypes()
		{
			var assembly = typeof(string).Assembly;

			Assert.AreEqual("mscorlib", assembly.GetName().Name);

			Assert.AreEqual(869, assembly.GetTypes().Count(type => type.IsValueType));
		}

		[TestMethod]
		public void IsSerializable_IfTheTypeHasASerializableAttribute_ShouldReturnTrue()
		{
			Assert.IsTrue(typeof(BaseClassWithSerializableAttribute).IsSerializable);
		}

		[TestMethod]
		public void IsSerializable_IfTheTypeHasNoSerializableAttributeAndNoOtherSerializableImplementation_ShouldReturnFalse()
		{
			Assert.IsFalse(typeof(MainClassWithoutSerializableAttribute).IsSerializable);
		}

		#endregion
	}
}