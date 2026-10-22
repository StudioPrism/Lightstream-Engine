namespace Sandbox.Network;

public struct ConnectionStats
{
	/// <summary>
	/// Current ping for this connection.
	/// </summary>
	public int Ping { get; set; }

	/// <summary>
	/// How many packets per second we're sending to this connection.
	/// </summary>
	public float OutPacketsPerSecond { get; set; }

	/// <summary>
	/// How many bytes per second we're sending to this connection.
	/// </summary>
	public float OutBytesPerSecond { get; set; }

	/// <summary>
	/// How many packets per second we're receiving from this connection.
	/// </summary>
	public float InPacketsPerSecond { get; set; }

	/// <summary>
	/// How many bytes per second we're receiving from this connection.
	/// </summary>
	public float InBytesPerSecond { get; set; }

	/// <summary>
	/// Estimate rate that we believe we can send data to this connection.
	/// </summary>
	public int SendRateBytesPerSecond { get; set; }

	/// <summary>
	/// From 0 to 1 how good is our connection to this?
	/// </summary>
	public float ConnectionQuality { get; set; }

	private string Name { get; set; }

	internal ConnectionStats( string name )
	{
		Name = name;
	}

	public override string ToString()
	{
		if ( string.IsNullOrEmpty( Name ) )
			return $"ConnectionStats( OutBps: {OutBytesPerSecond}, InBps: {InBytesPerSecond}, Quality: {ConnectionQuality}";
		else
			return $"ConnectionStats( {Name}, OutBps: {OutBytesPerSecond}, InBps: {InBytesPerSecond}, Quality: {ConnectionQuality}";
	}
}
