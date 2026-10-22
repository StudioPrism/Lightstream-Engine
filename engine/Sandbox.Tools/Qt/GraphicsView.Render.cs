using System;

namespace Editor
{
	internal enum RenderHints
	{
		Antialiasing = 0x01,
		TextAntialiasing = 0x02,
		SmoothPixmapTransform = 0x04,
		Qt4CompatiblePainting = 0x20,
		LosslessImageRendering = 0x40,
	};

	public partial class GraphicsView
	{
		public bool Antialiasing
		{
			set => _graphicsview.setRenderHint( RenderHints.Antialiasing, value );
		}

		public bool TextAntialiasing
		{
			set => _graphicsview.setRenderHint( RenderHints.TextAntialiasing, value );
		}

		public bool BilinearFiltering
		{
			set => _graphicsview.setRenderHint( RenderHints.SmoothPixmapTransform, value );
		}

	}
}
