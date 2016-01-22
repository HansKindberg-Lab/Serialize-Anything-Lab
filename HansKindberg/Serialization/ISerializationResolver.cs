using System;
using System.Collections.Generic;
using System.Reflection;

namespace HansKindberg.Serialization
{
	public interface ISerializationResolver
	{
		#region Properties

		bool InvestigateSerializability { get; set; }
		IList<SerializationFailure> SerializationFailures { get; }

		#endregion

		#region Methods

		object CreateUninitializedObject(Type type);
		IEnumerable<FieldInfo> GetFields(object instance, bool includeStaticFields);
		bool IsSerializable(object instance);

		#endregion
	}
}