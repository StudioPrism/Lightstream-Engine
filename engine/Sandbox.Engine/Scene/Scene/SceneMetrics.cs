namespace Sandbox;

internal static class SceneMetrics
{
	public static void Flip()
	{
		ParticlesCreated = 0;
		ParticlesDestroyed = 0;
		GameObjectsCreated = 0;
		GameObjectsDestroyed = 0;
		ComponentsCreated = 0;
		ComponentsDestroyed = 0;
		RayTrace = 0;
		RayTraceAll = 0;
	}

	public static double ParticlesCreated = 0;
	public static double ParticlesDestroyed = 0;
	public static double GameObjectsCreated = 0;
	public static double GameObjectsDestroyed = 0;
	public static double ComponentsCreated = 0;
	public static double ComponentsDestroyed = 0;
	public static double RayTrace = 0;
	public static double RayTraceAll = 0;
}
