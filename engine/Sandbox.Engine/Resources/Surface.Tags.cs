using NativeEngine;

namespace Sandbox;

public partial class Surface
{
	/// <summary>
	/// Internal accessor for the surface's tags (stored as StringToken)
	/// </summary>
	private HashSet<uint> TagList = new();

	/// <summary>
	/// A list of tags as one string.
	/// </summary>
	[Category( "Tags" )]
	[Editor( "tags" )]
	public string Tags { get; set; }

	/// <summary>
	/// Do we have a tag?
	/// </summary>
	/// <param name="tag"></param>
	/// <returns></returns>
	public bool HasTag( string tag )
	{
		return TagList.Contains( StringToken.FindOrCreate( tag ) );
	}

	/// <summary>
	/// Do we have all the tags on this hitbox?
	/// </summary>
	/// <param name="tags"></param>
	/// <returns>True if all tags match, false if any tag does not match.</returns>
	public bool HasAllTags( params string[] tags )
	{
		foreach ( var tag in tags )
		{
			if ( !HasTag( tag ) )
				return false;
		}

		return true;
	}

	/// <summary>
	/// Do we have all the tags on this hitbox?
	/// </summary>
	/// <param name="tags"></param>
	/// <returns>True if any tag matches, false if all tags do not match.</returns>
	public bool HasAnyTags( params string[] tags )
	{
		foreach ( var tag in tags )
		{
			if ( HasTag( tag ) )
				return true;
		}

		return false;
	}
}
