using System.Runtime.InteropServices;

#nullable enable

namespace Sandbox;

/// <summary>
/// Helper to wrap <see cref="Marshal.GetFunctionPointerForDelegate"/> while keeping
/// a reference to the original delegate, so it won't be garbage collected. <see cref="Dispose"/>
/// must be called to remove the reference.
/// </summary>
internal struct DelegateFunctionPointer : IDisposable
{
	public static DelegateFunctionPointer Null => default;

	private nint _ptr;
	private Delegate _handle;

	/// <summary>
	/// Gets the raw function pointer.
	/// </summary>
	public static implicit operator nint( DelegateFunctionPointer fp )
	{
		return fp._ptr;
	}

	/// <inheritdoc cref="DelegateFunctionPointer"/>
	public static DelegateFunctionPointer Get<T>( T deleg ) where T : Delegate
	{
		return new DelegateFunctionPointer( Marshal.GetFunctionPointerForDelegate( deleg ), deleg );
	}

	private DelegateFunctionPointer( nint ptr, Delegate handle )
	{
		_ptr = ptr;
		_handle = handle;
	}

	/// <summary>
	/// Removes the reference to the original delegate, and sets the function pointer to null.
	/// </summary>
	public void Dispose()
	{
		_ptr = nint.Zero;
		_handle = () => { };
	}
}
