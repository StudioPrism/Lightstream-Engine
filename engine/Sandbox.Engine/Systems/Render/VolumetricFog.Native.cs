using System.Runtime.InteropServices;

namespace NativeEngine;

[StructLayout( LayoutKind.Sequential )]
internal struct SceneVolumetricFogParameters
{
	public bool m_bFogEnabled;
	public float m_flAnisotropy;
	public float m_flScattering;
	public float m_flDrawDistance;
	public float m_flFadeInStart;
	public float m_flFadeInEnd;
	public float m_flIndirectStrength;
}

[StructLayout( LayoutKind.Sequential )]
internal struct SceneVolumetricFogVolume
{
	public uint m_uID;
	public float m_fStrength;
	public float m_fExponent;
	public Vector3 m_vColor;
	public Vector3 m_vMin;
	public Vector3 m_vMax;
	public bool m_bSpherical;
	public Matrix m_matWorldToVolume;
}
