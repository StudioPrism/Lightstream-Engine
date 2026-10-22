
namespace Sandbox;

[Flags, Expose]
public enum KeyboardModifiers : int
{
	None = 0,
	Alt = 1 << 0,
	Ctrl = 1 << 1,
	Shift = 1 << 2,
}
