using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Castle.DynamicProxy;

namespace HansKindberg.Serialization
{
	public class SerializationResolver : ISerializationResolver
	{
		#region Fields

		private const BindingFlags _defaultBindings = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
		private static readonly IDictionary<Type, IEnumerable<FieldInfo>> _fieldsCache = new Dictionary<Type, IEnumerable<FieldInfo>>();
		private static readonly object _fieldsCacheLockObject = new object();

		private static readonly Type[] _serializableBaseTypes =
		{
			typeof(Serializable),
			typeof(ValueType)
		};

		private static readonly IDictionary<Type, bool> _serializableTypesCache = new Dictionary<Type, bool>();
		private static readonly object _serializableTypesCacheLockObject = new object();

		private static readonly Type[] _unserializableBaseTypes =
		{
			typeof(Delegate)
		};

		private static readonly Type[] _unserializableDeclaringTypes =
		{
			typeof(ListDictionary)
		};

		private static readonly Type[] _unserializableTypes =
		{
			typeof(HybridDictionary),
			typeof(ListDictionary),
			Type.GetType("System.Runtime.Remoting.Messaging.ServerObjectTerminatorSink", true),
			Type.GetType("System.Runtime.Remoting.Messaging.StackBuilderSink", true)
		};

		#endregion

		#region Constructors

		public SerializationResolver(IProxyBuilder proxyBuilder, IMemoryFormatterFactory memoryFormatterFactory)
		{
			if(proxyBuilder == null)
				throw new ArgumentNullException(nameof(proxyBuilder));

			if(memoryFormatterFactory == null)
				throw new ArgumentNullException(nameof(memoryFormatterFactory));

			this.MemoryFormatter = memoryFormatterFactory.Create();
			this.MemoryFormatterFactory = memoryFormatterFactory;
			this.ProxyBuilder = proxyBuilder;
		}

		#endregion

		#region Properties

		protected internal virtual BindingFlags DefaultBindings => _defaultBindings;

		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		protected internal virtual IDictionary<Type, IEnumerable<FieldInfo>> FieldsCache => _fieldsCache;

		protected internal virtual object FieldsCacheLockObject => _fieldsCacheLockObject;
		public virtual bool InvestigateSerializability { get; set; }
		protected internal virtual IMemoryFormatter MemoryFormatter { get; }
		protected internal virtual IMemoryFormatterFactory MemoryFormatterFactory { get; }
		protected internal virtual IProxyBuilder ProxyBuilder { get; }
		protected internal virtual IEnumerable<Type> SerializableBaseTypes => _serializableBaseTypes;
		protected internal virtual IDictionary<Type, bool> SerializableTypesCache => _serializableTypesCache;
		protected internal virtual object SerializableTypesCacheLockObject => _serializableTypesCacheLockObject;
		public virtual IList<SerializationFailure> SerializationFailures { get; } = new List<SerializationFailure>();
		protected internal virtual IEnumerable<Type> UnserializableBaseTypes => _unserializableBaseTypes;
		protected internal virtual IEnumerable<Type> UnserializableDeclaringTypes => _unserializableDeclaringTypes;
		protected internal virtual IEnumerable<Type> UnserializableTypes => _unserializableTypes;

		#endregion

		#region Methods

		protected internal virtual Type ConvertUninitializedObjectType(Type type)
		{
			if(type == null)
				return null;

			if(type.IsInterface)
				return this.ProxyBuilder.CreateInterfaceProxyTypeWithoutTarget(type, null, ProxyGenerationOptions.Default);

			if(type.IsAbstract)
				return this.ProxyBuilder.CreateClassProxyType(type, null, ProxyGenerationOptions.Default);

			return type;
		}

