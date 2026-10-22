using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace NativeEngine
{
	[StructLayout( LayoutKind.Sequential )]
	internal struct CBaseHandle
	{
		public int Hello;
	}
}
