using System;
using System.Linq.Expressions;

namespace Sandbox.Bind;

/// <summary>
/// A helper to create binds between two properties (or whatever you want)
/// <para>
/// Example usage: set "BoolValue" from value of "StringValue"
/// <code>BindSystem.Build.Set( this, "BoolValue" ).From( this, "StringValue" );</code>
/// </para>
/// </summary>
public struct Builder
{
	internal BindSystem system;
	Proxy target;
	bool readOnly;

	/// <summary>
	/// Makes the bind link one way. The system will not try to write to the target/right hand property. (The one you set via "From" methods)
	/// </summary>
	public readonly Builder ReadOnly( bool makeReadOnly = true )
	{
		var t = this;
		t.readOnly = makeReadOnly;
		return t;
	}

	public readonly Builder Set<T>( T obj, string targetName, Action onChanged = null ) where T : class
	{
		try
		{
			return Set( PropertyProxy.Create( obj, targetName, onChanged ) );
		}
		catch ( MissingMemberException member )
		{
			system.Log.Warning( member );
			return this;
		}
	}

	/// <summary>
	/// Call this function when the Right hand changes. Stop updating when the object dies.
	/// </summary>
	public readonly Builder Set<T, U>( T obj, Func<U> read, Action<U> write ) where T : class
	{
		return Set( new MethodProxy<U>( obj, read, write ) );
	}

	public readonly Builder Set( Proxy binding )
	{
		var t = this;
		t.target = binding;
		return t;
	}

	public Link From<T>( T obj, PropertyInfo target ) where T : class
	{
		try
		{
			return From( new PropertyProxy( obj, target ) );
		}
		catch ( MissingMemberException member )
		{
			system.Log.Warning( member );
			return null;
		}
	}

	public Link From<T>( T obj, string targetName ) where T : class
	{
		try
		{
			return From( PropertyProxy.Create( obj, targetName ) );
		}
		catch ( MissingMemberException member )
		{
			system.Log.Warning( member );
			return null;
		}
	}

	/// <summary>
	/// Read and write the Right hand side via custom callbacks, rather than a specific property.
	/// </summary>
	/// <param name="read">Called to update the Left hand side. Return the target value.</param>
	/// <param name="write">Called to update the Right hand side. Do whatever you need with the provided value.</param>
	public Link From<T>( Func<T> read, Action<T> write )
	{
		return From( new MethodProxy<T>( read, write ) );
	}

	public Link From<T>( object sourceObject, Func<T> read, Action<T> write )
	{
		return From( new MethodProxy<T>( sourceObject, read, write ) );
	}

	public Link From<T, V>( T obj, Expression<Func<T, V>> propertyName ) where T : class
	{
		if ( propertyName.Body is System.Linq.Expressions.MemberExpression me )
		{
			if ( me.Member is PropertyInfo propInfo )
				return From( new PropertyProxy( obj, propInfo ) );

			system.Log.Warning( $"Couldn't create binding \"{me.Member}\" isn't a property" );
			return null;
		}

		system.Log.Warning( $"Couldn't create binding" );
		return null;
	}

	public Link From( Proxy source )
	{
		if ( source == null ) return null;
		if ( target == null ) return null;

		var link = new Link( source, target, readOnly );
		system.AddLink( link );
		return link;
	}

	public Link FromObject( object obj )
	{
		return From( new MethodProxy<object>( () => obj, null ) );
	}

	public Link FromDictionary<K, V>( Dictionary<K, V> dict, K key )
	{
		var read = () =>
		{
			if ( dict.TryGetValue( key, out var val ) )
				return val;

			return default;
		};

		var write = ( V v ) =>
		{
			dict[key] = v;
		};

		return From( read, write );
	}
}
