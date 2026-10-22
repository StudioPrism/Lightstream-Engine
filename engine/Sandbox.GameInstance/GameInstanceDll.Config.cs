using Sandbox.Utility;

namespace Sandbox;

internal partial class GameInstanceDll
{
	/// <summary>
	/// Read the json configurations from the network table and apply them.
	/// To encourage a single path, this is called on the host too - even if they're
	/// not hosting a multiplayer game!
	/// </summary>
	void UpdateConfigFromNetworkTable()
	{
		if ( ConfigTable.Entries.TryGetValue( "input", out var inputEntry ) )
		{
			var inputConfig = inputEntry.ReadJson<InputSettings>();
			Input.ReadConfig( inputConfig );
		}
	}

	/// <summary>
	/// Called when loading a game or when a localproject has been modified while playing a game.
	/// </summary>
	public void UpdateProjectConfig( Package package )
	{
		var inputSettings = ProjectSettings.Input;
		if ( inputSettings is null )
		{
			inputSettings = new InputSettings();
			inputSettings.InitDefault();
		}

		ConfigTable.SetJson( "input", inputSettings );

		UpdateConfigFromNetworkTable();
	}

	/// <summary>
	/// Called when the config for a game project has been changed. 
	/// We might need to update the config table.
	/// </summary>
	public void OnProjectConfigChanged( Package package )
	{
		if ( IGameInstance.Current is null )
			return;

		UpdateProjectConfig( package );
	}
}