		public virtual object CreateUninitializedObject(Type type)
		{
			if(type == null)
				throw new ArgumentNullException(nameof(type));

			if(typeof(string).IsAssignableFrom(type))
				return string.Empty;

			try
			{
				return FormatterServices.GetUninitializedObject(this.ConvertUninitializedObjectType(type));
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "An uninitialized object of type \"{0}\" could not be created.", type.FullName), exception);
			}
		}

		public virtual IEnumerable<FieldInfo> GetFields(object instance, bool includeStaticFields)
		{
			if(instance == null)
				return Enumerable.Empty<FieldInfo>();

			var bindings = this.DefaultBindings;

			if(includeStaticFields)
				bindings = bindings | BindingFlags.Static;

			return this.GetFields(instance.GetType(), bindings);
		}

		public virtual IEnumerable<FieldInfo> GetFields(Type type, BindingFlags bindings)
		{
			if(type == null)
				return Enumerable.Empty<FieldInfo>();

			// ReSharper disable PossibleMultipleEnumeration

			IEnumerable<FieldInfo> fields;

			if(!this.FieldsCache.TryGetValue(type, out fields))
			{
				lock(this.FieldsCacheLockObject)
				{
					if(!this.FieldsCache.TryGetValue(type, out fields))
					{
						fields = this.GetFieldsInternal(type, bindings);
						this.FieldsCache.Add(type, fields);
					}
				}
			}

			return fields;

			// ReSharper restore PossibleMultipleEnumeration
		}

		protected internal virtual IEnumerable<FieldInfo> GetFieldsInternal(Type type, BindingFlags bindings)
		{
			return type?.GetFields(bindings).Concat(this.GetFields(type.BaseType, bindings)) ?? Enumerable.Empty<FieldInfo>();
		}

		protected internal virtual IEnumerable<Type> GetGenericArgumentsRecursive(Type type)
		{
			if(type == null)
				throw new ArgumentNullException(nameof(type));

			foreach(var genericArgument in type.GetGenericArguments())
			{
				yield return genericArgument;

				foreach(var childGenericArgument in this.GetGenericArgumentsRecursive(genericArgument))
				{
					yield return childGenericArgument;
				}
			}
		}

		public virtual bool IsSerializable(object instance)
		{
			if(instance == null)
				return true;

			var type = instance.GetType();

			if(this.InvestigateSerializability)
			{
				bool isSerializable;

				if(!this.SerializableTypesCache.TryGetValue(type, out isSerializable))
				{
					lock(this.SerializableTypesCacheLockObject)
					{
						if(!this.SerializableTypesCache.TryGetValue(type, out isSerializable))
						{
							try
							{
								this.MemoryFormatter.Serialize(instance);

								this.SerializableTypesCache.Add(type, true);

								return true;
							}
							catch(SerializationException originalSerializationException)
							{
								var serializationException = new SerializationException(string.Format(CultureInfo.InvariantCulture, "The type \"{0}\" could not be serialized.", instance.GetType().FullName), originalSerializationException);

								if(this.SerializationFailures.Count == int.MaxValue)
									this.SerializationFailures.RemoveAt(0);

								this.SerializationFailures.Add(new SerializationFailure {Type = instance.GetType(), SerializationException = serializationException});

								return false;
							}
						}
					}
				}
			}

			return this.IsSerializable(type);
		}

		protected internal virtual bool IsSerializable(Type type)
		{
			if(type == null)
				throw new ArgumentNullException(nameof(type));

			bool isSerializable;

			if(!this.SerializableTypesCache.TryGetValue(type, out isSerializable))
			{
				lock(this.SerializableTypesCacheLockObject)
				{
					if(!this.SerializableTypesCache.TryGetValue(type, out isSerializable))
					{
						isSerializable = this.IsSerializableInternal(type);
						this.SerializableTypesCache.Add(type, isSerializable);
					}
				}
			}

			return isSerializable;
		}

		protected internal virtual bool IsSerializableInternal(Type type)
		{
			if(type == null)
				throw new ArgumentNullException(nameof(type));

			if(!type.IsSerializable)
				return false;

			if(this.SerializableBaseTypes.Any(serializableBaseType => serializableBaseType.IsAssignableFrom(type)))
				return true;

			if(this.UnserializableBaseTypes.Any(unserializableBaseType => unserializableBaseType.IsAssignableFrom(type)))
				return false;

			if(this.UnserializableTypes.Any(unserializableType => unserializableType == type))
				return false;

			var declaringType = type.DeclaringType;

			// ReSharper disable ConditionIsAlwaysTrueOrFalse
			if(declaringType != null && this.UnserializableDeclaringTypes.Any(unserializableDeclaringType => unserializableDeclaringType == declaringType))
				return false;
			// ReSharper restore ConditionIsAlwaysTrueOrFalse

			if(type.HasElementType)
			{
				Type elementType = type.GetElementType();

				if(elementType != null && (elementType == typeof(object) || !elementType.IsSerializable))
					return false;
			}

			if(typeof(IEnumerable).IsAssignableFrom(type))
			{
				if(typeof(ArrayList).IsAssignableFrom(type) || typeof(Hashtable).IsAssignableFrom(type))
					return false;
			}

			if(!this.GetGenericArgumentsRecursive(type).All(genericArgument => genericArgument.IsSerializable))
				return false;

			return true;
		}

		#endregion
	}
}