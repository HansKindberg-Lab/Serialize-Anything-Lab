using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Castle.DynamicProxy;
using HansKindberg.Collections.Generic;
using HansKindberg.Serialization;
using MvcApplication.Business.Reflection;
using MvcApplication.Models.ViewModels;

namespace MvcApplication.Controllers
{
	public class HomeController : Controller
	{
		#region Fields

		private static readonly ITreeFactory<FieldInfo> _fieldTreeFactory = new TreeFactory<FieldInfo>();
		private bool? _includeStaticFields;
		private const string _includeStaticFieldsKey = "IncludeStaticFields";
		private static readonly ISerializationResolver _serializationResolver = new SerializationResolver(new DefaultProxyBuilder(), new MemoryFormatterFactory()) {InvestigateSerializability = true};

		#endregion

		#region Properties

		protected internal virtual ITreeFactory<FieldInfo> FieldTreeFactory => _fieldTreeFactory;

		protected internal virtual bool IncludeStaticFields
		{
			get
			{
				if(this._includeStaticFields == null)
				{
					this._includeStaticFields = false;

					var includeStaticFieldsValue = this.Request.QueryString[this.IncludeStaticFieldsKey];

					if(includeStaticFieldsValue != null)
					{
						bool includeStaticFields;

						if(bool.TryParse(includeStaticFieldsValue, out includeStaticFields))
							this._includeStaticFields = includeStaticFields;
					}
				}

				return this._includeStaticFields.Value;
			}
		}

		protected internal virtual string IncludeStaticFieldsKey => _includeStaticFieldsKey;
		protected internal virtual ISerializationResolver SerializationResolver => _serializationResolver;

		#endregion

		#region Methods

		protected internal ITreeNode<FieldInfo> CreateTree()
		{
			var instance = System.Web.HttpContext.Current;
			var instances = new List<object> {instance};

			var tree = new TreeNode<FieldInfo>(null, new Field(instance.GetType(), "Root"), this.FieldTreeFactory);

			this.PopulateTree(instance, instances, tree);

			return tree;
		}

		public virtual ActionResult Index()
		{
			return this.View(new HomeViewModel
			{
				FieldTree = this.CreateTree(),
				IncludeStaticFields = this.IncludeStaticFields,
				IncludeStaticFieldsKey = this.IncludeStaticFieldsKey
			});
		}

		protected internal virtual bool IsAlreadyHandled(object instance, IList<object> instances)
		{
			if(instance == null)
				throw new ArgumentNullException(nameof(instance));

			if(instances == null)
				throw new ArgumentNullException(nameof(instances));

			if(instances.Contains(instance))
				return true;

			instances.Add(instance);

			return false;
		}

		protected internal virtual void PopulateTree(object instance, IList<object> instances, ITreeNode<FieldInfo> tree)
		{
			if(instance == null)
				throw new ArgumentNullException(nameof(instance));

			if(instances == null)
				throw new ArgumentNullException(nameof(instances));

			if(tree == null)
				throw new ArgumentNullException(nameof(tree));

			foreach(var field in this.SerializationResolver.GetFields(instance, this.IncludeStaticFields))
			{
				var child = tree.Children.Add(field);

				var value = field.GetValue(!field.IsStatic ? instance : null);

				if(value == null)
					continue;

				if(this.SerializationResolver.IsSerializable(value))
					continue;

				if(this.IsAlreadyHandled(value, instances))
					continue;

				this.PopulateTree(value, instances, child);
			}
		}

		#endregion
	}
}