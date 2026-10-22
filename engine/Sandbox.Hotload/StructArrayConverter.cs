using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sandbox
{
	internal abstract class StructArrayConverter : IDisposable
	{
		private delegate void MemmoveDelegate( ref byte dest, ref byte src, nuint len );

		private static MemmoveDelegate MemmoveImpl { get; }

		static StructArrayConverter()
		{
			try
			{
				var memMoveFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
				var memMoveArgTypes = typeof( MemmoveDelegate )
					.GetMethod( "Invoke" )!
					.GetParameters()
					.Select( x => x.ParameterType )
					.ToArray();

				var memmoveNet9 = typeof( Buffer ).GetMethod( "_Memmove", memMoveFlags, memMoveArgTypes );
				var memmoveNet7 = typeof( Buffer ).GetMethod( "Memmove", memMoveFlags, memMoveArgTypes );
				var memmoveNet10 = typeof( Buffer ).GetMethod( "MemmoveInternal", memMoveFlags, memMoveArgTypes );

				MemmoveImpl = (memmoveNet10 ?? memmoveNet9 ?? memmoveNet7)?.CreateDelegate<MemmoveDelegate>();
			}
			catch
			{
				//
			}
		}

		public static bool CanCopyHigherRankArrays => MemmoveImpl is not null;

		protected static void Memmove( ref byte dst, ref byte src, nuint len )
		{
			MemmoveImpl( ref dst, ref src, len );
		}

		protected int ElementSize { get; }

		public static StructArrayConverter Create( Type srcType, Type dstType )
		{
			var genType = typeof( StructArrayConverter<,> );
			var conType = genType.MakeGenericType( srcType, dstType );
			return (StructArrayConverter)Activator.CreateInstance( conType );
		}

		protected StructArrayConverter( Type srcType, int srcSize, Type dstType, int dstSize )
		{
			if ( srcSize != dstSize )
			{
				throw new Exception( $"Cannot construct a {nameof( StructArrayConverter )} with type arguments of " +
					$"{srcType} and {dstType} - structures are different sizes ({srcSize} vs {dstSize})." );
			}

			ElementSize = srcSize;
		}

		public void BlockCopy( Array src, Array dst, int count )
		{
			OnBlockCopy( src, dst, count );
		}

		protected abstract void OnBlockCopy( Array src, Array dst, int count );

		public void Dispose()
		{
			// 
		}
	}

	internal class StructArrayConverter<TSrc, TDst> : StructArrayConverter
		where TSrc : struct
		where TDst : struct
	{
		public StructArrayConverter() : base( typeof( TSrc ), Unsafe.SizeOf<TSrc>(), typeof( TDst ), Unsafe.SizeOf<TDst>() ) { }

		protected override void OnBlockCopy( Array src, Array dst, int count )
		{
			if ( src.Rank != dst.Rank )
			{
				throw new NotImplementedException( "Arrays must have same rank." );
			}

			if ( src.Length < count )
			{
				throw new ArgumentException( "Source array is too small.", nameof( src ) );
			}

			if ( dst.Length < count )
			{
				throw new ArgumentException( "Destination array is too small.", nameof( dst ) );
			}

			var byteCount = (uint)count * (nuint)ElementSize;

			ref var srcDataRef = ref MemoryMarshal.GetArrayDataReference( src );
			ref var dstDataRef = ref MemoryMarshal.GetArrayDataReference( dst );

			Memmove( ref dstDataRef, ref srcDataRef, byteCount );
		}
	}
}
