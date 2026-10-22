using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
	partial class Hotload
	{
		internal class TestHelper
		{
			public static Func<T, bool> GenericMethod<T>( T arg )
			{
				return t => Equals( t, arg );
			}
		}
	}
}
