using static Editor.StandaloneExporter;

namespace Editor;

public partial class StandaloneExporter
{
	public enum BuildStep
	{
		Unknown,
		CopyDLLs,
		CopyCoreAssets,
		CopyBaseAssets,
		CopyCloudAssets,
		CopyProjectAssets,
		CopyCode,
		CopyMisc,
		FinalizeExecutable
	}
}

public static class BuildStepExtensions
{
	public static string GetDescription( this BuildStep step ) => step switch
	{
		BuildStep.CopyDLLs => "Copying core s&box DLL files",
		BuildStep.CopyCode => "Copying core s&box code files",
		BuildStep.CopyCoreAssets => "Copying core s&box assets",
		BuildStep.CopyBaseAssets => "Copying base s&box assets",
		BuildStep.CopyCloudAssets => "Copying project cloud assets",
		BuildStep.CopyProjectAssets => "Copying project local assets",
		BuildStep.FinalizeExecutable => "Finalizing standalone executable",
		_ => "Copying miscallaneous files",
	};
}
