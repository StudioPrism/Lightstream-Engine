using Sandbox.Engine.Utility.RayTrace;

namespace Sandbox;


public partial class SceneWorld
{
	/// <summary>
	/// Trace against all scene objects in this scene world
	/// </summary>
	public MeshTraceRequest Trace => new() { targetWorld = this };
}
