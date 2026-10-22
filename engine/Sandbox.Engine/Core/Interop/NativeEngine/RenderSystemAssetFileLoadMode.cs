
namespace NativeEngine
{
	internal enum RenderSystemAssetFileLoadMode : int
	{
		Immediate,                                     // asset is created and loaded from disk immediately
		Asynchronous,                                  // asset will start loading asynchronously
		Streamed,                                      // asset will be asynchronously loaded when referenced.
	}
}
