namespace Sandbox.VR;

public partial class VROverlay
{
	static internal void RenderAll()
	{
		Graphics.AssertRenderBlock();

		if ( !VRSystem.IsActive ) return;

		for ( int i = All.Count - 1; i >= 0; i-- )
		{
			if ( !All[i].TryGetTarget( out var overlay ) ) continue;

			overlay.Render();
		}
	}

	static internal void UpdateAll()
	{
		if ( !VRSystem.IsActive ) return;

		for ( int i = All.Count - 1; i >= 0; i-- )
		{
			// Remove any dead references or dead handles
			if ( !All[i].TryGetTarget( out var overlay ) || overlay.handle == 0 )
			{
				All.RemoveAt( i );
				continue;
			}

			overlay.Update();
			overlay.PollEvents();
		}
	}

	internal void Update()
	{

	}

	internal UI.InputData InputData;

	internal void PollEvents()
	{

	}

	internal virtual void Render()
	{

	}

	internal void UpdateTexture()
	{
		if ( !Visible || Texture == null || !Texture.native.IsValid ) return;
	}
}
