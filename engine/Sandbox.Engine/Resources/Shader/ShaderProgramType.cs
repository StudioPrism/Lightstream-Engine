namespace Sandbox;

internal enum ShaderProgramType
{
	VFX_PROGRAM_FEATURE = 0, // Keep this first

	VFX_PROGRAM_VS,
	VFX_PROGRAM_PS,
	VFX_PROGRAM_GS,
	VFX_PROGRAM_HS, // Does nothing!
	VFX_PROGRAM_DS, // Does nothing!
	VFX_PROGRAM_CS,
	VFX_PROGRAM_PS_RENDER_STATE,
	VFX_PROGRAM_RTX,

	VFX_PROGRAM_MAX, // Used for looping over all real programs

	VFX_DUMMY_PROGRAM_MODES,
	VFX_DUMMY_PROGRAM_VERSION,
	VFX_DUMMY_PROGRAM_DEV_SHADER,
	VFX_DUMMY_PROGRAM_DESCRIPTION,
	VFX_DUMMY_PROGRAM_DEBUG_INFO,

	VFX_DUMMY_PROGRAM_MAX, // Used for looping over all dummy programs
};
