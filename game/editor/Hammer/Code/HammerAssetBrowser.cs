namespace Editor.MapEditor;

[Dock( "Hammer", "Asset Browser", "snippet_folder" )]
internal class HammerAssetBrowser : LocalAssetBrowser
{
	public static HammerAssetBrowser Instance { get; private set; }

	public HammerAssetBrowser( Widget parent ) : base( parent, null )
	{
		Instance = this;

		OnAssetHighlight += a => { if ( a.AssetType == AssetType.Material ) Hammer.SetCurrentMaterial( a ); };
		OnAssetSelected += a => { if ( a.AssetType == AssetType.Material ) Hammer.SetCurrentMaterial( a ); };
	}

	[Event( "asset.contextmenu", Priority = 150 )]
	protected static void OnAssetContextMenu_Hammer( AssetContextMenu e )
	{
		if ( e.AssetList?.Browser is not HammerAssetBrowser ) return;

		var count = e.SelectedList.Count;
		if ( count <= 0 ) return;

		e.Menu.AddSeparator();

		e.Menu.AddOption( "Select Objects Using Asset" + (count > 1 ? "s" : ""), "select_all", () =>
		{
			// GetHistory()->MarkUndoPosition( pActiveSession->GetSelection(), "Select Objects" );
			Selection.SelectMode = SelectMode.Objects;
			Selection.Clear();

			foreach ( var entry in e.SelectedList )
			{
				Hammer.SelectObjectsUsingAsset( entry.Asset );
			}
		} );

		if ( count == 1 )
		{
			var asset = e.SelectedList.First().Asset;

			e.Menu.AddOption( "List Map Objects Using Asset", "summarize", () =>
			{
				Hammer.ShowEntityReportForAsset( asset );
			} );

			e.Menu.AddOption( "Assign To Selection", "assignment", () =>
			{
				Hammer.AssignAssetToSelection( asset );
			} );

			if ( asset.AssetType == AssetType.Material )
			{
				e.Menu.AddOption( "Select Faces Using Material", "photo_size_select_large", () =>
				{
					Hammer.SelectFacesUsingMaterial( asset );
				} );
			}
		}
	}
}
