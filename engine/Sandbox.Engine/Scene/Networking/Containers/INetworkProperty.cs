namespace Sandbox;

/// <summary>
/// An interface for networked properties via <see cref="SyncAttribute"/>.
/// </summary>
internal interface INetworkProperty
{
	/// <summary>
	/// Called when initializing with a network table at the specified slot.
	/// </summary>
	/// <param name="slot">Our slot index in the network table</param>
	/// <param name="parent">The object we belong to</param>
	void Init( int slot, INetworkProxy parent );
}
