using System.Diagnostics.CodeAnalysis;
using System.Runtime.Remoting.Messaging;

namespace HansKindberg.Serialization
{
	public interface IMemoryFormatter : IRemotingFormatter
	{
		#region Methods

		[SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string")]
		object Deserialize(string serializationString);

		[SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string")]
		object Deserialize(string serializationString, HeaderHandler handler);

		string Serialize(object graph);
		string Serialize(object graph, Header[] headers);

		#endregion
	}
}