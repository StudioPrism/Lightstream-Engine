namespace Sandbox;

/// <summary>
/// A static class that handles user permissions.
/// </summary>
internal static class UserPermission
{
	private static FileWatch Watcher { get; set; }

	/// <summary>
	/// Represents a user in the user permissions config.
	/// </summary>
	private class User
	{
		public HashSet<string> Claims { get; init; } = new();
		public ulong SteamId { get; init; }
		public string Name { get; init; }
	}

	private static List<User> users = new();

	/// <summary>
	/// Get whether or not the specified <see cref="SteamId"/> has this permission.
	/// </summary>
	public static bool Has( SteamId steamId, string permission )
	{
		var user = users.FirstOrDefault( u => u.SteamId == steamId );
		return user?.Claims.Contains( permission ) ?? false;
	}

	/// <summary>
	/// Save user permissions to disk.
	/// </summary>
	internal static void Save()
	{
		var fs = EngineFileSystem.Config;
		fs.WriteJson( "users.json", users );
	}

	/// <summary>
	/// Load user permissions from disk. The file location should be <b>"config/users.json"</b>.
	/// </summary>
	internal static void Load()
	{
		if ( Watcher is not null )
		{
			Watcher.Dispose();
			Watcher = null;
		}

		var fs = EngineFileSystem.Config;

		if ( !LoadFromFile() )
			return;

		Watcher = fs.Watch( "/users.json" );
		Watcher.OnChanges += _ => LoadFromFile();
	}

	private static bool LoadFromFile()
	{
		var fs = EngineFileSystem.Config;

		// Nothing to do if the file does not exist.
		if ( !fs.FileExists( "users.json" ) )
			return false;

		try
		{
			users = fs.ReadJson<List<User>>( "users.json" );
		}
		catch ( Exception e )
		{
			Log.Error( e );
			Log.Warning( "The users.json permissions file is incorrectly formatted!" );
		}

		return true;
	}
}
