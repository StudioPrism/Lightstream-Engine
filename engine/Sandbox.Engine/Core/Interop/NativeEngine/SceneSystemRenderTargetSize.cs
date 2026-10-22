namespace NativeEngine
{
	internal enum SceneSystemRenderTargetSize : int
	{
		SCENE_RTSIZE_128 = 0,   // currently unused
		SCENE_RTSIZE_256,   // light propagation volumes only
		SCENE_RTSIZE_512,   // also MONITOR_RTSIZE; technically used for VSM shadows but the code looks like it has rotted
		SCENE_RTSIZE_FRAMEBUFFER,   // vgui (SCENE_RTGT_SCRATCH_TEXTURE_8888), scenesystem (SCENE_RTGT_SCRATCH_DEPTHSTENCIL_TEXTURE) for read-only depth stencil fallback. scenesystem (SCENE_RTGT_SCRATCH_TEXTURE_8888) for refract FB copy
		SCENE_RTSIZE_2K,    // also BAKEDSHADOW_SRC_SIZE. Only used for experimental scenesystem light shadow baking.

		SCENE_RTSIZE_COUNT,
		SCENE_RTSIZE_INVALID
	}
}
