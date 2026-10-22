using Sandbox.Rendering;

namespace Sandbox;

public abstract partial class Component
{
	/// <summary>
	/// Allows scene systems to react to render threads
	/// </summary>
	internal interface IRenderThread
	{
		void OnRenderStage( CameraComponent camera, Stage stage );
	}
}
