using NativeEngine;
using System;
using System.Runtime.InteropServices;

namespace NativeEngine
{
	[StructLayout( LayoutKind.Sequential )]
	internal struct AudioDeviceDesc
	{
		public bool IsAvailable;
		public bool IsDefault;
	}
}
