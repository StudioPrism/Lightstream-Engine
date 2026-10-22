namespace Sandbox;

/// <summary>
/// A class that holds all configured input settings for a game.
/// This is serialized as a config and shared from the server to the client.
/// </summary>
[Expose]
public class InputSettings : ConfigData
{
	public InputSettings()
	{
		Actions = new List<InputAction>();
		InitDefault();
	}

	public void InitDefault()
	{
		Actions.Clear();
		Actions.AddRange( Engine.Input.CommonInputs.Select( x => new InputAction( x ) ) );
	}

	/// <summary>
	/// A list of actions used by the game.
	/// </summary>
	public List<InputAction> Actions { get; set; }
}
