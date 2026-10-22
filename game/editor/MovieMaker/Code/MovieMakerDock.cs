namespace Editor.MovieMaker;

#nullable enable

[Dock( "Editor", "Movie Maker", "movie_creation" )]
public class MovieMakerDock : Widget
{
	private MovieEditor? _editor;

	public MovieMakerDock( Widget parent ) : base( parent )
	{
		Layout = Layout.Column();
		FocusMode = FocusMode.TabOrClickOrWheel;

		Build();
	}

	protected override bool OnClose()
	{
		if ( _editor?.Session is { HasUnsavedChanges: true } session )
		{
			this.ShowUnsavedChangesDialog(
				assetName: session.FileName,
				assetType: "movie",
				onSave: () => session.Save() );

			return false;
		}

		return base.OnClose();
	}

	[EditorEvent.Hotload]
	private void Build()
	{
		FromThemeAttribute.Apply();

		var sessions = _editor?.Sessions;

		_editor?.CloseSession();
		_editor?.Destroy();

		Layout.Clear( true );
		Layout.Add( _editor = new MovieEditor( this, sessions ) );
	}

	private int _titleHash;

	[EditorEvent.Frame]
	private void Frame()
	{
		UpdateTitle();
	}

	private void UpdateTitle()
	{
		var titleHash = HashCode.Combine( _editor?.Session?.HasUnsavedChanges );

		if ( _titleHash != titleHash )
		{
			_titleHash = titleHash;

			WindowTitle = _editor?.Session is { HasUnsavedChanges: true }
				? "Movie Maker*"
				: "Movie Maker";
		}
	}
}

