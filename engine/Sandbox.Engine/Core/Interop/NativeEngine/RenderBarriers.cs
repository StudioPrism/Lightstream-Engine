
namespace NativeEngine;

[Flags]
internal enum RenderBarrierAccessFlags_t : int
{
	IndirectCommandReadBit = (1 << 0),
	IndexReadBit = (1 << 1),
	VertexAttributeReadBit = (1 << 2),
	UniformReadBit = (1 << 3),
	InputAttachmentReadBit = (1 << 4),
	ShaderReadBit = (1 << 5),
	ShaderWriteBit = (1 << 6),
	ColorAttachmentReadBit = (1 << 7),
	ColorAttachmentWriteBit = (1 << 8),
	DepthStencilAttachmentReadBit = (1 << 9),
	DepthStencilAttachmentWriteBit = (1 << 10),
	TransferReadBit = (1 << 11),
	TransferWriteBit = (1 << 12)
}

[Flags]
internal enum RenderBarrierPipelineStageFlags_t : int
{
	DrawIndirectBit = (1 << 0),
	VertexInputBit = (1 << 1),
	PreRasterizationShadersBit = (1 << 2),
	FragmentShaderBit = (1 << 3),
	EarlyFragmentTestsBit = (1 << 4),
	LateFragmentTestsBit = (1 << 5),
	ColorAttachmentOutputBit = (1 << 6),
	ComputeShaderBit = (1 << 7),
	TransferBit = (1 << 8),
	AllGraphicsBit = (1 << 9),
	RayTracingShaderBit = (1 << 10),
	TopOfPipeBit = (1 << 11),
	BottomOfPipeBit = (1 << 12)
}
