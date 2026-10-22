namespace Sandbox.Network;

[Expose]
public enum LobbyPrivacy
{
	/// <summary>
	/// This lobby is open to everyone.
	/// </summary>
	[Title( "Public" ), Icon( "public" )]
	Public,

	/// <summary>
	/// Nobody can join this lobby unless they are invited.
	/// </summary>
	[Title( "Private" ), Icon( "lock" )]
	Private,

	/// <summary>
	/// Only friends can join this lobby.
	/// </summary>
	[Title( "Friends Only" ), Icon( "people" )]
	FriendsOnly
}
