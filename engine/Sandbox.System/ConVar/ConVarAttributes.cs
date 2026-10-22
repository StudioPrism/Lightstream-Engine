namespace Sandbox;

/// <summary>
/// Console variable
/// </summary>
[AttributeUsage( AttributeTargets.Property )]
[CodeGenerator( CodeGeneratorFlags.Static | CodeGeneratorFlags.WrapPropertySet, "Sandbox.ConsoleSystem.OnWrappedSet" )]
[CodeGenerator( CodeGeneratorFlags.Static | CodeGeneratorFlags.WrapPropertyGet, "Sandbox.ConsoleSystem.OnWrappedGet" )]
public class ConVarAttribute : Attribute
{
	/// <summary>
	/// If unset the name will be set to the name of the method/property
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// Describes why this command exists
	/// </summary>
	public string Help { get; set; }

	/// <summary>
	/// Minimum value for this command
	/// </summary>
	public float Min
	{
		get => MinValue ?? 0.0f;
		set => MinValue = value;
	}

	/// <summary>
	/// Maximum value for this command
	/// </summary>
	public float Max
	{
		get => MaxValue ?? 0.0f;
		set => MaxValue = value;
	}

	/// <summary>
	/// If true this variable is saved
	/// </summary>
	public bool Saved
	{
		get => Flags.Contains( ConVarFlags.Saved );
		set => Flags = Flags.WithFlag( ConVarFlags.Saved, value );
	}

	/// <summary>
	/// Describes the kind of convar this is
	/// </summary>
	public ConVarFlags Flags { get; set; }

	// Nullables cannot be used as attributes
	internal float? MinValue;
	internal float? MaxValue;

	/// <summary>
	/// If set to "menu" then this is a menu convar
	/// </summary>
	internal string Context { get; set; }

	public ConVarAttribute( string name = null, ConVarFlags flags = default )
	{
		Name = name;
		Flags = flags;
	}

	public ConVarAttribute( ConVarFlags flags )
	{
		Flags = flags;
	}
}

[Flags]
public enum ConVarFlags
{
	None = 0,

	/// <summary>
	/// Saved and restored between sessions
	/// </summary>
	Saved = 1,

	/// <summary>
	/// The value of this is synced on a server. Only the server or server admins may change the value.
	/// </summary>
	Replicated = 2,

	/// <summary>
	/// This is a cheat command, don't run it unless cheats are enabled
	/// </summary>
	Cheat = 4,

	/// <summary>
	/// Adds to userinfo - making it accessible via the connection class on other clients
	/// </summary>
	UserInfo = 8,

	/// <summary>
	/// Hide in find and autocomplete
	/// </summary>
	Hidden = 16,

	/// <summary>
	/// Tell clients when the value changes
	/// </summary>
	ChangeNotice = 32,

	/// <summary>
	/// Can't be accessed via game code (can be changed manually via console, or tools)
	/// </summary>
	Protected = 64,

	/// <summary>
	/// This command will be run on the server in a multiplayer game
	/// </summary>
	Server = 128,

	/// <summary>
	/// Only an admin of the server can run this command
	/// </summary>
	Admin = 256,

	/// <summary>
	/// A game setting that is exposed to the platform for UI editing
	/// </summary>
	GameSetting = 512,
}
