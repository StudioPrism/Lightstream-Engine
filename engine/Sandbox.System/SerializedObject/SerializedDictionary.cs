namespace Sandbox;

internal class SerializedDictionary : SerializedCollection
{
	internal System.Collections.IDictionary dict;

	public override void SetTargetObject( object obj, SerializedProperty property )
	{
		base.SetTargetObject( obj, property );

		dict = obj as System.Collections.IDictionary;
		ArgumentNullException.ThrowIfNull( dict );
	}

	protected override void PrepareEnumerator()
	{
		if ( PropertyList is not null && PropertyList.Count == dict.Count )
			return;

		PropertyList = new List<SerializedProperty>();

		foreach ( var key in dict.Keys )
		{
			PropertyList.Add( new SerializedDictProperty( this, key ) );
		}
	}

	public override bool Remove( SerializedProperty property )
	{
		var index = PropertyList.IndexOf( property );

		if ( index == -1 )
		{
			return false;
		}

		return RemoveAt( index );
	}

	public override bool RemoveAt( object index )
	{
		if ( !dict.Contains( index ) )
			return false;

		NotePreChange();
		dict.Remove( index );
		OnEntryRemoved?.Invoke();
		NoteChanged();
		return true;
	}

	public override bool Add( object key, object value )
	{
		// invalid old key type
		if ( !Translation.TryConvert( ref key, KeyType ) )
			return false;

		if ( value is null && ValueType.IsValueType )
			value = Activator.CreateInstance( ValueType );

		// invalid value type
		if ( !Translation.TryConvert( ref value, ValueType ) )
			return false;

		if ( dict.Contains( key ) )
		{
			dict.Remove( key );
			// TODO - remove from PropertyList
		}

		var prop = new SerializedDictProperty( this, key );
		NotePreChange( prop );

		dict.Add( key, value );

		PropertyList.Add( prop );
		OnEntryAdded?.Invoke();
		NoteChanged( prop );
		return true;
	}

	/// <summary>
	/// Called when changing the key
	/// </summary>
	internal bool TryChangeKey( object key, object newKey )
	{
		// invalid old key type
		if ( !Translation.TryConvert( ref key, KeyType ) )
			return false;

		// invalid new key type
		if ( !Translation.TryConvert( ref newKey, KeyType ) )
			return false;

		// can't change to an existing one
		if ( dict.Contains( newKey ) ) return false;

		// can't change if they key doesn't exist, I dunno how this would happen
		if ( !dict.Contains( key ) ) return false;

		var v = dict[key];
		dict.Remove( key );
		dict[newKey] = v;
		return true;
	}

	/// <summary>
	/// If this is a dictionary, this will create a property to easily create a key
	/// </summary>
	public override SerializedProperty NewKeyProperty()
	{
		return new SerializedDictNewKey( this );
	}
}

file class SerializedDictProperty : SerializedProperty
{
	private SerializedProperty keyProperty;
	private SerializedDictionary dict;
	public object Key => keyProperty.GetValue<object>();

	public override string Name => $"{Key}";
	public override Type PropertyType => dict.ValueType;
	public override SerializedObject Parent => dict;

	public SerializedDictProperty( SerializedDictionary serializedList, object key )
	{
		this.dict = serializedList;
		keyProperty = new SerializedDictKey( serializedList, key );
	}

	public override SerializedProperty GetKey() => keyProperty;

	public override IEnumerable<Attribute> GetAttributes()
	{
		return dict?.ParentProperty?.GetAttributes() ?? Enumerable.Empty<Attribute>();
	}

	public override T GetValue<T>( T defaultValue = default )
	{
		if ( !dict.dict.Contains( Key ) )
			return default;

		return ValueToType<T>( dict.dict[Key], defaultValue );
	}

	public override void SetValue<T>( T value )
	{
		if ( Translation.TryConvert( value, PropertyType, out var converted ) )
		{
			dict.NotePreChange( this );
			dict.dict[Key] = converted;
			dict.NoteChanged( this );
		}
	}

	public override bool TryGetAsObject( out SerializedObject obj )
	{
		obj = dict.PropertyToObject?.Invoke( this ) ?? null;
		return obj is not null;
	}
}

file class SerializedDictKey : SerializedProperty
{
	private SerializedDictionary dict;
	public object Key;

	public override string Name => $"{Key}";
	public override Type PropertyType => dict.KeyType;
	public override SerializedObject Parent => dict;

	public SerializedDictKey( SerializedDictionary serializedList, object key )
	{
		this.dict = serializedList;
		this.Key = key;
	}

	public override IEnumerable<Attribute> GetAttributes()
	{
		return dict?.ParentProperty?.GetAttributes() ?? Enumerable.Empty<Attribute>();
	}

	public override T GetValue<T>( T defaultValue = default )
	{
		return ValueToType<T>( Key, defaultValue );
	}

	public override void SetValue<T>( T value )
	{
		if ( Translation.TryConvert( value, PropertyType, out var converted ) )
		{
			dict.NotePreChange( this );
			if ( dict.TryChangeKey( Key, converted ) )
			{
				Key = converted;
				dict.NoteChanged( this );
			}
		}
	}

	public override bool TryGetAsObject( out SerializedObject obj )
	{
		obj = dict.PropertyToObject?.Invoke( this ) ?? null;
		return obj is not null;
	}
}


file class SerializedDictNewKey : SerializedProperty
{
	private SerializedDictionary dict;
	public object Key;

	public override string Name => $"{Key}";
	public override Type PropertyType => dict.KeyType;
	public override SerializedObject Parent => dict;

	public SerializedDictNewKey( SerializedDictionary serializedList )
	{
		this.dict = serializedList;
	}

	public override IEnumerable<Attribute> GetAttributes()
	{
		return dict?.ParentProperty?.GetAttributes() ?? Enumerable.Empty<Attribute>();
	}

	public override T GetValue<T>( T defaultValue = default )
	{
		return ValueToType<T>( Key, defaultValue );
	}

	public override void SetValue<T>( T value )
	{
		if ( Translation.TryConvert( value, PropertyType, out var converted ) )
		{
			Key = converted;
		}
	}

	public override bool TryGetAsObject( out SerializedObject obj )
	{
		obj = dict.PropertyToObject?.Invoke( this ) ?? null;
		return obj is not null;
	}
}
