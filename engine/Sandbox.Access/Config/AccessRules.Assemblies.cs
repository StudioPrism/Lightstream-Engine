namespace Sandbox;

public partial class AccessRules
{
	public List<string> AssemblyWhitelist = new();

	void InitAssemblyList()
	{
		AssemblyWhitelist.Add( "System.Private.CoreLib" );
		AssemblyWhitelist.Add( "Sandbox.Engine" );
		AssemblyWhitelist.Add( "Sandbox.Reflection" );
		AssemblyWhitelist.Add( "Sandbox.Mounting" );
		AssemblyWhitelist.Add( "Microsoft.AspNetCore.Components" ); // this is our fake razor assembly
		AssemblyWhitelist.Add( "System.Text.Json" );
		AssemblyWhitelist.Add( "System.Numerics.Vectors" );
		AssemblyWhitelist.Add( "System.ComponentModel.Annotations" );
		AssemblyWhitelist.Add( "System.Runtime" );
		AssemblyWhitelist.Add( "Sandbox.System" );
		AssemblyWhitelist.Add( "Sandbox.Filesystem" );
		AssemblyWhitelist.Add( "System.Linq" );
		AssemblyWhitelist.Add( "System.Runtime.Extensions" );
		AssemblyWhitelist.Add( "System.Collections" );
		AssemblyWhitelist.Add( "System.Collections.Concurrent" );
		AssemblyWhitelist.Add( "System.Text.RegularExpressions" );
		AssemblyWhitelist.Add( "System.ComponentModel.Primitives" );
		AssemblyWhitelist.Add( "System.IO.Compression" );
		AssemblyWhitelist.Add( "System.Collections.Specialized" );
		AssemblyWhitelist.Add( "System.Web.HttpUtility" );
		AssemblyWhitelist.Add( "System.Private.Uri" );
		AssemblyWhitelist.Add( "System.ComponentModel.Primitives" );
		AssemblyWhitelist.Add( "System.ObjectModel" );
		AssemblyWhitelist.Add( "System.Collections.Immutable" );
		AssemblyWhitelist.Add( "System.Security.Cryptography" );
		AssemblyWhitelist.Add( "System.Threading.Channels" );
		AssemblyWhitelist.Add( "System.Threading" );
		AssemblyWhitelist.Add( "System.Memory" );
		AssemblyWhitelist.Add( "System.Net.Http" );
		AssemblyWhitelist.Add( "System.Net.Http.Json" );
		AssemblyWhitelist.Add( "System.Net.Primitives" );
	}
}
