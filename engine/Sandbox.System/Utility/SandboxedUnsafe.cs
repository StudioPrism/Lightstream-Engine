using Sandbox;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

internal static class SandboxedUnsafe
{
	private static ReflectionCache<Type, bool> AcceptableTypeCache { get; } = new( IsAcceptableUncached );

	/// <summary>
	/// Return true if this is an acceptable Plain Old data
	/// </summary>
	internal static bool IsAcceptablePod( in Type t )
	{
		if ( !t.IsValueType ) return false;
		if ( t.IsPointer ) return false;
		if ( t.IsByRef ) return false;
		if ( t.IsClass ) return false;
		if ( t.IsInterface ) return false;
		if ( t == typeof( IntPtr ) || t == typeof( UIntPtr ) ) return false;
		if ( t.IsPrimitive ) return true;

		return AcceptableTypeCache[t];
	}

	/// <summary>
	/// Cached validation result per generic type - computed once per T
	/// </summary>
	private static class ValidationType<T> where T : unmanaged
	{
		public static readonly bool IsValid = IsAcceptablePod( typeof( T ) );
	}

	/// <summary>
	/// Return true if this is an acceptable Plain Old data
	/// </summary>
	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	internal static bool IsAcceptablePod<T>() where T : unmanaged => ValidationType<T>.IsValid;

	public static T Read<T>( ReadOnlySpan<byte> src ) where T : unmanaged
	{
		if ( !SandboxedUnsafe.IsAcceptablePod<T>() )
			return default;

		return MemoryMarshal.Read<T>( src );
	}

	internal static void Write<T>( Span<byte> dst, T arr ) where T : unmanaged
	{
		if ( !SandboxedUnsafe.IsAcceptablePod<T>() )
			throw new Exception( "Invalid Data Type" );

		MemoryMarshal.Write( dst, arr );
	}

	private static bool IsAcceptableUncached( Type t )
	{
		//Log.Info( $"Acceptable: {t} ?" );
		foreach ( var field in t.GetFields( System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public ) )
		{
			//Log.Info( $" - {field}" );
			if ( !IsAcceptablePod( field.FieldType ) )
			{
				return false;
			}
		}

		return true;
	}
}
