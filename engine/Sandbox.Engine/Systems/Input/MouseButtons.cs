
namespace Sandbox;

/// <summary>
/// State of mouse buttons being pressed or not.
/// </summary>
[Flags, Expose]
public enum MouseButtons
{
	// Note that these values line up with Qt::MouseButtons
	// so if we change them we should go around making a translator for it

	/// <summary>
	/// No buttons are being pressed.
	/// </summary>
	None = 0x00000000,

	/// <summary>
	/// Left mouse button is being pressed.
	/// </summary>
	Left = 0x00000001,

	/// <summary>
	/// Right mouse button is being pressed.
	/// </summary>
	Right = 0x00000002,

	/// <summary>
	/// Middle mouse button (mouse wheel) is being pressed in.
	/// </summary>
	Middle = 0x00000004,

	/// <summary>
	/// The "back" mouse button (mouse4) being pressed in.
	/// </summary>
	Back = 0x00000008,

	/// <summary>
	/// The "forward" mouse button (mouse5) being pressed in.
	/// </summary>
	Forward = 0x00000010,
	//	Task = 0x00000020,

}
