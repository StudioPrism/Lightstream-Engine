namespace Sandbox;

/// <summary>
/// Utility info for menu usage.
/// </summary>
public static class Global
{
	/// <summary>
	/// Are we connected to the API? (If not, offline mode. Requires Steam Servers to be online to connect..)
	/// </summary>
	public static bool IsApiConnected => Api.IsConnected;
}
