namespace Editor.MapEditor;

partial class EntityTool : IEntityTool
{
	Widget Container { get; set; }
	EntitySelector EntitySelector { get; set; }

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

		if ( !Container.IsValid() ) return;

		var oldSelected = GetCurrentEntityClassName();

		if ( EntitySelector.IsValid() && EntitySelector.IsValid ) EntitySelector.Destroy();

		Container.Layout.Clear( true );

		EntitySelector = new EntitySelector( Container );
		EntitySelector.SelectedEntity = oldSelected;
		Container.Layout.Add( EntitySelector, 1 );
	}
}
