using Sandbox;

namespace NativeEngine
{
	internal struct CTextureDesc
	{
		public short m_nWidth;
		public short m_nHeight;
		public short m_nDepth;     // Doubles as slice count if TSPEC_TEXTURE_ARRAY is specified. Cannot do arrays of volume textures
		public short m_nNumMipLevels;
		public TextureDecodingFlags m_nDecodeFlags;
		public ImageFormat m_nImageFormat;
		public RuntimeTextureSpecificationFlags m_nFlags;

		public short m_nDisplayRectWidth;  // Width of the sub-rect of the texture that should actually be displayed
		public short m_nDisplayRectHeight; // Height of the sub-rect of the texture that should actually be displayed
		public short m_nMotionVectorsMaxDistanceInPixels;  // For motion vectors, the maximum distance that can be displaced per-frame

		public readonly bool IsArray => (m_nFlags & RuntimeTextureSpecificationFlags.TSPEC_TEXTURE_ARRAY) != 0;
		public readonly bool IsCube => (m_nFlags & RuntimeTextureSpecificationFlags.TSPEC_CUBE_TEXTURE) != 0;
		public readonly int ArrayCount
		{
			get
			{
				int nCount = IsArray ? m_nDepth : 1;
				return IsCube ? nCount * 6 : nCount;
			}
		}
		public readonly int Depth => ((m_nFlags & RuntimeTextureSpecificationFlags.TSPEC_VOLUME_TEXTURE) != 0) ? m_nDepth : 1;


	}

}
