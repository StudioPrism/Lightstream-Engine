namespace Sandbox.Bind;

/// <summary>
/// Gets and Sets a value from somewhere.
/// </summary>
public abstract class Proxy
{
	/// <summary>
	/// The object to read data from and write data to.
	/// </summary>
	public WeakReference<object> Target { get; set; }

	/// <summary>
	/// Debug name for this property
	/// </summary>
	public string Name { get; protected set; }

	/// <summary>
	/// Get or set the value.
	/// </summary>
	public abstract object Value { get; set; }

	/// <summary>
	/// True if we can get the value.
	/// </summary>
	public abstract bool CanRead { get; }

	/// <summary>
	/// True if we can set the value
	/// </summary>
	public abstract bool CanWrite { get; }

	public override string ToString() => $"{Name} {GetType()}";

	// Special shit here..
	// -1 means unset
	// -2 means null
	int hash = -1;

	// We gotta do this separately, as hash of an integer is just the integer, so if its value is our special values -1 or -2 - shit breaks.
	bool unset = true;

	static int BuildObjectHash( ref object obj )
	{
		if ( obj == null ) return -2;
		if ( obj is string str ) return str.GetHashCode();
		if ( obj is Vector2 ) return obj.GetHashCode();
		if ( obj is Vector3 ) return obj.GetHashCode();
		if ( obj is Vector4 ) return obj.GetHashCode();
		if ( obj is Color ) return obj.GetHashCode();
		if ( obj is Angles ) return obj.GetHashCode();
		if ( obj is Rotation ) return obj.GetHashCode();
		if ( obj is Transform ) return obj.GetHashCode();

		if ( obj is IEnumerable list )
		{
			HashCode hc = new HashCode();

			var e = list.GetEnumerator();

			while ( e.MoveNext() )
			{
				var c = e.Current;
				hc.Add( BuildObjectHash( ref c ) );
			}

			return hc.ToHashCode();
		}

		var type = obj.GetType();

		if ( type.IsEnum ) return obj.GetHashCode();
		if ( type.IsPrimitive ) return obj.GetHashCode();
		if ( type.IsClass ) return obj.GetHashCode();

		//
		// For structs - lets encode them as json and compare the strings
		//
		if ( type.IsValueType )
		{
			var json = System.Text.Json.JsonSerializer.Serialize( obj );
			return json.FastHash();
		}

		return obj.GetHashCode();
	}

	internal bool UpdateHash( ref object value )
	{
		var code = BuildObjectHash( ref value );

		// if we were unset, and the first value is null
		// we just act as unset so that the other link will take
		// priority if it has a value
		if ( unset && value == null ) return false;

		if ( code == hash && !unset ) return false;

		hash = code;
		unset = false;

		return true;
	}

	internal void SetValue( ref object value )
	{
		if ( CanRead && Value == value )
			return;

		Value = value;

		//
		// If we can read, store the hash of the value that we stored.
		// this way if the objects are of different types, our hash will
		// match in the next think. If we don't do this the hash will ping
		// pong between the two types, as they will think they're changed.
		//
		if ( CanRead && (Value == null || value == null || Value.GetType() != value.GetType()) )
		{
			value = Value;
		}

		UpdateHash( ref value );
	}

	internal bool IsChanged( out object value )
	{
		if ( !CanRead )
		{
			value = default;
			return false;
		}

		value = Value;

		if ( !UpdateHash( ref value ) )
			return false;

		return true;
	}

	/// <summary>
	/// Should return <see langword="false"/> if the proxy is now invalid, like if the source object was destroyed.
	/// </summary>
	public virtual bool IsValid
	{
		get
		{
			// Target was never set - always valid
			if ( Target == null )
				return true;

			// Target is dead - invalid
			if ( !Target.TryGetTarget( out var obj ) )
				return false;

			// Target implements IValid and we can test whether it's destroyed or not
			if ( obj is IValid valid && !valid.IsValid )
			{
				return false;
			}

			// default, valid
			return true;
		}
	}
}
