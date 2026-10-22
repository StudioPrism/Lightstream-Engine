namespace Editor;

public partial class StandaloneExporter
{
	public struct ExportProgress
	{
		public float ProgressFraction { get; set; }
		public int FilesDone { get; set; }
		public int FilesTotal { get; set; }
		public string[] BuildIssues { get; set; }
		public string CurrentOperation { get; set; }
	}

	private void UpdateProgress( ExportProgress progress )
	{
		MainThread.Queue( () => OnProgressChanged( progress ) );
	}
}
