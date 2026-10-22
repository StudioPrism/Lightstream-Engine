using System;

namespace NativeEngine;

struct BufferDesc
{
	public int m_nElementCount;        // Number of vertices/indices
	public int m_nElementSizeInBytes;  // Size of a single vertex/index
	public nint m_pDebugName;           // Used to debug buffers
	public nint m_pBudgetGroupName;
}

enum RenderBufferType
{
	RENDER_BUFFER_TYPE_STATIC = 0,          // GPU can read from it only, CPU can only write once
	RENDER_BUFFER_TYPE_SEMISTATIC,          // GPU can read, writes are infrequent from CPU
	RENDER_BUFFER_TYPE_STAGING,             // GPU can write, CPU can read
	RENDER_BUFFER_TYPE_GPU_ONLY,            // GPU can read/write, CPU cannot read/write (used for GPU-generated data)
}

[Flags]
enum RenderBufferFlags
{
	RENDER_BUFFER_USAGE_VERTEX_BUFFER = 0x0001,
	RENDER_BUFFER_USAGE_INDEX_BUFFER = 0x0002,
	RENDER_BUFFER_USAGE_SHADER_RESOURCE = 0x0004,
	RENDER_BUFFER_USAGE_UNORDERED_ACCESS = 0x0008,
	RENDER_BUFFER_BYTEADDRESS_BUFFER = 0x0010,
	RENDER_BUFFER_STRUCTURED_BUFFER = 0x0020,
	RENDER_BUFFER_APPEND_CONSUME_BUFFER = 0x0040,
	RENDER_BUFFER_UAV_COUNTER = 0x0080,
	RENDER_BUFFER_UAV_DRAW_INDIRECT_ARGS = 0x0100,
}
