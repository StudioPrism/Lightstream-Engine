namespace Sandbox;

[Obsolete( "Use GpuBufferUsageFlags" )]
public enum ComputeBufferType
{
	/// <summary>
	/// Structured Buffer (HLSL RWStructuredBuffer)
	/// </summary>
	Structured,
	/// <summary>
	/// Byte Address Buffer (HLSL RWByteAddressBuffer)
	/// </summary>
	ByteAddress,
	/// <summary>
	/// Append Structured Buffer (HLSL AppendStructuredBuffer)
	/// </summary>
	Append,
	/// <summary>
	/// Indirect argument buffer for indirect draws
	/// <seealso cref="GpuBuffer.IndirectDrawArguments"/>
	/// </summary>
	IndirectDrawArguments
}

[Obsolete( "Use GpuBuffer" )]
public class ComputeBuffer<T> : GpuBuffer<T> where T : unmanaged
{
	public ComputeBuffer( int elementCount, ComputeBufferType type = ComputeBufferType.Structured )
		: base( elementCount, (GpuBuffer.UsageFlags)type )
	{

	}
}
