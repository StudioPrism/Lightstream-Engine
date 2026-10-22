using System.Reflection;

namespace Sandbox;

/// <summary>
/// Utility info for tools usage.
/// </summary>
public static class Global
{
	internal static Assembly Assembly { get; set; }

	/// <summary>
	/// Is the local client in game or not.
	/// </summary>
	public static bool InGame { get; internal set; }

	/// <summary>
	/// Name of the map the local client is on.
	/// </summary>
	public static string MapName { get; internal set; } = "";

	/// <summary>
	/// Identity of the gamemode the local client is currently playing.
	/// </summary>
	public static string GameIdent { get; internal set; } = "";

	/// <summary>
	/// Front facing identity for the backend
	/// </summary>
	public static string BackendTitle => "sbox.game";

	/// <summary>
	/// Url for the backend
	/// </summary>
	public static string BackendUrl => "https://sbox.game";

	/// <summary>
	/// Are we connected to the API? (If not, offline mode. Requires Steam Servers to be online to connect..)
	/// </summary>
	public static bool IsApiConnected => Api.IsConnected;
}
