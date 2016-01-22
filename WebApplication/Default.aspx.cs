using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using HansKindberg.Serialization;

namespace WebApplication
{
	public partial class Default : Page
	{
		#region Fields

		private IEnumerable<FieldInfo> _fields;
		private bool? _includeStaticFields;
		private const string _includeStaticFieldsKey = "IncludeStaticFields";
		private static readonly SerializationResolver _serializationResolver = new SerializationResolver();

		#endregion

		#region Properties

		public virtual IEnumerable<FieldInfo> Fields => this.FieldsInternal.Any() ? this.FieldsInternal : null;
		protected internal virtual IEnumerable<FieldInfo> FieldsInternal => this._fields ?? (this._fields = this.SerializationResolver.GetFields(this.Instance, this.IncludeStaticFields).OrderBy(field => field.Name));

		public virtual bool IncludeStaticFields
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
		protected internal virtual object Instance => this.Context;
		public virtual Type InstanceType => this.Instance?.GetType();
		public virtual int NumberOfFields => this.FieldsInternal.Count();
		protected internal virtual SerializationResolver SerializationResolver => _serializationResolver;

		#endregion

		#region Methods

		#region Eventhandlers

		protected override void OnPreRender(EventArgs e)
		{
			this.FieldRepeater.DataBind();

			base.OnPreRender(e);
		}

		#endregion

		#endregion
	}
}