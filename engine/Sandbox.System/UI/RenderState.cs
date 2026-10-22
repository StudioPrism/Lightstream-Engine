
namespace Sandbox.UI;

/// <summary>
/// Describes panel's position and size for rendering operations.
/// </summary>
public readonly struct RenderState
{
	/// <summary>
	/// Position of the panel on the X axis. This can be a negative value!
	/// </summary>
	public float X { readonly get; init; }

	/// <summary>
	/// Position of the panel on the Y axis. This can be a negative value!
	/// </summary>
	public float Y { readonly get; init; }

	/// <summary>
	/// Width of the panel.
	/// </summary>
	public float Width { readonly get; init; }

	/// <summary>
	/// Height of the panel.
	/// </summary>
	public float Height { readonly get; init; }

	/// <summary>
	/// Render Opacity Overrides
	/// </summary>
	internal float RenderOpacity { readonly get; init; }

	/// <summary>
	/// Allows easy cast to a <see cref="Rect"/> for usage in rendering functions.
	/// </summary>
	public static implicit operator Rect( RenderState rs ) => new( rs.X, rs.Y, rs.Width, rs.Height );
}
