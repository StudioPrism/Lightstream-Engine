using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sandbox;

/// <summary>
/// Interface for objects that can become invalid over time,
/// such as references to deleted game objects or disposed resources.
/// </summary>
public interface IValid
{
	/// <summary>
	/// Returns true if this object is still valid and can be safely accessed.
	/// When false, accessing the object's properties or methods may throw exceptions.
	/// </summary>
	bool IsValid { get; }
}

public static partial class SandboxSystemExtensions
{
	/// <summary>
	/// Returns false if <see cref="IValid"/> object is null or if <see cref="IValid.IsValid"/> returns false.
	/// </summary>
#nullable enable
	[Pure, ActionGraphNode( "sys.isvalid" ), Icon( "assignment_turned_in" ), Category( "Object:data_object" )]
	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	public static bool IsValid( [NotNullWhen( true )] this IValid? obj ) => obj != null && obj.IsValid;
#nullable restore
}
