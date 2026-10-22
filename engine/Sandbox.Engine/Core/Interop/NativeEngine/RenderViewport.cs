using Sandbox;

namespace NativeEngine
{
	internal struct RenderViewport
	{
		int m_nVersion { get; set; }
		int m_nTopLeftX { get; set; }
		int m_nTopLeftY { get; set; }
		int m_nWidth { get; set; }
		int m_nHeight { get; set; }
		public float MinZ { get; set; }
		public float MaxZ { get; set; }

		public RenderViewport( int x, int y, int w, int h )
		{
			m_nVersion = 1;
			m_nTopLeftX = x;
			m_nTopLeftY = y;
			m_nWidth = w;
			m_nHeight = h;
			MinZ = 0;
			MaxZ = 1;
		}

		public RenderViewport( in Rect rect )
		{
			m_nVersion = 1;
			m_nTopLeftX = (int)rect.Left;
			m_nTopLeftY = (int)rect.Top;
			m_nWidth = (int)rect.Right - (int)rect.Left;
			m_nHeight = (int)rect.Bottom - (int)rect.Top;
			MinZ = 0;
			MaxZ = 1;
		}

		public RenderViewport( in Rect rect, float minZ, float maxZ ) : this( rect )
		{
			MinZ = minZ;
			MaxZ = maxZ;
		}

		public Rect Rect
		{
			get => new Rect( m_nTopLeftX, m_nTopLeftY, m_nWidth, m_nHeight );
			set
			{
				m_nTopLeftX = (int)value.Left;
				m_nWidth = (int)value.Width;
				m_nTopLeftY = (int)value.Top;
				m_nHeight = (int)value.Height;
			}
		}

		public static RenderViewport operator /( RenderViewport viewport, float divisor )
		{
			return new RenderViewport(
				(int)(viewport.m_nTopLeftX / divisor),
				(int)(viewport.m_nTopLeftY / divisor),
				(int)(viewport.m_nWidth / divisor),
				(int)(viewport.m_nHeight / divisor)
			)
			{
				MinZ = viewport.MinZ,
				MaxZ = viewport.MaxZ
			};
		}
	}
}
