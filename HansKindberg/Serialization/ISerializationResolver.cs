using System.Collections.Generic;
using System.Reflection;

namespace HansKindberg.Serialization
{
	public interface ISerializationResolver
	{
		#region Methods

		IEnumerable<FieldInfo> GetFields(object instance, bool includeStaticFields);

		#endregion
	}
}