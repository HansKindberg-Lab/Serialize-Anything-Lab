using System.Reflection;
using HansKindberg.Collections.Generic;

namespace MvcApplication.Models.ViewModels
{
	public class HomeViewModel
	{
		#region Properties

		public virtual ITreeNode<FieldInfo> FieldTree { get; set; }
		public virtual bool IncludeStaticFields { get; set; }
		public virtual string IncludeStaticFieldsKey { get; set; }

		#endregion
	}
}