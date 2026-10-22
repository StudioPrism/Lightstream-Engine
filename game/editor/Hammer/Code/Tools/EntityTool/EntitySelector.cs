namespace Editor.MapEditor;

public partial class EntitySelector : Widget
{
	string _selectedEntity = "";
	public string SelectedEntity { get => _selectedEntity; set { _selectedEntity = value; UpdateList(); } }

	LineEdit SearchFilter;
	EntityTreeView EntitiesTreeView;
	Button SearchFilterClear;
	bool IsPathTool;

	public EntitySelector( Widget parent, bool isPathTool = false ) : base( parent )
	{
		Layout = Layout.Column();
		Layout.Margin = 0;
		Layout.Spacing = 0;

		IsPathTool = isPathTool;
		Name = IsPathTool ? "pathtool" : "entitytool";

		// Filtering
		{
			var toolbar = new ToolBar( this );
			Layout.Add( toolbar );

			SearchFilter = new LineEdit( this );
			SearchFilter.PlaceholderText = "Search Entities";
			SearchFilter.TextEdited += ( x ) => UpdateList();
			toolbar.AddWidget( SearchFilter );

			SearchFilterClear = new Button( "", "close", SearchFilter );
			SearchFilterClear.Visible = false;
			SearchFilterClear.Clicked = () => { SearchFilter.Text = ""; UpdateList(); };
			SearchFilter.Layout = Layout.Row();
			SearchFilter.Layout.Add( SearchFilterClear );
			SearchFilter.Layout.AddStretchCell();

			var settings = toolbar.AddWidget( new Button( "", "settings", toolbar ) );
			settings.Clicked = () => OpenSettingsMenu( settings.ScreenRect );
		}

		// Container
		{
			Layout.AddSpacingCell( 4 );

			EntitiesTreeView = new EntityTreeView( this );
			EntitiesTreeView.OnItemSelected += OnEntitySelected;
			Layout.Add( EntitiesTreeView, 1 );
		}
	}

	void OnEntitySelected( string className )
	{
		_selectedEntity = className;

		var recent = EditorCookie.Get( $"hammer.{Name}.recent", new List<string>() );
		recent.Add( className );
		EditorCookie.Set( $"hammer.{Name}.recent", recent.Distinct().TakeLast( 20 ) );
	}

	void OpenSettingsMenu( Rect source )
	{
		var menu = new ContextMenu( this );
		menu.OpenAt( source.BottomLeft, false );
	}

	public IEnumerable<IGrouping<string, MapClass>> GetItems()
	{
		// Every entity we have
		var items = GameData.EntityClasses
			.Where( e => IsPathTool ? (e.IsPathClass || e.IsCableClass) : (e.IsPointClass || e.IsSolidClass) ) // bit wank, needs to be solved in fgdlib really or completely managed
			.Where( e => !e.Name.Equals( "worldspawn" ) ); // Special case

		//
		// When we do a search, do it across all entities
		//
		if ( !string.IsNullOrEmpty( SearchFilter.Text ) )
		{
			return items.Where( e => e.Name.Contains( SearchFilter.Text, System.StringComparison.OrdinalIgnoreCase ) ||
									 e.DisplayName.Contains( SearchFilter.Text, System.StringComparison.OrdinalIgnoreCase ) )
				.OrderBy( x => x.Name )
				.GroupBy( x => x.Category?.ToLower() ).OrderBy( p => p.Key == null )
				.ThenBy( p => p.Key );
		}

		// Order
		items = items.OrderBy( x => x.Name );

		// Group and order categories and have Uncategorized (null) last
		return items.GroupBy( x => x.Category?.ToLower() ).OrderBy( p => p.Key == null ).ThenBy( p => p.Key );
	}

	public void UpdateList()
	{
		SearchFilterClear.Visible = SearchFilter.Text.Length > 0;
		EntitiesTreeView.Clear();

		foreach ( var category in GetItems() )
		{
			var header = new TreeNode.Header( GetCategoryIcon( category.Key?.ToLower() ), category.Key ?? "Uncategorized" );

			header.IconColor = Theme.Text.Darken( 0.1f );
			EntitiesTreeView.AddItem( header );

			// Default to expanded
			EntitiesTreeView.Open( header );

			foreach ( var item in category )
			{
				var node = new EntityDataNode( item );
				node.PreferClassNames = true;
				node.ShowGameIcon = !string.IsNullOrEmpty( SearchFilter.Text );
				header.AddItem( node );

				// Restore selected entity
				if ( item.Name == SelectedEntity )
				{
					EntitiesTreeView.SelectItem( node );
				}
			}

			header.AddItem( new TreeNode.Spacer( 8 ) );
		}
	}

	/// <summary>
	/// Hard code some icons from category names, this ain't great but where else should I get these from
	/// </summary>
	static string GetCategoryIcon( string category ) => category switch
	{
		"lighting" => "lightbulb",
		"player" => "face",
		"effects" => "blur_on",
		"navigation" => "directions_run",
		"triggers" => "texture",
		"fog & sky" => "cloud",
		"constraints" => "anchor",
		"destruction" => "wine_bar",
		"recent" => "history",
		"sound" => "volume_up",
		"gameplay" => "sports_esports",
		_ => "invalid_icon",
	};
}
