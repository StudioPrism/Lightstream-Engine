namespace Sandbox;

partial class Compiler
{
	private List<FileWatch> sourceWatchers = new();

	/// <summary>
	/// Watch the filesystem for changes to our c# files, and trigger a recompile if they change.
	/// </summary>
	public void WatchForChanges()
	{
		foreach ( var fs in SourceLocations )
		{
			var watcher = fs.Watch( "*.*" );
			watcher.OnChangedFile += OnFileChanged;
			sourceWatchers.Add( watcher );
		}
	}

	private void OnFileChanged( string file )
	{
		if ( !file.EndsWith( ".cs", StringComparison.OrdinalIgnoreCase ) && !file.EndsWith( ".razor", StringComparison.OrdinalIgnoreCase ) )
			return;

		if ( file.Contains( "/obj/", StringComparison.OrdinalIgnoreCase ) )
			return;

		MarkForRecompile();
	}
}
