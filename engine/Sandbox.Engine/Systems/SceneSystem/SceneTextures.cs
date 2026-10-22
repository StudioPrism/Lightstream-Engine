using System;

namespace Sandbox
{
	internal class SceneTextures : IDisposable
	{
		public int MultisampleAmount { get; private set; }
		public Vector2 Size { get; set; }
		public Texture Color { get; set; }
		public Texture Depth { get; set; }

		public SceneTextures( Vector2 size, bool isHDRBuffer = true, bool hasMultiSample = false )
		{
			Size = size;

			var textureBuilder = Texture.CreateRenderTarget()
				.WithSize( Size )
				.WithFormat( isHDRBuffer ? ImageFormat.RGBA16161616F : ImageFormat.RGBA8888 );

			if ( hasMultiSample ) textureBuilder = textureBuilder.WithScreenMultiSample();

			Color = textureBuilder.Create();

			MultisampleAmount = (int)Color.MultisampleType;

			textureBuilder = Texture.CreateRenderTarget()
				.WithSize( Size )
				.WithDepthFormat();

			if ( hasMultiSample ) textureBuilder = textureBuilder.WithScreenMultiSample();

			Depth = textureBuilder.Create();
		}


		~SceneTextures()
		{
			Dispose();
		}

		public void Dispose()
		{
			Color?.Dispose();
			Color = null;

			Depth?.Dispose();
			Depth = null;
		}
	}
}
