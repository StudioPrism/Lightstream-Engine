namespace Sandbox;


public abstract class SerializedCollection : SerializedObject
{
	public Type KeyType { get; internal set; }
	public Type ValueType { get; internal set; }

	public Action OnEntryAdded;
	public Action OnEntryRemoved;

	object _targetObject;

	public object TargetObject => _targetObject;

	public Func<SerializedProperty, SerializedObject> PropertyToObject;

	public virtual void SetTargetObject( object obj, SerializedProperty property )
	{
		_targetObject = obj;
		ParentProperty = property;
	}

	public virtual bool Remove( SerializedProperty property ) => false;

	public virtual bool RemoveAt( object index ) => false;

	public virtual bool Add( object value ) => false;

	public virtual bool Add( object key, object value ) => false;

	public virtual SerializedProperty NewKeyProperty() => null;

	internal static SerializedCollection Create( Type type )
	{
		if ( IsDictionaryType( type, out var keyType, out var valueType ) )
		{
			return new SerializedDictionary
			{
				KeyType = keyType,
				ValueType = valueType
			};
		}

		if ( type.IsSZArray && type.IsAssignableTo( typeof( Array ) ) )
		{
			return new SerializedArray
			{
				KeyType = typeof( int ),
				ValueType = type.GetElementType()
			};
		}

		if ( IsListType( type, out var listElemType ) )
		{
			return new SerializedList
			{
				KeyType = typeof( int ),
				ValueType = listElemType
			};
		}

		if ( IsSetType( type, out var setElemType ) )
		{
			return new SerializedSet
			{
				KeyType = setElemType,
				ValueType = setElemType
			};
		}

		return null;
	}

	private static bool IsDictionaryType( Type type, out Type keyType, out Type valueType )
	{
		keyType = null;
		valueType = null;

		if ( !type.IsAssignableTo( typeof( System.Collections.IDictionary ) ) )
		{
			return false;
		}

		foreach ( var @interface in type.GetInterfaces() )
		{
			if ( !@interface.IsConstructedGenericType ) continue;
			if ( @interface.GetGenericTypeDefinition() != typeof( IDictionary<,> ) ) continue;

			keyType = @interface.GetGenericArguments()[0];
			valueType = @interface.GetGenericArguments()[1];

			return true;
		}

		return false;
	}

	private static bool IsListType( Type type, out Type valueType )
	{
		valueType = null;

		if ( !type.IsAssignableTo( typeof( System.Collections.IList ) ) )
		{
			return false;
		}

		foreach ( var @interface in type.GetInterfaces() )
		{
			if ( !@interface.IsConstructedGenericType ) continue;
			if ( @interface.GetGenericTypeDefinition() != typeof( IList<> ) ) continue;

			valueType = @interface.GetGenericArguments()[0];

			return true;
		}

		return false;
	}

	private static bool IsSetType( Type type, out Type valueType )
	{
		valueType = null;

		// Early out if this isn't even an enumerable
		// Unfortunately, there is no non-generic System.Collections.ISet
		if ( !type.IsAssignableTo( typeof( System.Collections.IEnumerable ) ) )
		{
			return false;
		}

		foreach ( var @interface in type.GetInterfaces() )
		{
			if ( !@interface.IsConstructedGenericType ) continue;
			if ( @interface.GetGenericTypeDefinition() != typeof( ISet<> ) ) continue;

			valueType = @interface.GetGenericArguments()[0];
			return true;
		}

		return false;
	}
}
