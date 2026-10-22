
namespace Sandbox;

internal static class SceneSystem
{
	internal static void OnBeforeRender( SceneObject sceneObject, in ManagedRenderSetup_t setup )
	{
		using ( new Graphics.Scope( in setup ) )
		{
			sceneObject.ExecuteBefore();
		}
	}

	internal static void OnAfterRender( SceneObject sceneObject, in ManagedRenderSetup_t setup )
	{
		using ( new Graphics.Scope( in setup ) )
		{
			sceneObject.ExecuteAfter();
		}
	}
}
