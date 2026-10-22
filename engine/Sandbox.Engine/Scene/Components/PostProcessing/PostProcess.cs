using Sandbox.Rendering;

namespace Sandbox;

/// <summary>
/// Adds an effect to the camera
/// </summary>
[Obsolete( "Switch to use BasePostProcess<>" )]
public abstract class PostProcess : Component, Component.DontExecuteOnServer
{
	[RequireComponent]
	public CameraComponent Camera { get; set; }

	/// <summary>
	/// The stage in the render pipeline that we'll get rendered in
	/// </summary>
	protected virtual Stage RenderStage => Stage.AfterPostProcess;

	/// <summary>
	/// Lower numbers get renderered first
	/// </summary>
	protected virtual int RenderOrder => 200;

	protected CommandList CommandList { get; private set; }

	protected override void OnEnabled()
	{
		CommandList = new CommandList( GetType().Name );
		Camera.AddCommandList( CommandList, RenderStage, RenderOrder );
	}

	protected override void OnDisabled()
	{
		Camera.RemoveCommandList( CommandList );
		CommandList = null;
	}

	protected override void OnUpdate()
	{
		if ( CommandList is null )
			return;

		CommandList.Reset();
		UpdateCommandList();
	}

	/// <summary>
	/// You should implement this method and fill the CommandList with the actions
	/// that you want to do for this post process.
	/// </summary>
	protected virtual void UpdateCommandList()
	{

	}
}
