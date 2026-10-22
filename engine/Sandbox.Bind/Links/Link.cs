using Sandbox.Diagnostics;

namespace Sandbox.Bind;

/// <summary>
/// Joins two proxies together, so one can be updated from the other (or both from each other)
/// </summary>
public sealed class Link
{
	static double timeOffset;

	/// <summary>
	/// This is updated in tick. Will return false if either binding is invalid. Bindings become
	/// invalid if the object is garbage collected or is an IValid and made invalid.
	/// </summary>
	public bool IsValid { get; private set; }

	/// <summary>
	/// True if this should only update from left to right.
	/// </summary>
	public bool OneWay { get; private set; }

	/// <summary>
	/// The primary binding. Changes to this value always take priority over the other.
	/// </summary>
	public Proxy Left { get; private set; }

	/// <summary>
	/// The secondary binding, if we're OneWay then this will only ever be written to.
	/// </summary>
	public Proxy Right { get; private set; }

	/// <summary>
	/// The next time this link is allowed to check
	/// </summary>
	double nextCall = -1.0;

	/// <summary>
	/// Called from manager for each link.
	/// It's this function's job to avoid calling Tick to save performance.
	/// </summary>
	internal bool Tick( double seconds, BindSystem manager )
	{
		// Throttling
		if ( seconds >= 0 )
		{
			if ( nextCall > seconds )
				return false;

			timeOffset += 1.0f / 300.0f;
			nextCall = seconds + (1.0f / 100.0f) + (timeOffset % 0.004);  // next call is slightly offset to avoid bunching
		}

		try
		{
			return Tick();
		}
		catch ( System.Exception e )
		{
			manager.Log.Warning( e, $"Removing link {this} due to exception" );
			IsValid = false;

			if ( !manager.CatchExceptions )
			{
				throw;
			}

			return false;
		}
	}

	public override string ToString() => $"{Left} <-> {Right}";

	internal Link( Proxy left, Proxy right, bool readOnly )
	{
		System.ArgumentNullException.ThrowIfNull( left, nameof( left ) );
		System.ArgumentNullException.ThrowIfNull( right, nameof( right ) );

		Left = left;
		Right = right;
		OneWay = readOnly;

		IsValid = Left.CanRead && Left.CanWrite;

		if ( !OneWay )
			IsValid = IsValid && Left.CanWrite;
	}

	bool Tick()
	{
		IsValid = Left.IsValid && Right.IsValid;
		if ( !IsValid ) return false;

		// Left check
		{
			if ( Left.IsChanged( out var value ) && Right.CanWrite )
			{
				//Logging.GetLogger( "Temp" ).Info( $"Left Changed: {Left} => {value} ({value?.GetType()}) => {Right}" );
				Right.SetValue( ref value );
				return true;
			}
		}

		// Right check
		{
			if ( !OneWay && Right.IsChanged( out var value ) && Left.CanWrite )
			{
				//Logging.GetLogger( "Temp" ).Info( $"Right Changed: {Right} => {value} ({value?.GetType()}) => {Left}" );
				Left.SetValue( ref value );
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// A value on our value has changed, which has changed our value. Replace both bindings
	/// with our value.
	/// </summary>
	internal void OnDownstreamChanged( object upstreamValue )
	{
		IsValid = Left.IsValid && Right.IsValid;
		if ( !IsValid ) return;

		if ( !OneWay )
			Left.SetValue( ref upstreamValue );

		Right.SetValue( ref upstreamValue );
	}

	internal bool ContainsObject( object obj, string property = null )
	{
		if ( Left is PropertyProxy leftB && leftB.HasSourceObject( obj, property ) )
			return true;

		if ( Right is PropertyProxy rightB && rightB.HasSourceObject( obj, property ) )
			return true;

		return false;
	}

	internal System.Attribute[] GetAttributes( object obj, string property )
	{
		if ( Left is PropertyProxy leftB && leftB.HasSourceObject( obj, property ) )
		{
			if ( Right is PropertyProxy t )
			{
				return t.PropertyInfo.GetCustomAttributes<System.Attribute>().ToArray();
			}
		}

		if ( Right is PropertyProxy rightB && rightB.HasSourceObject( obj, property ) )
		{
			if ( Left is PropertyProxy t )
			{
				return t.PropertyInfo.GetCustomAttributes<System.Attribute>().ToArray();
			}
		}

		return null;
	}
}
