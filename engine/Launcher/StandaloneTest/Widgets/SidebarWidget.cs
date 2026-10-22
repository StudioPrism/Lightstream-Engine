namespace Editor;


class SidebarWidget : Widget
{
	public SidebarWidget( Widget parent ) : base( parent )
	{
		Layout = Layout.Column();
		Layout.Margin = 16;
		Layout.Spacing = 4;

		FixedWidth = 200;
	}

	public T Add<T>( T widget ) where T : Widget
	{
		return Layout.Add( widget );
	}

	public void AddSpacer()
	{
		Layout.AddSpacingCell( 8 );
	}

	public void AddStretchCell()
	{
		Layout.AddStretchCell();
	}

	public void AddSeparator()
	{
		AddSpacer();
		Layout.AddSeparator( true );
		AddSpacer();
	}

	protected override Vector2 SizeHint()
	{
		return new Vector2( 64, 64 );
	}

	protected override void OnPaint()
	{
		Paint.ClearPen();
		Paint.ClearBrush();
		Paint.SetDefaultFont();

		var r = LocalRect;
		Paint.SetBrush( Theme.SidebarBackground );
		Paint.DrawRect( r );
	}
}
