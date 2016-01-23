using System;
using System.Reflection;

namespace HansKindberg.Serialization
{
	public class FieldCacheKey : IFieldCacheKey, IEquatable<IFieldCacheKey>
	{
		#region Constructors

		public FieldCacheKey(BindingFlags bindings, Type type)
		{
			if(type == null)
				throw new ArgumentNullException("type");

			this.Bindings = bindings;
			this.Type = type;
		}

		#endregion

		#region Properties

		public virtual BindingFlags Bindings { get; }
		public virtual Type Type { get; }

		#endregion

		#region Methods

		public override bool Equals(object obj)
		{
			if(ReferenceEquals(this, obj))
				return true;

			return this.Equals(obj as IFieldCacheKey);
		}

		public virtual bool Equals(IFieldCacheKey other)
		{
			if(other == null)
				return false;

			return Equals(this.Bindings, other.Bindings) && this.Type == other.Type;
		}

		public override int GetHashCode()
		{
			return ((int) this.Bindings + "_" + (this.Type?.AssemblyQualifiedName ?? string.Empty).ToUpperInvariant()).GetHashCode();
		}

		public static bool operator ==(FieldCacheKey left, FieldCacheKey right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(FieldCacheKey left, FieldCacheKey right)
		{
			return !Equals(left, right);
		}

		#endregion
	}
}