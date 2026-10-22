using SkiaSharp;

namespace Sandbox;

public partial class Bitmap
{
	public unsafe static Bitmap CreateFromTgaBytes( byte[] data )
	{
		if ( !IsTga( data ) )
			return default;

		FloatBitMap_t fbm = FloatBitMap_t.Create();

		try
		{
			fixed ( byte* ptr = data )
			{
				var success = fbm.LoadFromInMemoryTGA( (IntPtr)ptr, data.Length );
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

	/// <summary>
	/// Return true if this data is a Tga file
	/// </summary>
	public static bool IsTga( byte[] data )
	{
		if ( data is null || data.Length < 18 ) return false;
		int imageType = data[2];

		if ( imageType == 2 || imageType == 3 || imageType == 10 )
		{
			int width = data[12] | (data[13] << 8);
			int height = data[14] | (data[15] << 8);
			return width > 0 && height > 0;
		}

		return false;
	}
}
