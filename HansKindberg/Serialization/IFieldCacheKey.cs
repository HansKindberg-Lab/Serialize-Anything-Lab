using System;
using System.Reflection;

namespace HansKindberg.Serialization
{
	public interface IFieldCacheKey
	{
		#region Properties

		BindingFlags Bindings { get; }
		Type Type { get; }

		#endregion
	}
}