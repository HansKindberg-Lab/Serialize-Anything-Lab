using System;
using System.Globalization;
using System.Reflection;

namespace MvcApplication.Business.Reflection
{
	public class Field : FieldInfo
	{
		#region Constructors

		public Field(Type declaringType, string name)
		{
			this.DeclaringType = declaringType;
			this.Name = name;
		}

		#endregion

		#region Properties

		public override FieldAttributes Attributes => FieldAttributes.Public;
		public override Type DeclaringType { get; }
		public override RuntimeFieldHandle FieldHandle => new RuntimeFieldHandle();
		public override Type FieldType => null;
		public override string Name { get; }
		public override Type ReflectedType => null;

		#endregion

		#region Methods

		public override object[] GetCustomAttributes(bool inherit)
		{
			return new object[0];
		}

		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return new object[0];
		}

		public override object GetValue(object obj)
		{
			return null;
		}

		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return false;
		}

		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture) {}

		#endregion
	}
}