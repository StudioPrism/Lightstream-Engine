namespace Sandbox;

public abstract partial class Component
{
	/// <summary>
	/// Called on update start. This is the very first thing called.
	/// </summary>
	public interface ISceneStage
	{
		void Start() { }
		void End() { }
	}
}
