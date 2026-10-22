namespace Sandbox.DataModel;

/// <summary>
/// A <see cref="ConVarAttribute"/> that has been marked with <see cref="ConVarFlags.GameSetting"/>
/// This is stored as project metadata so we can set up a game without loading it.
/// </summary>
public record struct GameSetting( string Name, string Title, string Group, string Default = null )
{
	public record struct Option( string Name, string Icon );
	public List<Option> Options { get; set; }

	public float? Min { get; set; } = null;
	public float? Max { get; set; } = null;
	public float? Step { get; set; } = null;
}
