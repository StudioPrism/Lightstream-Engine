namespace Sandbox;

public class VolumetricFogParameters
{
	/// <summary>
	/// Indicates whether the fog system is enabled.
	/// </summary>
	public bool Enabled { get; set; }

	/// <summary>
	/// Level of anisotropy.
	/// </summary>
	public float Anisotropy { get; set; }

	/// <summary>
	/// Scattering value.
	/// </summary>
	public float Scattering { get; set; }

	/// <summary>
	/// Draw distance.
	/// </summary>
	public float DrawDistance { get; set; }

	/// <summary>
	/// Start distance where fading begins.
	/// </summary>
	public float FadeInStart { get; set; }

	/// <summary>
	/// End distance where fading concludes.
	/// </summary>
	public float FadeInEnd { get; set; }

	/// <summary>
	/// Strength of indirect illumination.
	/// </summary>
	public float IndirectStrength { get; set; }

	/// <summary>
	/// Provides indirect lighting from a baked volume texture.
	/// This gets compiled with your map and is provided by an env_volumetric_controller.
	/// </summary>
	/// <remarks>
	/// You shouldn't expect to be able to add new runtime fog volumes if using this.
	/// </remarks>
	public Texture BakedIndirectTexture { get; set; }
}

internal class VolumetricFog : IDisposable
{
	internal IVolumetricFog native;

	internal VolumetricFog()
	{
		native = g_pSceneUtils.CreateVolumetricFog();
	}

	internal void Update( VolumetricFogParameters p )
	{
		NativeEngine.SceneVolumetricFogParameters parameters = new()
		{
			m_bFogEnabled = p.Enabled,
			m_flAnisotropy = p.Anisotropy,
			m_flScattering = p.Scattering,
			m_flDrawDistance = p.DrawDistance,
			m_flFadeInStart = p.FadeInStart,
			m_flFadeInEnd = p.FadeInEnd,
			m_flIndirectStrength = p.IndirectStrength,
		};

		native.SetParams( parameters, (p.BakedIndirectTexture != null) ? p.BakedIndirectTexture.native : IntPtr.Zero );
	}

	~VolumetricFog()
	{
		Dispose( false );
	}

	bool disposedValue;
	protected virtual void Dispose( bool disposing )
	{
		if ( !disposedValue )
		{
			g_pSceneUtils.DestroyVolumetricFog( native );
			native = default;
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose( true );
		GC.SuppressFinalize( this );
	}
}
