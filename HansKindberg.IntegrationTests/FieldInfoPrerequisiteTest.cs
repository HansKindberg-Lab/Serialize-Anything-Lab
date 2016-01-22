using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HansKindberg.IntegrationTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HansKindberg.IntegrationTests
{
	[TestClass]
	public class FieldInfoPrerequisiteTest
	{
		#region Methods

		[TestMethod]
		public void Equals_Test()
		{
			const string fieldName = "PublicInstanceFieldForMainClass";

			// ReSharper disable EqualExpressionComparison
			Assert.IsTrue(typeof(MainClass).GetField(fieldName).Equals(typeof(MainClass).GetField(fieldName)));
			// ReSharper restore EqualExpressionComparison

			var fields = new List<FieldInfo>
			{
				typeof(MainClass).GetField(fieldName),
				typeof(MainClass).GetField(fieldName)
			};

			Assert.AreEqual(2, fields.Count);

			Assert.AreEqual(1, fields.Distinct().Count());
		}

		[TestMethod]
		public void IsStatic_IfTheFieldIsAConstantField_ShouldReturnTrue()
		{
			Assert.IsTrue(typeof(MainClass).GetField("PublicConstantFieldForMainClass").IsStatic);
		}

		#endregion
	}
}