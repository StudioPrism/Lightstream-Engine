namespace Sandbox;

public partial class Bitmap
{
	ImageFormat ImageFormat => IsFloatingPoint ? ImageFormat.RGBA16161616F : ImageFormat.RGBA8888;

	/// <summary>
	/// Try to create a texture from this bitmap
	/// </summary>
	public Texture ToTexture( bool mips = true )
	{
		var builder = Texture.Create( Width, Height )
					.WithFormat( ImageFormat )
					.WithData( _bitmap.GetPixels(), _bitmap.BytesPerPixel * Width * Height );

		if ( mips )
		{
			builder = builder.WithMips();
		}

		return builder.Finish();
	}
}
