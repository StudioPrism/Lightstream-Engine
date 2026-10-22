
namespace Sandbox.Rendering;

/// <summary>
/// Represents filtering modes for texture sampling in the rendering pipeline.
/// </summary>
[Expose]
public enum FilterMode : int
{
	/// <summary>
	/// Uses the nearest texel without interpolation.
	/// Fastest but lowest visual quality.
	/// </summary>
	Point = 0x0,

	/// <summary>
	/// Interpolates between the four nearest texels in the same mip level.
	/// Smoother than point sampling but does not blend between mip levels.
	/// </summary>
	Bilinear = 0x14,

	/// <summary>
	/// Bilinear sampling with smooth transitions between mipmap levels.
	/// Provides better visual quality across distances.
	/// </summary>
	Trilinear = 0x15,

	/// <summary>
	/// Enhances texture detail on steep viewing angles.
	/// Best visual quality, higher performance cost.
	/// </summary>
	Anisotropic = 0x55,
}
