using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sandbox
{
	internal class ReferenceComparer : IEqualityComparer<object>
	{
		public static IEqualityComparer<object> Singleton { get; } = new ReferenceComparer();

		private ReferenceComparer() { }

		bool IEqualityComparer<object>.Equals( object x, object y )
		{
			return ReferenceEquals( x, y );
		}

		public int GetHashCode( object obj )
		{
			return RuntimeHelpers.GetHashCode( obj );
		}
	}
}
