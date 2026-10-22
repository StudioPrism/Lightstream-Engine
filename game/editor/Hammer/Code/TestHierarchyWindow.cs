using Editor.MapDoc;

namespace Editor.MapEditor;

class TestHierarchyWindow : Window
{
	[Menu( "Hammer", "Sbox/Test Hierarchy", "info" )]
	public static void OpenWindow2()
	{
		new TestHierarchyWindow();
	}

	public TestHierarchyWindow()
	{
		Title = "Test Hammer Window";
		Size = new Vector2( 400, 700 );

		CreateUI();
		Show();
	}

	public void CreateUI()
	{
		var map = Hammer.ActiveMap;
		if ( !map.IsValid() ) return;

		Title = map.PathName;

		var canvas = new Widget( null );
		canvas.Layout = Layout.Column();
		canvas.Layout.Spacing = 8;
		canvas.Layout.Margin = 8;

		var note = new InformationBox( "Testing MapNode hierarchy in C#", canvas );

		var tree = new TreeView( canvas );

		var worldNode = tree.AddItem( new TreeNode.Section( "public", "MapWorld", true ) );

		foreach ( var mapNode in map.World.Children )
		{
			worldNode.AddItem( new TreeMapNode( mapNode ) );
		}

		tree.Open( worldNode );

		canvas.Layout.Add( note, 0 );
		canvas.Layout.Add( tree, 1 );

		Canvas = canvas;
	}

	class TreeMapNode : TreeNode<MapNode>
	{
		public TreeMapNode( MapNode obj ) : base( obj )
		{

		}

		public override void OnPaint( VirtualWidget item )
		{
			PaintSelection( item );

			var rect = item.Rect;

			var color = Theme.Green;

			Paint.Antialiasing = true;

			var iconRect = rect;
			iconRect.Width = iconRect.Height;

			var displayInfo = DisplayInfo.For( Value );

			var r = rect;
			Paint.SetPen( color.Lighten( 0.2f ) );
			Paint.DrawIcon( iconRect, displayInfo.Icon ?? "help_outline", 11, TextFlag.Center );
			r.Left = iconRect.Right + 5;

			Paint.SetDefaultFont( 8 );
			Paint.SetPen( color );
			Paint.DrawText( r, Value.ToString(), TextFlag.LeftCenter );
		}
	}
}
