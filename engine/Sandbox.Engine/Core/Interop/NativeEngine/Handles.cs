using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeEngine
{
	internal struct InputContextHandle
	{
		public IntPtr pointer;
		public InputContextHandle( IntPtr p ) { pointer = p; }
	}
}
