using System.Collections.Generic;

namespace HansKindberg.Collections.Generic
{
	public interface ITreeNodeCollection<T> : IEnumerable<ITreeNode<T>>
	{
		#region Properties

		ITreeNode<T> Parent { get; }

		#endregion

		#region Methods

		ITreeNode<T> Add(T value);
		void Clear();
		int Remove(T value);

		#endregion
	}
}