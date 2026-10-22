namespace NativeEngine
{
	internal enum SceneSystemWellKnownTextureObjectID : int
	{
		// Simple 1x1 white texture RGBA8
		SCENE_TXTR_WHITE_1x1,
		// Simple 1x1 black texture RGBA8, with alpha=255
		SCENE_TXTR_BLACK_1x1,
		// Simple 1x1 transparent black texture RGBA8
		SCENE_TXTR_TRANSPARENT_BLACK_1x1,
		// Simple 1x1 white texture array RGBA8
		SCENE_TXTR_WHITE_1x1_ARRAY,
		// Simple 2x2 black/white checkerboard RGBA8
		SCENE_TXTR_CHECKER_2x2,
		// Simple 2x2x2 3D transparent black RGBA8
		SCENE_TXTR_BLACK_2x2x2,
		// Fixed-width 11x22 debug font - see debugfontdef.h
		//	SCENE_TXTR_DEBUG_FONT,
		// Light cookie sheet
		SCENE_TXTR_LIGHT_COOKIE_SHEET,
		// Simple 1x1 black cube map texture
		SCENE_TXTR_BLACK_CUBE_1x1,
		// Simple 1x1x1 black cube map texture array
		SCENE_TXTR_BLACK_CUBE_1x1_ARRAY,
		// Simple 1x1 transparent black texture array RGBA8
		SCENE_TXTR_TRANSPARENT_BLACK_1x1_ARRAY,
		// Simple 2x2x2 3D opaque black RGBA8
		SCENE_TXTR_OPAQUE_BLACK_2x2x2,
		// 1x1 single channel 16 bit unorm texture. Can be used as standin for depth texture that does comparison sampling in shader
		SCENE_TXTR_ONE_16_BIT_1x1,
		// 1x1x1 single channel 16 bit unorm texture array. Can be used as standin for depth texture that does comparison sampling in shader
		SCENE_TXTR_ONE_16_BIT_1x1_ARRAY,

		SCENE_TXTR_COUNT
	}
}
