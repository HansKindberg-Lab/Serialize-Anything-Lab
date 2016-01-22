namespace HansKindberg.Collections.Generic
{
	public interface ITreeFactory<T>
	{
		#region Methods

		ITreeNode<T> Create(ITreeNode<T> parent);
		ITreeNodeCollection<T> CreateCollection(ITreeNode<T> parent);

		#endregion
	}
}