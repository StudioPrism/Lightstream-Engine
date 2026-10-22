namespace Sandbox;

/// <summary>
/// A filesystem that redirects a package's local paths to the actual files in a download cache
/// </summary>
internal class PackageFileSystem : BaseFileSystem
{
	internal Sandbox.Internal.RedirectFileSystem Redirect => system as Sandbox.Internal.RedirectFileSystem;

	internal PackageFileSystem()
	{
		// Files are all lowercase in manifests, so ignore case in comparer
		system = AssetDownloadCache.CreateRedirectFileSystem();
	}
}
