namespace Sandbox.VR;

[Flags]
internal enum VREye
{
	Left = 1 << 1,
	Right = 1 << 2,
	Both = Left | Right
}
