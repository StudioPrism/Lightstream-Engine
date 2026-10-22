using System;

namespace Sandbox
{
	[Flags]
	internal enum CollisionFunctionMask : byte
	{
		EnableSolidContact = 1 << 0,
		EnableTouchEvent = 1 << 1,
		EnableSelfCollisions = 1 << 2,
		EnableTouchPersists = 1 << 3
	}
}
