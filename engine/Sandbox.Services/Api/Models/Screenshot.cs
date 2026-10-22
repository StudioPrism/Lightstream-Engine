namespace Sandbox.Services;

public class Screenshot
{
	public DateTime Created { get; set; }
	public int Width { get; set; }
	public int Height { get; set; }
	public string Url { get; set; }
	public string Thumb { get; set; }
	public bool IsVideo { get; set; }

	public string GetThumb( float width )
	{
		var height = width / ((float)Width / (float)Height);
		return Thumb.Replace( "{width}", width.ToString() ).Replace( "{height}", height.ToString() );
	}
}
