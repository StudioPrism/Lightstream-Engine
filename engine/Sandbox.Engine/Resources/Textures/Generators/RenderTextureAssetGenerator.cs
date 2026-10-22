using System.Threading;

namespace Sandbox.Resources;

/// <summary>
/// Provides a texture generator entry that returns the texture owned by a RenderTexture asset.
/// </summary>
[Title( "Render Texture Asset" )]
[Icon( "switch_video" )]
[ClassName( "rendertexture" )]
public sealed class RenderTextureAssetGenerator : TextureGenerator
{
	/// <summary>
	/// The render texture asset to reference.
	/// </summary>
	[Property]
	public RenderTextureAsset Asset { get; set; }

	protected override async ValueTask<Texture> CreateTexture( Options options, CancellationToken ct )
	{
		if ( Asset is null )
			return default;

		if ( options.Compiler is not null && !string.IsNullOrEmpty( Asset.ResourcePath ) )
		{
			options.Compiler.Context.AddCompileReference( Asset.ResourcePath );
		}

		await MainThread.Wait();
		ct.ThrowIfCancellationRequested();

		return Asset.Texture;
	}
}
