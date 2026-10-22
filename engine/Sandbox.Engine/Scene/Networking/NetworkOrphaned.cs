namespace Sandbox;

/// <summary>
/// Specifies what happens when the owner of a networked object disconnects.
/// </summary>
[Expose]
public enum NetworkOrphaned
{
	/// <summary>
	/// Destroy the networked object.
	/// </summary>
	[Icon( "delete" )]
	Destroy,

	/// <summary>
	/// Assign the host as the owner.
	/// </summary>
	[Icon( "person" )]
	Host,

	/// <summary>
	/// Randomly assign another connection as the owner.
	/// </summary>
	[Icon( "shuffle" )]
	Random,

	/// <summary>
	/// Clear the owner of the networked object.
	/// </summary>
	[Icon( "clear" )]
	ClearOwner
}
