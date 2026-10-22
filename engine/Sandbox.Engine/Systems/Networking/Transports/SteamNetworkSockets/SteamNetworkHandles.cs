
namespace Sandbox.Network;

/// <summary>
/// A handle to a connection.
/// </summary>
internal struct HSteamNetConnection
{
	public uint Id;
	public override string ToString() => $"{Id}";
}

/// <summary>
/// A handle to a listen socket
/// </summary>
internal struct HSteamListenSocket
{
	public uint Id;
	public override string ToString() => $"{Id}";
}

/// <summary>
/// A handle to a poll group
/// </summary>
internal struct HSteamNetPollGroup
{
	public uint Id;
	public override string ToString() => $"{Id}";
}
