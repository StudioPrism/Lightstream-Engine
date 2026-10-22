namespace Sandbox;

/// <summary>
/// An interface for specifying how a custom type can be serialized and deserialized
/// over the network with support for only sending changes.
/// </summary>
internal interface INetworkSerializer
{
	/// <summary>
	/// Write any changes to the <see cref="ByteStream"/>. This is only applicable if
	/// the type that implements this also implements <see cref="INetworkReliable"/>.
	/// </summary>
	/// <param name="data"></param>
	void WriteChanged( ref ByteStream data );

	/// <summary>
	/// Write all data to the <see cref="ByteStream"/>.
	/// </summary>
	/// <param name="data"></param>
	void WriteAll( ref ByteStream data );

	/// <summary>
	/// Read data from a <see cref="ByteStream"/>.
	/// </summary>
	/// <param name="data"></param>
	void Read( ref ByteStream data );

	/// <summary>
	/// Whether we currently have changes (are we dirty?)
	/// </summary>
	bool HasChanges { get; }
}
