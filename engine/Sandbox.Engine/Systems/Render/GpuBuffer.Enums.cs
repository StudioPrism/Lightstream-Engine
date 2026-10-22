using System.Runtime.InteropServices;

namespace Sandbox;

public partial class GpuBuffer
{
	/// <summary>
	/// You can combine these e.g UsageFlags.Index | UsageFlags.ByteAddress for a buffer that can be used as an index buffer and in a compute shader.
	/// </summary>
	[Flags]
	public enum UsageFlags
	{
		/// <summary>
		/// Can be used as a vertex buffer.
		/// </summary>
		Vertex = 0x0001,
		/// <summary>
		/// Can be used as an index buffer.
		/// </summary>
		Index = 0x0002,
		/// <summary>
		/// Byte Address Buffer (HLSL RWByteAddressBuffer)
		/// </summary>
		ByteAddress = 0x0010,
		/// <summary>
		/// Structured Buffer (HLSL RWStructuredBuffer)
		/// </summary>
		Structured = 0x0020,
		/// <summary>
		/// Append Structured Buffer (HLSL AppendStructuredBuffer)
		/// </summary>
		Append = 0x0040,
		[Obsolete( "Structured and Append buffers automatically have counters" )]
		Counter = 0x0080,
		/// <summary>
		/// Indirect argument buffer for indirect draws
		/// <seealso cref="GpuBuffer.IndirectDrawArguments"/>
		/// <seealso cref="IndirectDrawIndexedArguments"/>
		/// </summary>
		IndirectDrawArguments = 0x0100
	}

	[StructLayout( LayoutKind.Sequential )]
	public struct IndirectDrawArguments
	{
		/// <summary>
		/// Number of vertices to draw per instance.
		/// </summary>
		public uint VertexCount;

		/// <summary>
		/// Number of instances to draw.
		/// </summary>
		public uint InstanceCount;

		/// <summary>
		/// Index of the first vertex to draw.
		/// </summary>
		public uint FirstVertex;

		/// <summary>
		/// Instance ID of the first instance.
		/// </summary>
		public uint FirstInstance;
	}

	[StructLayout( LayoutKind.Sequential )]
	public struct IndirectDrawIndexedArguments
	{
		/// <summary>
		/// Number of indices to draw per instance.
		/// </summary>
		public uint IndexCount;

		/// <summary>
		/// Number of instances to draw.
		/// </summary>
		public uint InstanceCount;

		/// <summary>
		/// Index of the first index to draw.
		/// </summary>
		public uint FirstIndex;

		/// <summary>
		/// Value added to each index before indexing into the vertex buffer.
		/// </summary>
		public int BaseVertex;

		/// <summary>
		/// Instance ID of the first instance.
		/// </summary>
		public uint FirstInstance;
	}

	[StructLayout( LayoutKind.Sequential )]
	public struct IndirectDispatchArguments
	{
		public uint ThreadGroupCountX { get; set; }
		public uint ThreadGroupCountY { get; set; }
		public uint ThreadGroupCountZ { get; set; }
	}
}
