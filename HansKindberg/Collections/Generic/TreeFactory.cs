using System.Collections.Generic;

namespace HansKindberg.Collections.Generic
{
	public class TreeFactory<T> : ITreeFactory<T>
	{
		#region Methods

		public virtual ITreeNode<T> Create(ITreeNode<T> parent)
		{
			return new TreeNode<T>(parent, this);
		}

		public virtual ITreeNodeCollection<T> CreateCollection(ITreeNode<T> parent)
		{
			return new TreeNodeCollection<T>(parent, this, EqualityComparer<T>.Default);
		}

		#endregion
	}
}