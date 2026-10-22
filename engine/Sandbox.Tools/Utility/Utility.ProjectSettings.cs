namespace Editor;

public static partial class EditorUtility
{
	/// <summary>
	/// Load a project settings file
	/// </summary>
	public static T LoadProjectSettings<T>( string filename ) where T : ConfigData, new()
	{
		var txt = FileSystem.ProjectSettings.ReadAllText( $"/{filename}" );
		var config = new T();

		if ( string.IsNullOrEmpty( txt ) )
			return config;

		config.Deserialize( txt );
		return config;
	}

	/// <summary>
	/// Save a project settings file
	/// </summary>
	public static void SaveProjectSettings<T>( T data, string filename ) where T : ConfigData, new()
	{
		FileSystem.ProjectSettings.WriteJson( $"/{filename}", data.Serialize() );
	}
}
