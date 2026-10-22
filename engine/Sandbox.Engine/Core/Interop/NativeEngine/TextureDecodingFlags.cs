namespace NativeEngine
{
	[Flags]
	internal enum TextureDecodingFlags : int
	{
		TEXTURE_DECODE_FLAGS = 0x0000,
		TEXTURE_DECODE_COLOR_DILATION = 0x0001,
		TEXTURE_DECODE_CONVERT_TO_YCOCG_DXT5 = 0x0002,
	}
}
