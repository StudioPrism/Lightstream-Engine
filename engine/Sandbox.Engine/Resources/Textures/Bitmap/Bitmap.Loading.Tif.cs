using SkiaSharp;

namespace Sandbox;

public partial class Bitmap
{
	public unsafe static Bitmap CreateFromTifBytes( byte[] data )
	{
		if ( !IsTif( data ) )
			return default;

		var fbm = FloatBitMap_t.Create();

		try
		{
			fixed ( byte* ptr = data )
			{
				var success = fbm.LoadFromInMemoryTIF( (IntPtr)ptr, data.Length );
				if ( !success ) return null;

				var bitmap = new SKBitmap( fbm.Width(), fbm.Height(), SKColorType.Rgba8888, SKAlphaType.Unpremul );

				fbm.WriteToBuffer( bitmap.GetPixels(), bitmap.ByteCount, ImageFormat.RGBA8888, false, false, 0 );

				return new Bitmap( bitmap );
			}
		}
		finally
		{
			fbm.Delete();
		}
	}

	private const ushort TiffBigEndian = 0x4D4D;
	private const ushort TiffLittleEndian = 0x4949;
	private const ushort TiffVersion = 42;

	public static bool IsTif( ReadOnlySpan<byte> data )
	{
		if ( data.Length < 8 )
			return false;

		ushort tiffMagic = ReadUInt16LittleEndian( data[..2] );
		ushort tiffVersion = ReadUInt16LittleEndian( data[2..4] );

		return tiffMagic switch
		{
			TiffBigEndian => SwapUInt16( tiffVersion ) == TiffVersion,
			TiffLittleEndian => tiffVersion == TiffVersion,
			_ => false
		};
	}

	private static ushort ReadUInt16LittleEndian( ReadOnlySpan<byte> bytes )
	{
		return (ushort)(bytes[0] | (bytes[1] << 8));
	}

	private static ushort SwapUInt16( ushort value )
	{
		return (ushort)((value >> 8) | (value << 8));
	}
}

