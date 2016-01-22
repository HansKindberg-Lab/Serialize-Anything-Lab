using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace HansKindberg.Serialization
{
	[Serializable]
	public class SerializationFailure
	{
		#region Properties

		public virtual SerializationException SerializationException { get; set; }

		[SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
		public virtual Type Type { get; set; }

		#endregion
	}
}