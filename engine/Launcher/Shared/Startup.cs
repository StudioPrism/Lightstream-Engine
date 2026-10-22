using System;

namespace Sandbox;

public static class Program
{
	/// <summary>
	/// Because the dlls aren't next to the exe, we need to do a dance to init all the paths before
	/// any of the types are touched. To do this we init in the main, here, and then call Launch which
	/// will call your applcation defined code.
	/// </summary>
	[STAThread]
	public static int Main()
	{
		LauncherEnvironment.Init();
		return Launch();
	}

	static int Launch()
	{
		return Launcher.Main();
	}
}
