using System.Reflection;
using System.Runtime.CompilerServices;

namespace Sandbox;

public static partial class SandboxSystemExtensions
{
	private static readonly ReflectionCache<Type, int> _managedSizeCache = new( ComputeManagedSize,
		new KeyValuePair<Type, int>( typeof( void ), 0 ) );

	/// <summary>
	/// Get the managed size of a given type. This matches an IL-level sizeof(t), even if it cannot be determined normally in C#.
	/// Note that <c>sizeof(t) != Marshal.SizeOf(t)</c> when t is char or bool.
	/// </summary>
	/// <remarks>
	/// An IL-level <c>sizeof(t)</c> will return <c>sizeof(IntPtr)</c> for reference types, as it refers to the size on stack or in an object,
	/// not the size on heap.
	/// </remarks>
	public static int GetManagedSize( this Type t ) => _managedSizeCache[t];

	private static MethodInfo _getManagedSizeHelper;

	private static int ComputeManagedSize( Type t )
	{
		_getManagedSizeHelper ??= typeof( Unsafe ).GetMethod( nameof( Unsafe.SizeOf ) )!;

		return _getManagedSizeHelper.MakeGenericMethod( t ).CreateDelegate<Func<int>>()();
	}
}
