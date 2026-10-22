using Sandbox.Utility;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sandbox;

/// <summary>
/// Asset that owns a GPU render target texture which can be shared across runtime systems.
/// </summary>
[AssetType( Name = "Render Texture", Extension = "rtex", Category = "Rendering" )]
public sealed partial class RenderTextureAsset : GameResource
{
	private static readonly HashSet<ImageFormat> SupportedFormats = new()
	{
		ImageFormat.RGBA8888,
		ImageFormat.RGBA16161616F,
	};

	private Texture _texture;

	public Texture Texture => _texture;

	/// <summary>
	/// Resolution of the render target in pixels.
	/// </summary>
	[Property, Title( "Size (pixels)" )]
	public Vector2Int Size { get; set; } = new( 512, 512 );

	/// <summary>
	/// Color format used when building the render target. Unsupported formats fall back to RGBA8888.
	/// </summary>
	[Property, Title( "Color Format" )]
	public ImageFormat Format { get; set; } = ImageFormat.RGBA8888;

	/// <summary>
	/// Multisample anti-aliasing level for the render target.
	/// </summary>
	[Property, Title( "Multisampling" ), Hide]
	private MultisampleAmount Multisample { get; set; } = MultisampleAmount.MultisampleNone;

	/// <summary>
	/// Optional clear colour applied when the texture is (re)created.
	/// </summary>
	[Property, Title( "Initial Clear Color" )]
	public Color ClearColor { get; set; } = Color.Transparent;

	private void BuildTexture()
	{
		ThreadSafe.AssertIsMainThread();

		var width = Size.x;
		var height = Size.y;

		var builder = Texture.CreateRenderTarget()
			.WithSize( width, height )
			.WithFormat( EnsureSupportedFormat( Format ) )
			.WithMSAA( Multisample )
			.WithInitialColor( ClearColor )
			.WithUAVBinding( true )
			.WithMips();

		var debugName = string.IsNullOrEmpty( ResourcePath ) ? $"RenderTextureAsset:{GetHashCode()}" : ResourcePath;
		_texture = builder.Create( debugName );
	}

	private static ImageFormat EnsureSupportedFormat( ImageFormat format )
	{
		if ( SupportedFormats.Contains( format ) )
			return format;

		Log.Warning( $"RenderTextureAsset does not support {format}, falling back to {ImageFormat.RGBA8888}." );
		return ImageFormat.RGBA8888;
	}

	protected override void PostLoad()
	{
		base.PostLoad();
		BuildTexture();
	}

	protected override void PostReload()
	{
		base.PostReload();
		BuildTexture();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

		Texture?.Dispose();
		_texture = null;
	}

	protected override Bitmap CreateAssetTypeIcon( int width, int height )
	{
		const string svg = @"<svg viewBox='0 0 64 64' xmlns='http://www.w3.org/2000/svg'><rect x='6' y='12' width='52' height='40' rx='6' ry='6' fill='#3a8ee6'/><rect x='12' y='18' width='40' height='28' rx='4' ry='4' fill='#0f172a'/><path d='M38 33a8 8 0 1 1-16 0 8 8 0 0 1 16 0Z' fill='#3a8ee6'/><path d='M45 24h6v16h-6z' fill='#60a5fa'/></svg>";
		return Bitmap.CreateFromSvgString( svg, width, height );
	}
}
