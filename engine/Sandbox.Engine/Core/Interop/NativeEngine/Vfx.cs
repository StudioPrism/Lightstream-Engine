
namespace NativeEngine;

public enum VfxCompileTarget_t
{
	COMPILE_TARGET_UNSUPPORTED = -1,
	SM_5_0 = 0,
	SM_6_0,
	SM_6_0_VULKAN,
	COMPILE_TARGET_MAX
};

enum VfxCompileShaderFlags_t
{
	VFX_COMPILE_SHADER_FLAGS_DISABLE_OPTIMIZATION = 0x1,    // disable microcode/shader optimizations
	VFX_COMPILE_SHADER_FLAGS_ENABLE_DEBUGGING = 0x2,    // enable shader debugging: writes UPDB's on 360, generated shader source code to disk/load from disk on PC
	VFX_COMPILE_SHADER_PREFER_FLOW_CONTROL = 0x4,   // prefer flow control
	VFX_COMPILE_SHADER_SKIP_COMPILE_OUTPUT_SOURCE = 0x8,    // Don't compile the shader, just output the preprocessed shader source
};
