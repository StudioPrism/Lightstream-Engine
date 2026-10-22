namespace NativeEngine;

[Flags]
internal enum RuntimeTextureSpecificationFlags : uint
{
	TSPEC_FLAGS = 0x0000,
	TSPEC_RENDER_TARGET = 0x0001,
	TSPEC_VERTEX_TEXTURE = 0x0002,
	TSPEC_UNFILTERABLE_OK = 0x0004,
	TSPEC_RENDER_TARGET_SAMPLEABLE = 0x0008,
	TSPEC_NO_LOD = 0x0010,  // Don't downsample on lower-level cards
	TSPEC_CUBE_TEXTURE = 0x0020,
	TSPEC_VOLUME_TEXTURE = 0x0040,
	TSPEC_TEXTURE_ARRAY = 0x0080,
	TSPEC_TEXTURE_GEN_MIP_MAPS = 0x0100,    // Must be used with TSPEC_RENDER_TARGET for it to have any effect on D3D11
	TSPEC_SHARED_RESOURCE = 0x0200, // For sharing a texture handle between different d3d devices (only works for 2D non-mipped textures)
	TSPEC_UAV = 0x0400, // Support binding the texture as a Unordered Access View in a compute or pixel shader
	TSPEC_INPUT_ATTACHMENT = 0x0800,    // Support binding the texture as a SubpassInput in the pixel shader
	TSPEC_CUBE_CAN_SAMPLE_AS_ARRAY = 0x1000,    // Allows cube map to be treated as a 2D Array inside compute shaders
	TSPEC_LINEAR_COLOR_SPACE = 0x2000,  // For GL ES 3.0: Create the API texture to be fetched with linear color space instead of the default sRGB
}
