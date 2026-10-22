using System;

namespace Editor;

public partial class Asset
{
	/// <summary>
	/// Should target a static method like 
	/// `public static Pixmap RenderThumbnail( Asset thumbnail )`
	/// where the method returns a thumbnail for that asset type. 
	/// This kind of sucks I don't like it.
	/// </summary>
	[AttributeUsage( AttributeTargets.Method, AllowMultiple = false )]
	public class ThumbnailRendererAttribute : System.Attribute
	{
		/// <summary>
		/// The priority of this callback. Higher gets called first.
		/// </summary>
		public int Priority { get; set; }
	}
}
