using System.IO;
using NativeEngine;

namespace Sandbox.Mounting;

public static partial class TextureLoader
{
	/// <summary>
	/// Creates a <see cref="Texture"/> from a DDS byte buffer.
	/// </summary>
	/// <param name="bytes">Raw DDS file contents.</param>
	/// <returns>A <see cref="Texture"/> with mips.</returns>
	public static Texture FromDds( ReadOnlySpan<byte> bytes )
	{
		var reader = new DDSReader( bytes );
		return Texture.Create( (int)reader.Width, (int)reader.Height, reader.Format )
			.WithData( reader.GetMipData( bytes ) )
			.WithMips( (int)reader.MipMapCount )
			.WithStaticUsage()
			.Finish();
	}
}

file class DDSReader
{
	public uint Width { get; private set; }
	public uint Height { get; private set; }
	public uint MipMapCount { get; private set; }
	public uint Depth { get; private set; }
	public ImageFormat Format { get; private set; }

	readonly string _fourCC;
	readonly bool _isDX10;
	readonly uint _dxgiFormat, _pixelFormatFlags, _rgbBitCount, _rBitMask, _gBitMask, _bBitMask, _aBitMask;
	readonly int _dataOffset;

	public unsafe DDSReader( ReadOnlySpan<byte> bytes )
	{
		if ( bytes.Length < 128 ) throw new InvalidDataException( "File too small for DDS header." );

		var offset = 0;
		fixed ( byte* p = bytes )
		{
			static uint ReadU32( byte* ptr, ref int off ) { uint v = *(uint*)(ptr + off); off += 4; return v; }

			if ( ReadU32( p, ref offset ) != 0x20534444u ) throw new InvalidDataException( "Not a valid DDS file." );
			if ( ReadU32( p, ref offset ) != 124 ) throw new InvalidDataException( "Invalid DDS header size." );

			var flags = ReadU32( p, ref offset );
			Height = ReadU32( p, ref offset );
			Width = ReadU32( p, ref offset );

			offset += 4;

			Depth = ((flags & 0x800000u) != 0) ? ReadU32( p, ref offset ) : 1u; offset += ((flags & 0x800000u) == 0) ? 4 : 0;
			MipMapCount = ((flags & 0x20000u) != 0) ? ReadU32( p, ref offset ) : 1u; offset += ((flags & 0x20000u) == 0) ? 4 : 0;

			offset += 44;

			if ( ReadU32( p, ref offset ) != 32 ) throw new InvalidDataException( "Invalid pixel format size." );

			_pixelFormatFlags = ReadU32( p, ref offset );
			_fourCC = FourCCToString( ReadU32( p, ref offset ) );
			_rgbBitCount = ReadU32( p, ref offset );
			_rBitMask = ReadU32( p, ref offset );
			_gBitMask = ReadU32( p, ref offset );
			_bBitMask = ReadU32( p, ref offset );
			_aBitMask = ReadU32( p, ref offset );

			offset += 20;
			_dataOffset = 128;

			if ( (_pixelFormatFlags & 0x4) != 0 && _fourCC == "DX10" )
			{
				_isDX10 = true;
				if ( bytes.Length < 148 ) throw new InvalidDataException( "File too small for DX10 header." );
				_dxgiFormat = ReadU32( p, ref offset );
				offset += 16;
				_dataOffset = 148;
			}
		}

		if ( bytes.Length < _dataOffset ) throw new InvalidDataException( "No data in DDS file." );
		Format = GetImageFormat();
	}

	static string FourCCToString( uint v ) => new( [(char)(v & 0xFF), (char)((v >> 8) & 0xFF), (char)((v >> 16) & 0xFF), (char)((v >> 24) & 0xFF)] );

	ImageFormat GetImageFormat()
	{
		if ( _isDX10 )
		{
			return _dxgiFormat switch
			{
				71u or 72u => ImageFormat.DXT1,
				74u or 75u => ImageFormat.DXT3,
				77u or 78u => ImageFormat.DXT5,
				80u => ImageFormat.ATI1N,
				83u => ImageFormat.ATI2N,
				95u => ImageFormat.BC6H,
				98u or 99u => ImageFormat.BC7,
				10u => ImageFormat.RGBA16161616F,
				11u => ImageFormat.RGBA16161616,
				2u => ImageFormat.RGBA32323232F,
				6u => ImageFormat.RGB323232F,
				16u => ImageFormat.RG3232F,
				34u => ImageFormat.R32F,
				41u => ImageFormat.R16F,
				54u => ImageFormat.RG1616F,
				56u => ImageFormat.R16,
				28u => ImageFormat.RGBA8888,
				87u => ImageFormat.BGRA8888,
				_ => throw new NotSupportedException( $"Unsupported DXGI format: {_dxgiFormat}" )
			};
		}

		if ( (_pixelFormatFlags & 0x4u) != 0 )
		{
			return _fourCC switch
			{
				"DXT1" => ImageFormat.DXT1,
				"DXT2" or "DXT3" => ImageFormat.DXT3,
				"DXT4" or "DXT5" => ImageFormat.DXT5,
				"ATI1" or "BC4U" or "BC4S" => ImageFormat.ATI1N,
				"ATI2" or "BC5U" or "BC5S" => ImageFormat.ATI2N,
				_ => throw new NotSupportedException( $"Unsupported FourCC: {_fourCC}" )
			};
		}

		if ( (_pixelFormatFlags & 0x40u) != 0 )
		{
			var hasAlpha = (_pixelFormatFlags & 0x1u) != 0;

			if ( hasAlpha && _rgbBitCount == 32 )
			{
				if ( _rBitMask == 0x00ff0000u && _gBitMask == 0x0000ff00u && _bBitMask == 0x000000ffu && _aBitMask == 0xff000000u ) return ImageFormat.BGRA8888;
				if ( _rBitMask == 0xff000000u && _gBitMask == 0x00ff0000u && _bBitMask == 0x0000ff00u && _aBitMask == 0x000000ffu ) return ImageFormat.ABGR8888;
				if ( _rBitMask == 0x000000ffu && _gBitMask == 0x0000ff00u && _bBitMask == 0x00ff0000u && _aBitMask == 0xff000000u ) return ImageFormat.RGBA8888;
				if ( _rBitMask == 0x0000ff00u && _gBitMask == 0x00ff0000u && _bBitMask == 0xff000000u && _aBitMask == 0x000000ffu ) return ImageFormat.ARGB8888;
				throw new NotSupportedException( "Unsupported 32-bit RGBA mask configuration." );
			}

			if ( !hasAlpha && _rgbBitCount == 24 )
			{
				if ( _rBitMask == 0x00ff0000u && _gBitMask == 0x0000ff00u && _bBitMask == 0x000000ffu ) return ImageFormat.BGR888;
				if ( _rBitMask == 0x000000ffu && _gBitMask == 0x0000ff00u && _bBitMask == 0x00ff0000u ) return ImageFormat.RGB888;
				throw new NotSupportedException( "Unsupported 24-bit RGB mask configuration." );
			}

			if ( _rgbBitCount == 16 && _rBitMask == 0xf800u && _gBitMask == 0x07e0u && _bBitMask == 0x001fu ) return ImageFormat.RGB565;
		}

		if ( (_pixelFormatFlags & 0x2u) != 0 && _rgbBitCount == 8 ) return ImageFormat.A8;

		if ( (_pixelFormatFlags & 0x20000u) != 0 )
		{
			var hasAlpha = (_pixelFormatFlags & 0x1u) != 0;
			if ( hasAlpha && _rgbBitCount == 16 ) return ImageFormat.IA88;
			if ( !hasAlpha && _rgbBitCount == 8 ) return ImageFormat.I8;
		}

		throw new NotSupportedException( "Unsupported pixel format." );
	}

	public byte[] GetMipData( ReadOnlySpan<byte> bytes )
	{
		var mipData = bytes[_dataOffset..];
		var mipCount = (int)MipMapCount;

		var mipSizes = new int[mipCount];
		var width = (int)Width;
		var height = (int)Height;
		var depth = (int)Depth;

		long totalBytes = 0;
		for ( var i = 0; i < mipCount; i++ )
		{
			var size = ImageLoader.GetMemRequired( width, height, depth, Format, false );
			mipSizes[i] = size;
			totalBytes += size;

			width = Math.Max( 1, width >> 1 );
			height = Math.Max( 1, height >> 1 );
			depth = Math.Max( 1, depth >> 1 );
		}

		if ( totalBytes > mipData.Length )
			throw new InvalidDataException( $"Truncated DDS: need {totalBytes}, have {mipData.Length}." );
		if ( totalBytes > int.MaxValue )
			throw new InvalidDataException( $"DDS too large: {totalBytes} > {int.MaxValue}." );

		var total = (int)totalBytes;
		var result = GC.AllocateUninitializedArray<byte>( total );

		for ( int i = 0, src = 0; i < mipCount; i++ )
		{
			var size = mipSizes[i];
			var dst = total - (src + size);
			mipData.Slice( src, size ).CopyTo( result.AsSpan( dst ) );
			src += size;
		}

		return result;
	}
}
