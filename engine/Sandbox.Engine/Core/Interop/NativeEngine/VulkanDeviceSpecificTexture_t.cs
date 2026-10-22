using System.Runtime.InteropServices;

namespace NativeEngine;

[StructLayout( LayoutKind.Sequential )]
internal struct VulkanDeviceSpecificTexture_t
{
	public ulong m_pImage;            // VkImage
	public uint m_nFormat;           // VkFormat
	public uint m_nWidth;
	public uint m_nHeight;
};
