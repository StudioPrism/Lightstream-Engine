using NativeEngine;

namespace Sandbox
{
	public partial class Texture
	{
		/// <summary>
		/// Begins creation of a custom texture. Finish by calling <see cref="TextureBuilder.Create"/>.
		/// </summary>
		public static TextureBuilder CreateCustom() => new();

		/// <summary>
		/// Begins creation of a custom texture. Finish by calling <see cref="Texture2DBuilder.Finish"/>.
		/// </summary>
		public static Texture2DBuilder Create( int width, int height, ImageFormat format = ImageFormat.RGBA8888 ) => new() { Width = width, Height = height, Format = format };

		/// <summary>
		/// Begins creation of a custom 3D texture. Finish by calling <see cref="Texture3DBuilder.Finish"/>.
		/// </summary>
		public static Texture3DBuilder CreateVolume( int width, int height, int depth, ImageFormat format = ImageFormat.RGBA8888 ) => new() { Width = width, Height = height, Depth = depth, Format = format };

		/// <summary>
		/// Begins creation of a custom cube texture. (A texture with 6 sides) Finish by calling <see cref="TextureCubeBuilder.Finish"/>.
		/// </summary>
		public static TextureCubeBuilder CreateCube( int width = 1, int height = 1, ImageFormat format = ImageFormat.RGBA8888 ) => new() { Width = width, Height = height, Format = format };

		/// <summary>
		/// Begins creation of a custom texture array. Finish by calling <see cref="TextureArrayBuilder.Finish"/>.
		/// </summary>
		public static TextureArrayBuilder CreateArray( int width = 1, int height = 1, int count = 1, ImageFormat format = ImageFormat.RGBA8888 ) => new() { Width = width, Height = height, Count = count, Format = format };

		/// <summary>
		/// Begins creation of a <a href="https://en.wikipedia.org/wiki/Render_Target">render target</a>. Finish by calling <see cref="TextureBuilder.Create"/>.
		/// </summary>
		/// <returns>The texture builder to help build the render target.</returns>
		public static TextureBuilder CreateRenderTarget()
		{
			var builder = new TextureBuilder();

			builder._config.m_nDepth = 1;

			if ( builder._config.m_nNumMipLevels <= 0 )
				builder._config.m_nNumMipLevels = 1;

			builder._config.m_nFlags |= RuntimeTextureSpecificationFlags.TSPEC_RENDER_TARGET;
			builder._config.m_nFlags |= RuntimeTextureSpecificationFlags.TSPEC_RENDER_TARGET_SAMPLEABLE;
			builder._config.m_nFlags |= RuntimeTextureSpecificationFlags.TSPEC_UAV;

			if ( builder._config.m_nNumMipLevels > 1 )
			{
				builder._config.m_nFlags |= RuntimeTextureSpecificationFlags.TSPEC_TEXTURE_GEN_MIP_MAPS;
			}

			builder._config.m_nUsage = TextureUsage.TEXTURE_USAGE_GPU_ONLY;
			builder._config.m_nScope = TextureScope.TEXTURE_SCOPE_GLOBAL;

			return builder;
		}

		static ImageFormat[] ValidRenderTargetFormats = new ImageFormat[]
		{
			ImageFormat.RGBA8888,
			ImageFormat.RGB565,
			ImageFormat.A8,
			ImageFormat.BGRA8888,
			ImageFormat.BGR565,
			ImageFormat.RGBA16161616F,
			ImageFormat.RGBA16161616,
			ImageFormat.RGBA8888_LINEAR,
			ImageFormat.BGRA8888_LINEAR,
			ImageFormat.BGRX8888_LINEAR,
			ImageFormat.D16,
			ImageFormat.D15S1,
			ImageFormat.D32,
			ImageFormat.D24S8,
			ImageFormat.LINEAR_D24S8,
			ImageFormat.D24X8,
			ImageFormat.D24X4S4,
			ImageFormat.D24FS8,
		};

		/// <summary>
		/// A convenience function to quickly create a <a href="https://en.wikipedia.org/wiki/Render_Target">render target</a>.
		/// </summary>
		/// <param name="name">A meaningless debug name for your texture.</param>
		/// <param name="format">The image format.</param>
		/// <param name="size">The size of the texture.</param>
		/// <returns>The newly created render target texture.</returns>
		public static Texture CreateRenderTarget( string name, ImageFormat format, Vector2 size )
		{
			if ( !ValidRenderTargetFormats.Contains( format ) )
			{
				Log.Warning( $"{format} is an invalid format for a render target - switching to ImageFormat.RGBA8888" );
				format = ImageFormat.RGBA8888;
			}

			return CreateRenderTarget().WithFormat( format ).WithSize( size ).WithInitialColor( Color.Magenta ).Create( name );
		}

		/// <summary>
		/// This will create a <a href="https://en.wikipedia.org/wiki/Render_Target">render target</a> texture if <paramref name="oldTexture"/> is null or doesn't match what you've passed in. This is designed
		/// to be called regularly to resize your texture in response to other things changing (like the screen size, panel size etc).
		/// </summary>
		/// <param name="name">A meaningless debug name for your texture.</param>
		/// <param name="format">The image format.</param>
		/// <param name="size">The size of the texture.</param>
		/// <param name="oldTexture">A previously created texture.</param>
		/// <returns>Will return a new texture, or the <paramref name="oldTexture"/>.</returns>
		public static Texture CreateRenderTarget( string name, ImageFormat format, Vector2 size, Texture oldTexture = null )
		{
			if ( oldTexture != null )
			{
				if ( oldTexture.ImageFormat == format && oldTexture.Size.AlmostEqual( size, 0.5f ) )
					return oldTexture;
			}

			return CreateRenderTarget( name, format, size );
		}
	}
}
