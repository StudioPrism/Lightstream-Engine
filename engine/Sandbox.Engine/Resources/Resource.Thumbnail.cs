namespace Sandbox;

public partial class Resource
{
	/// <summary>
	/// Render a thumbnail for this specific resource.
	/// </summary>
	public virtual Bitmap RenderThumbnail( ThumbnailOptions options )
	{
		// default, render nothing
		return default;
	}

	public record struct ThumbnailOptions
	{
		public int Width { get; set; }
		public int Height { get; set; }
	}
}
