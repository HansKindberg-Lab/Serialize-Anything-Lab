using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HansKindberg.Serialization
{
	[Serializable]
	public class SerializationResolver
	{
		#region Fields

		private const BindingFlags _defaultBindings = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

		#endregion

		#region Properties

		protected internal virtual BindingFlags DefaultBindings => _defaultBindings;

		#endregion

		#region Methods

		public virtual IEnumerable<FieldInfo> GetFields(object instance, bool includeStaticFields)
		{
			if(instance == null)
				return Enumerable.Empty<FieldInfo>();

			var bindings = this.DefaultBindings;

			if(includeStaticFields)
				bindings = bindings | BindingFlags.Static;

			return this.GetFields(instance.GetType(), bindings);
		}

		protected internal virtual IEnumerable<FieldInfo> GetFields(Type type, BindingFlags bindings)
		{
			return type?.GetFields(bindings).Concat(this.GetFields(type.BaseType, bindings)) ?? Enumerable.Empty<FieldInfo>();
		}

		#endregion
	}
}