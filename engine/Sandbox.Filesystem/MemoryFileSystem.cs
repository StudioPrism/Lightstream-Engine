namespace Sandbox;

/// <summary>
/// A filesystem that only exists in memory
/// </summary>
internal class MemoryFileSystem : BaseFileSystem
{
	internal MemoryFileSystem() : base( new Zio.FileSystems.MemoryFileSystem() )
	{

	}
}
