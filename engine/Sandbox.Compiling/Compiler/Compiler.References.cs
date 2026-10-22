namespace Sandbox;

partial class Compiler
{
	/// <summary>
	/// References needed for compile
	/// </summary>
	HashSet<string> _references { get; } = new( StringComparer.OrdinalIgnoreCase )
	{
		"Sandbox.System",
		"Sandbox.Engine",
		"Sandbox.Filesystem",
		"Sandbox.Reflection",
		"Sandbox.Mounting",
		"Microsoft.AspNetCore.Components", // razor mockups
	};

	/// <summary>
	/// Add a reference to this compiler. This might be a system dll, or an assembly name from a fellow compiler.
	/// </summary>
	public void AddReference( string referenceName )
	{
		if ( string.IsNullOrWhiteSpace( referenceName ) )
			throw new ArgumentException( $"cannot be null or empty", nameof( referenceName ) );

		_references.Add( referenceName );
	}

	/// <summary>
	/// Returns true if <see cref="_references"/> contains the given reference assembly name.
	/// If <paramref name="deep"/> is true, referenced compilers are searched too.
	/// </summary>
	public bool HasReference( string referenceName, bool deep = false ) => FindReference( referenceName, deep );

	private bool FindReference( string assemblyName = null, bool deep = false, HashSet<Compiler> visited = null )
	{
		if ( assemblyName != null && string.Equals( AssemblyName, assemblyName, StringComparison.OrdinalIgnoreCase ) )
			return true;

		if ( !deep )
			return false;

		visited ??= new HashSet<Compiler>();

		if ( !visited.Add( this ) )
		{
			return false;
		}

		foreach ( var r in _references )
		{
			var c = Group.FindCompilerByAssemblyName( r );

			if ( c?.FindReference( assemblyName, true, visited ) ?? false )
			{
				return true;
			}
		}

		return false;
	}
}
