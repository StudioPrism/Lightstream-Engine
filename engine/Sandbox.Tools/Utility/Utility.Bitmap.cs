using Sandbox.Utility;
namespace Editor;

public static partial class EditorUtility
{
	/// <summary>
	/// Load a float bitmap. This is usually a png, tga, exr, psd
	/// </summary>
	public static FloatBitmap LoadBitmap( string filename )
	{
		var bm = FloatBitMap_t.Create();

		if ( !bm.LoadFromFile( filename, FBMGammaType_t.FBM_GAMMA_LINEAR ) )
		{
			bm.Delete();
			return default;
		}

		return new FloatBitmap( bm );
	}
}
