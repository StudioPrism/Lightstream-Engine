namespace NativeEngine
{
	internal enum TextureUsage : int
	{
		TEXTURE_USAGE_INVALID = -1,

		TEXTURE_USAGE_DEFAULT = 0,

		// GPU can read from it only, CPU can only write once
		// Scope must be GLOBAL.
		TEXTURE_USAGE_STATIC,

		// GPU can read, writes are infrequent from CPU
		// Scope must be GLOBAL.
		TEXTURE_USAGE_SEMISTATIC,

		// GPU can read, writes are frequent from CPU
		// Scope can be global, per-frame, or per-context.
		TEXTURE_USAGE_DYNAMIC,

		// GPU can read/write, CPU will never read
		// Currently, scope must be global.
		TEXTURE_USAGE_GPU_ONLY,

		TEXTURE_USAGE_COUNT
	}
}
