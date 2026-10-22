using System;
using System.Runtime.InteropServices;

[StructLayout( LayoutKind.Sequential )]
internal struct GetLightmapInfoForPoint_t
{
	public Vector2 LightmapUv;
	public IntPtr BakedLightingInfo;
};
