using SkiaSharp;

namespace Sandbox;

public partial class Bitmap
{
	/// <summary>
	/// Creates a Bitmap instance from PSD file data.
	/// </summary>
	/// <param name="data">Byte array containing the PSD file data.</param>
	/// <returns>A Bitmap instance if successful, or null if the data is not valid PSD.</returns>
	public unsafe static Bitmap CreateFromPsdBytes( byte[] data )
	{
		if ( !IsPsd( data ) )
			return default;

		FloatBitMap_t fbm = FloatBitMap_t.Create();

		try
		{
			fixed ( byte* ptr = data )
			{
				var success = fbm.LoadFromInMemoryPSD( (IntPtr)ptr, data.Length );
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
	/// Checks if the provided byte array is a valid PSD file.
	/// </summary>
	/// <param name="data">Byte array to check.</param>
	/// <returns>True if the data is a PSD file, otherwise false.</returns>
	public static bool IsPsd( byte[] data )
	{
		if ( data is null || data.Length < 26 ) return false;

		// PSD files start with the signature "8BPS" (38 42 50 53 in ASCII)
		return data[0] == '8' && data[1] == 'B' && data[2] == 'P' && data[3] == 'S';
	}
}

