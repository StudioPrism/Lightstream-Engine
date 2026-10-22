using Mono.Cecil;
using System;

namespace Sandbox;

public partial class AccessControl
{
	private class AssemblyNameComparer : IEqualityComparer<AssemblyNameReference>
	{
		public static AssemblyNameComparer Instance { get; } = new AssemblyNameComparer();

		public bool Equals( AssemblyNameReference x, AssemblyNameReference y )
		{
			if ( ReferenceEquals( x, y ) )
			{
				return true;
			}

			if ( ReferenceEquals( x, null ) )
			{
				return false;
			}

			if ( ReferenceEquals( y, null ) )
			{
				return false;
			}

			return string.Equals( x.Name, y.Name, StringComparison.OrdinalIgnoreCase ) && x.Version.Equals( y.Version );
		}

		public int GetHashCode( AssemblyNameReference obj )
		{
			var hashCode = new HashCode();
			hashCode.Add( obj.Name, StringComparer.OrdinalIgnoreCase );
			hashCode.Add( obj.Version );
			return hashCode.ToHashCode();
		}
	}
}
