namespace Sandbox;

internal sealed partial class ToneMapping
{
	ITonemapSystem native;

	internal ToneMapping()
	{
		native = g_pSceneUtils.CreateTonemapSystem();
	}

	~ToneMapping()
	{
		g_pSceneUtils.DestroyTonemapSystem( native );
	}

	internal ITonemapSystem GetNative() => native;
}
