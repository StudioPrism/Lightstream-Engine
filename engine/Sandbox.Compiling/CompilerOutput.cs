using Microsoft.CodeAnalysis;

namespace Sandbox;

public class CompilerOutput
{
	public CompilerOutput( Compiler compiler )
	{
		Compiler = compiler;
	}

	/// <summary>
	/// True if the build succeeded
	/// </summary>
	public bool Successful { get; internal set; }

	/// <summary>
	/// The compiler that has produced this build
	/// </summary>
	public Compiler Compiler { get; }

	/// <summary>
	/// The version of the assembly
	/// </summary>
	public Version Version { get; set; }

	/// <summary>
	/// The [assembly].dll contents for this build
	/// </summary>
	public byte[] AssemblyData { get; internal set; }

	/// <summary>
	/// A code archive created during the compile
	/// </summary>
	public CodeArchive Archive { get; internal set; }

	/// <summary>
	/// The [assembly].xml contents for this build
	/// </summary>
	public string XmlDocumentation { get; internal set; }

	/// <summary>
	/// A list of diagnostics caused by the previous build
	/// </summary>
	public List<Microsoft.CodeAnalysis.Diagnostic> Diagnostics { get; } = new();

	/// <summary>
	/// If an exception happened during the build, it'll be available here
	/// </summary>
	public Exception Exception { get; internal set; }

	/// <summary>
	/// For referencing this assembly from another compiler.
	/// </summary>
	internal PortableExecutableReference MetadataReference { get; set; }
}
