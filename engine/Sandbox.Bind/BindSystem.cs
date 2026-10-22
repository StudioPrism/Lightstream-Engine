using Sandbox.Diagnostics;
using System.Text.Json.Serialization;

namespace Sandbox.Bind;

/// <summary>
/// Data bind system, bind properties to each other.
/// </summary>
public class BindSystem
{
	System.Diagnostics.Stopwatch timer = System.Diagnostics.Stopwatch.StartNew();
	internal Logger Log { get; set; }

	List<Link> Links = new List<Link>();

	/// <summary>
	/// The debug name given to this system (ie Tools, Client, Server)
	/// </summary>
	public string Name { get; init; }

	/// <summary>
	/// If true we'll throttle time between link change checks. This should
	/// always be enabled in game, for performance reasons.
	/// </summary>
	public bool ThrottleUpdates { get; internal set; }

	/// <summary>
	/// If true we'll catch and remove exceptions when testing links instead of
	/// propagating them to the Tick call.
	/// </summary>
	public bool CatchExceptions { get; internal set; }

	/// <summary>
	/// The current amount of active links
	/// </summary>
	public int LinkCount => Links.Count;

	internal BindSystem( string name )
	{
		Name = name;
		Log = new Logger( $"Bind-{Name}" );
	}

	bool running = false;

	/// <summary>
	/// Should be called every frame. Will run through the links and check
	/// for changes, then action those changes. Will also remove dead links.
	/// </summary>
	public void Tick()
	{
		var time = timer.Elapsed.TotalSeconds;
		if ( !ThrottleUpdates ) time = -1;

		for ( int i = 0; i < 3; i++ )
		{
			if ( !DoTick( time ) )
				return;
		}
	}

	bool DoTick( double time )
	{
		if ( running || Links.Count == 0 )
			return false;

		bool changes = false;

		try
		{
			running = true;
			for ( int i = Links.Count - 1; i >= 0; i-- )
			{
				var l = Links[i];

				changes = l.Tick( time, this ) || changes;

				if ( !l.IsValid )
				{
					Links.RemoveAt( i );
					continue;
				}
				;
			}

			return changes;
		}
		finally
		{
			running = false;
		}
	}

	/// <summary>
	/// Call a tick with no timer limits, forcing all pending actions to be actioned
	/// </summary>
	public void Flush()
	{
		DoTick( -1 );
	}

	/// <summary>
	/// A helper to create binds between two properties (or whatever you want)
	/// </summary>
	[JsonIgnore]
	[Hide]
	public Builder Build => new Builder { system = this };

	internal void AddLink( Link link )
	{
		Links.Add( link );
	}

	/// <summary>
	/// For this object, with this property, find the property
	/// that supplies it and return any attributes set on it.
	/// This is useful for editors to allow them to supply the correct
	/// editor, without having access to the property.
	/// </summary>
	public System.Attribute[] FindAttributes<T>( T obj, string property )
	{
		var link = Links.FirstOrDefault( x => x.ContainsObject( obj, property ) );
		if ( link == null ) return null;

		return link.GetAttributes( obj, property );
	}

}
