namespace Sandbox.Rendering;

/// <summary>
/// Specifies how texture coordinates outside the [0.0, 1.0] range are handled.
/// </summary>
[Expose]
public enum TextureAddressMode : int
{
	/// <summary>
	/// Wraps the texture coordinates. Values beyond 1.0 wrap around to the beginning.
	/// Produces a repeating tiling pattern.
	/// </summary>
	Wrap = 0,

	/// <summary>
	/// Mirrors the texture coordinates when sampling outside the [0.0, 1.0] range.
	/// Creates a mirrored tiling effect.
	/// </summary>
	Mirror = 1,

	/// <summary>
	/// Clamps the texture coordinates to the [0.0, 1.0] range.
	/// Texture edges are stretched when sampling outside the range.
	/// </summary>
	Clamp = 2,

	/// <summary>
	/// Uses a constant border color when sampling outside the [0.0, 1.0] range.
	/// Requires a border color to be defined.
	/// </summary>
	Border = 3,

	/// <summary>
	/// Mirrors the texture once, then clamps to the edge.
	/// Coordinates in [0.0, 1.0] sample normally, [-1.0, 0.0] mirror once, and everything else clamps.
	/// </summary>
	MirrorOnce = 4
}
