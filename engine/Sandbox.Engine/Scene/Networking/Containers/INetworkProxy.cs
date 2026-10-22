namespace Sandbox;

/// <summary>
/// An interface to implement to determine whether this object is a proxy.
/// </summary>
internal interface INetworkProxy
{
	public bool IsProxy { get; }
}
