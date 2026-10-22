namespace Sandbox;

/// <summary>
/// Specifies who can control ownership of a networked object.
/// </summary>
[Expose]
public enum OwnerTransfer
{
	/// <summary>
	/// Anyone can control ownership.
	/// </summary>
	[Icon( "transfer_within_a_station" )]
	Takeover,

	/// <summary>
	/// Only the host can change the ownership.
	/// </summary>
	[Icon( "person" )]
	Fixed,

	/// <summary>
	/// Anyone can request ownership changes from the host.
	/// </summary>
	[Icon( "mail" )]
	Request
}
