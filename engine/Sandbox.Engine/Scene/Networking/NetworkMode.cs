namespace Sandbox;

/// <summary>
/// Specifies how a <see cref="GameObject"/> should be networked.
/// </summary>
[Expose]
public enum NetworkMode
{
	/// <summary>
	/// Never network this <see cref="GameObject"/>.
	/// </summary>
	[Title( "Never Network" )]
	[Icon( "wifi_off" )]
	Never,

	/// <summary>
	/// Network this <see cref="GameObject"/> as a single network object. Objects networked in this
	/// way can have an owner, and synchronized properties with <see cref="SyncAttribute"/>.
	/// </summary>
	[Title( "Network Object" )]
	[Icon( "wifi" )]
	Object,

	/// <summary>
	/// Network this <see cref="GameObject"/> to other clients as part of the <see cref="Scene"/> snapshot.
	/// </summary>
	[Title( "Network Snapshot" )]
	[Icon( "network_wifi_2_bar" )]
	Snapshot
}
