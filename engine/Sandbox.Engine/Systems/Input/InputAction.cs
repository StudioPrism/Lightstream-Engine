using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sandbox;

/// <summary>
/// An input action defined by a game project.
/// </summary>
[Expose]
public partial class InputAction
{
	public InputAction( string name, string keyboardCode, GamepadCode gamepadCode = GamepadCode.None, string groupName = "Other", string title = null )
	{
		Name = name;
		KeyboardCode = keyboardCode;
		GamepadCode = gamepadCode;
		GroupName = groupName;
		Title = title;
	}

	public InputAction()
	{
	}

	public InputAction( InputAction other )
	{
		Name = other.Name;
		KeyboardCode = other.KeyboardCode;
		GamepadCode = other.GamepadCode;
		GroupName = other.GroupName;
		Title = other.Title;
	}

	/// <summary>
	/// The name of the input action. Used by Input.Down|Pressed|Released.
	/// </summary>
	[RegularExpression( @"^[a-zA-Z0-9_\-]+$", ErrorMessage = "Lower or upper case letters and underscores, no spaces or other special characters" )]
	[Group( "Display" )]
	public string Name { get; set; }

	/// <summary>
	/// A group name for this input when showing in a binding system
	/// </summary>
	[Group( "Display" )]
	public string GroupName { get; set; } = "Other";

	/// <summary>
	/// A friendly name for this input action when showing in a binding system
	/// </summary>
	[Group( "Display" )]
	public string Title { get; set; }

	/// <summary>
	/// The key or key combo we'll be watching for.
	/// </summary>
	[Editor( "keybind" ), Group( "Keybinds" )]
	public string KeyboardCode { get; set; }

	/// <summary>
	/// What gamepad button should this action map to?
	/// </summary>
	[JsonIgnore( Condition = JsonIgnoreCondition.Never ), Group( "Keybinds" )]
	public GamepadCode GamepadCode { get; set; } = GamepadCode.None;
}
