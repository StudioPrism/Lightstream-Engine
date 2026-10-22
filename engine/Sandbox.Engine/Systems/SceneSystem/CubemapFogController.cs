
namespace Sandbox;

public class CubemapFogController
{
	/// <summary>
	/// Adjust how quickly the cubemap blurs out at closer distances. A value of 0.0 always uses the lowest resolution MIP over the entire range, while a value of 1.0 uses the highest.
	/// </summary>
	public float LodBias { get; set; }

	/// <summary>
	/// The distance from the player at which the fog will start to fade in.
	/// </summary>
	public float StartDistance { get; set; }

	/// <summary>
	/// The distance from the player at which the fog will be at full strength.
	/// </summary>
	public float EndDistance { get; set; }

	/// <summary>
	/// Exponent for distance falloff. For example, 2.0 is proportional to square of distance.
	/// </summary>
	public float FalloffExponent { get; set; }

	/// <summary>
	/// The distance between the start of the height fog and where it is fully opaque. Setting this to 0 will disable height based blending.
	/// </summary>
	public float HeightWidth { get; set; }

	/// <summary>
	/// The absolute height in the map at which the height fog will start to fade in.
	/// </summary>
	public float HeightStart { get; set; }

	/// <summary>
	/// Exponent for height falloff. For example, 2.0 is proportional to square of distance.
	/// </summary>
	public float HeightExponent { get; set; }

	/// <summary>
	/// Is this cubemap fog active?
	/// </summary>
	public bool Enabled { get; set; }

	/// <summary>
	/// Cubemap texture to use for the fog.
	/// </summary>
	public Texture Texture { get; set; }

	/// <summary>
	/// Location of the fog.
	/// </summary>
	public Transform Transform { get; set; }

	/// <summary>
	/// Tint of the fog. 
	/// </summary>
	public Color Tint { get; set; }

	internal void Write( RenderAttributes attr )
	{
		attr.Set( "CubeFogParams", new Vector4( StartDistance, EndDistance, LodBias, FalloffExponent ) );
		attr.Set( "CubeFogParams2", new Vector4( HeightWidth, HeightStart, HeightExponent, 0.0f ) );
		attr.Set( "CubeFogTransform", Matrix.FromTransform( Transform ) );
		attr.Set( "CubemapFogColor", Tint );

		if ( Enabled )
		{
			attr.Set( "CubemapFogTexture", Texture );
			attr.SetCombo( "D_ENABLE_CUBEMAP_FOG", 1 );
		}
		else
		{
			attr.SetCombo( "D_ENABLE_CUBEMAP_FOG", 0 );
		}
	}
}
