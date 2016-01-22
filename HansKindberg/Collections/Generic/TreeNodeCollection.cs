using System;
using System.Collections;
using System.Collections.Generic;

namespace HansKindberg.Collections.Generic
{
	public class TreeNodeCollection<T> : ITreeNodeCollection<T>
	{
		#region Fields

		private readonly IList<ITreeNode<T>> _list;

		#endregion

		#region Constructors

		public TreeNodeCollection(ITreeNode<T> parent, ITreeFactory<T> treeFactory, IEqualityComparer<T> equalityComparer)
		{
			if(parent == null)
				throw new ArgumentNullException(nameof(parent));

			if(treeFactory == null)
				throw new ArgumentNullException(nameof(treeFactory));

			if(equalityComparer == null)
				throw new ArgumentNullException(nameof(equalityComparer));

			this.EqualityComparer = equalityComparer;
			this._list = new List<ITreeNode<T>>();
			this.Parent = parent;
			this.TreeFactory = treeFactory;
		}

		#endregion

		#region Properties

		protected internal virtual IEqualityComparer<T> EqualityComparer { get; }
		public virtual ITreeNode<T> Parent { get; }
		protected internal virtual ITreeFactory<T> TreeFactory { get; }

		#endregion

		#region Methods

		public virtual ITreeNode<T> Add(T value)
		{
			var treeNode = this.CreateTreeNode(value);
			this._list.Add(treeNode);
			return treeNode;
		}

		public virtual void Clear()
		{
			this._list.Clear();
		}

		protected internal virtual ITreeNode<T> CreateTreeNode(T value)
		{
			var treeNode = this.TreeFactory.Create(this.Parent);
			treeNode.Value = value;
			return treeNode;
		}

		public virtual IEnumerator<ITreeNode<T>> GetEnumerator()
		{
			return this._list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public virtual int Remove(T value)
		{
			var removedTreeNodes = 0;

			for(var i = this._list.Count - 1; i >= 0; i--)
			{
				if(!this.EqualityComparer.Equals(this._list[i].Value, value))
					continue;

				this._list.RemoveAt(i);
				removedTreeNodes++;
			}

			return removedTreeNodes;
		}

		#endregion
	}
}