namespace Sandbox.Rendering;

/// <summary>
/// Gives global access to the texture streaming system.
/// </summary>
public static class TextureStreaming
{
	static bool disabledStreaming;

	/// <summary>
	/// Run a block of code with texture streaming disabled
	/// </summary>
	public static void ExecuteWithDisabled( Action action )
	{
		var prev = disabledStreaming;
		disabledStreaming = true;
		g_pRenderDevice.SetForcePreloadStreamingData( true );
		try
		{
			action();
		}
		finally
		{
			disabledStreaming = prev;
			g_pRenderDevice.SetForcePreloadStreamingData( prev );
		}
	}
}
