namespace Editor.MapEditor;

public class PathToolSettings
{
	[Category( "Radius Offset" )]
	public bool OffsetByRadius { get; set; } = true;

	[Category( "Radius Offset" ), MinMax( 0, 100 )]
	public float Radius { get; set; } = 5.0f;

	public void Save()
	{
		EditorCookie.Set( "hammer.pathtool.useoffset", OffsetByRadius );
		EditorCookie.Set( "hammer.pathtool.offsetradius", Radius );
	}
	public void Load()
	{
		OffsetByRadius = EditorCookie.Get( "hammer.pathtool.useoffset", true );
		Radius = EditorCookie.Get( "hammer.pathtool.offsetradius", 5.0f );
	}
}

/// <summary>
/// Entity tool in Hammer, implements an interface called from native.
/// </summary>
partial class PathTool : IPathTool
{
	Widget Container { get; set; }
	EntitySelector EntitySelector { get; set; }

	Layout Properties { get; set; }
	PathToolSettings Settings { get; set; } = new();

	SerializedObject SerializedObject;

	public void CreateUI( Widget container )
	{
		// Keep a reference to our container so we can hotload from C#
		Container = container;

		Container.Layout = Layout.Column();
		Container.Layout.Margin = 0;
		Container.Layout.Spacing = 0;

		RefreshUI();
	}

	[Event( "tools.gamedata.refresh" )]
	[EditorEvent.Hotload]
	public void RefreshUI()
	{
		//
		// Careful touching any of this, if you do this shit wrong it will all crash
		//

		if ( SerializedObject is not null )
			SerializedObject.OnPropertyChanged -= OnPropertiesChanged;

		if ( !Container.IsValid() ) return;

		var oldSelected = GetCurrentEntityClassName();
		Settings.Load();

		// Clear old shite
		if ( EntitySelector.IsValid() && EntitySelector.IsValid ) EntitySelector.Destroy();
		if ( Properties.IsValid() && Properties.IsValid ) Properties.Destroy();

		Container.Layout.Clear( false );

		// Create new stuff
		EntitySelector = new( Container, true );
		EntitySelector.SelectedEntity = oldSelected;
		Container.Layout.Add( EntitySelector, 1 );

		Properties = Layout.Column();

		var sheet = new ControlSheet();
		SerializedObject = Settings.GetSerialized();
		SerializedObject.OnPropertyChanged += OnPropertiesChanged;
		sheet.AddObject( SerializedObject );

		Properties.Add( sheet );
		Properties.AddStretchCell();

		Container.Layout.Add( Properties );
	}

	private void OnPropertiesChanged( SerializedProperty property )
	{
		Settings.Save();
	}
}
