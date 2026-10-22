namespace Sandbox.Network;

/// <summary>
/// Can be implemented on a type so that when used in conjunction with <see cref="SyncAttribute">[Sync]</see> you
/// can write and read directly from delta snapshots.
/// </summary>
internal interface INetworkDeltaSnapshot
{
	/// <summary>
	/// Write to a <see cref="DeltaSnapshot"/>.
	/// </summary>
	/// <param name="slot">The parent slot in the network table for this property</param>
	/// <param name="snapshot">The snapshot we're writing to</param>
	void WriteSnapshotState( int slot, LocalSnapshotState snapshot );

	/// <summary>
	/// Read from a <see cref="DeltaSnapshot"/>.
	/// </summary>
	/// <param name="slot">The parent slot in the network table for this property</param>
	/// <param name="snapshot">The snapshot we're reading from</param>
	void ReadSnapshot( int slot, DeltaSnapshot snapshot );
}
