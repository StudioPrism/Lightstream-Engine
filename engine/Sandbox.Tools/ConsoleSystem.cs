namespace Editor;

[SkipHotload]
public static partial class ConsoleSystem
{
	/// <summary>
	/// Try to set a console variable. You will only be able to set variables that you have permission to set.
	/// </summary>
	public static void SetValue( string name, object value ) => ConVarSystem.SetValue( name, value?.ToString(), true );

	/// <summary>
	/// Get a convar value as a string
	/// </summary>
	public static string GetValue( string name, string defaultValue = null ) => ConVarSystem.GetValue( name, defaultValue, true );

	/// <summary>
	/// Get a convar value as an integer if possible.
	/// </summary>
	public static int GetValueInt( string name, int defaultValue = 0 ) => ConVarSystem.GetInt( name, defaultValue, true );

	/// <summary>
	/// Get a convar value as an float if possible.
	/// </summary>
	public static float GetValueFloat( string name, float defaultValue = 0.0f ) => ConVarSystem.GetFloat( name, defaultValue, true );

	/// <summary>
	/// Run this command. This should be a single command.
	/// </summary>
	public static void Run( string command )
	{
		// Tools can do anything, let them run any command
		ConVarSystem.Run( command );
	}
}
