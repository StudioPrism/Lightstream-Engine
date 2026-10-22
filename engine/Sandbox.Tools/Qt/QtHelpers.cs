using Sandbox;

namespace Editor;

internal static class QtHelpers
{
	public static KeyboardModifiers Translate( QtKeyboardModifiers mods )
	{
		var o = KeyboardModifiers.None;

		if ( mods.Contains( QtKeyboardModifiers.ShiftModifier ) ) o |= KeyboardModifiers.Shift;
		if ( mods.Contains( QtKeyboardModifiers.AltModifier ) ) o |= KeyboardModifiers.Alt;
		if ( mods.Contains( QtKeyboardModifiers.ControlModifier ) ) o |= KeyboardModifiers.Ctrl;

		return o;
	}
}
