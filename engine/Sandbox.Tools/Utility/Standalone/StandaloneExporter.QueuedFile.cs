using System.IO;

namespace Editor;

public partial class StandaloneExporter
{
	public enum QueuedFileState
	{
		Waiting,
		Copied,

		FileNotFound = 254,
		FailedToCopy = 255,
	}

	public class QueuedFile
	{
		public QueuedFileState State;

		public readonly BuildStep Step;
		public readonly int FileSize;
		public readonly string Source;
		public readonly string Destination;

		internal QueuedFile( string source, string destination, BuildStep step = BuildStep.Unknown )
		{
			Source = source.Replace( '\\', '/' );
			Destination = destination.Replace( '\\', '/' );

			State = QueuedFileState.Waiting;
			FileSize = File.Exists( source ) ? (int)new FileInfo( source ).Length : 0;

			Step = step;
		}
	}
}
