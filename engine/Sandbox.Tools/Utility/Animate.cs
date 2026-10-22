using System;
using Sandbox.Utility;

namespace Editor;

public static class Animate
{
	class Entry
	{
		public object Owner;
		public float Start;
		public float End;
		public Action<float> Function;
	}

	static List<Entry> Entries = new List<Entry>();

	/// <summary>
	/// Add a float animation for this object
	/// </summary>
	public static void Add( object owningObject, float secondsToTake, float from, float to, Action<float> value, string ease = "ease-in-out" )
	{
		if ( from == to )
			return;

		var e = new Entry
		{
			Owner = owningObject,
			Start = RealTime.Now,
			End = RealTime.Now + secondsToTake
		};

		var easeFunction = Easing.GetFunction( ease );

		e.Function = delta =>
		{
			delta = easeFunction( delta );

			var f = delta.Remap( 0, 1, from, to );
			value( f );

			if ( owningObject is Widget w && w.IsValid() )
			{
				w.Update();
				w.Parent?.Update();
			}

		};

		Entries.Add( e );
	}

	/// <summary>
	/// Cancel all of this object's active animations
	/// </summary>
	public static void CancelAll( object owningObject, bool jumpToEnd )
	{
		for ( int i = Entries.Count - 1; i >= 0; i-- )
		{
			var e = Entries[i];
			if ( e.Owner != owningObject ) continue;

			Entries.RemoveAt( i );
		}
	}

	/// <summary>
	/// Returns true if this object has any active animations
	/// </summary>
	public static bool IsActive( object owningObject )
	{
		return Entries.Any( x => x.Owner == owningObject );
	}


	[EditorEvent.Frame]
	public static void Frame()
	{
		var t = RealTime.Now;

		for ( int i = Entries.Count - 1; i >= 0; i-- )
		{
			var e = Entries[i];

			if ( e.Owner is IValid v && !v.IsValid )
			{
				Entries.RemoveAt( i );
				continue;
			}

			var delta = t.Remap( e.Start, e.End, 0, 1 );

			if ( delta < 0 )
				continue;

			if ( delta >= 1.0f )
				delta = 1.0f;

			e.Function( delta );

			if ( delta >= 1.0f )
			{
				Entries.RemoveAt( i );
			}
		}
	}
}
