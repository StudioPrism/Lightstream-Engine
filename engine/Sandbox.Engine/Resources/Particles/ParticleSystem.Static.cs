namespace Sandbox;

[Obsolete]
public sealed partial class ParticleSystem
{
	/// <summary>
	/// Loads a particle system from given file.
	/// </summary>
	[Obsolete]
	public static ParticleSystem Load( string filename ) => default;

	/// <summary>
	/// Load a particle system by file path.
	/// </summary>
	/// <param name="filename">The file path to load as a particle system.</param>
	/// <returns>The loaded particle system, or null</returns>
	[Obsolete]
	public static Task<ParticleSystem> LoadAsync( string filename ) => default;
}
