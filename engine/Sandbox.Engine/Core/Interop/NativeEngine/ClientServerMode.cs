using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace NativeEngine
{
	internal enum ClientServerMode
	{
		NONE = 0,
		SERVER = (1 << 0),
		CLIENT = (1 << 1),
		LISTENSERVER = (SERVER | CLIENT),
	}
}
