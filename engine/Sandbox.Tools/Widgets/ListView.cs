using Sandbox.UI;
using System;

namespace Editor;

public partial class ListView : BaseItemWidget
{
	/// <summary>
	/// Called when an item is scrolled into view.
	/// </summary>
	public Action<object> ItemScrollEnter { get; set; }

	/// <summary>
	/// Called when an item is scrolled out of view.
	/// </summary>
	public Action<object> ItemScrollExit { get; set; }

	Vector2 _itemSize;
	public Vector2 ItemSize { get => _itemSize; set { _itemSize = value; OnLayoutChanged(); } }

	Vector2 _itemSpacing;
	public Vector2 ItemSpacing { get => _itemSpacing; set { _itemSpacing = value; OnLayoutChanged(); } }

	Align _itemAlign;
	public Align ItemAlign { get => _itemAlign; set { _itemAlign = value; OnLayoutChanged(); } }

	int itemsPerRow { get; set; } = 1;
	int rowCount { get; set; }
	int rowHeight { get; set; }

	public ListView( Widget parent = null ) : base( parent )
	{
		ItemSize = 100;
		ItemSpacing = 2;
		ItemAlign = Align.Stretch;
		Margin = new Margin( 8, 8, 16, 8 );
	}

	protected override void OnLayoutChanged()
	{
		if ( _itemSize.y < 5 ) _itemSize.y = 5;
		if ( _itemSize.x < 5 && _itemSize.x > 0 ) _itemSize.x = 5;

		base.OnLayoutChanged();
	}

	protected override void OnPaint()
	{
		base.OnPaint();

		if ( DebugModeEnabled )
		{
			var firstRow = ItemLayouts.Count > 0 ? ItemLayouts.Min( x => x.Row ) : 0;

			var debugText = $"Items:	{Items.Count():n0}\nVisible:	{ItemLayouts.Count:n0}\nFirst Row: {firstRow:n0}\nPaint Time: {TimeMsPaint:0.00}ms\nLayout Time: {timeMsRebuild:0.00}ms";

			var mt = Paint.MeasureText( LocalRect.Shrink( 10 ), debugText, TextFlag.LeftTop );

			Paint.ClearPen();
			Paint.SetBrush( Color.Black.WithAlpha( 0.5f ) );
			Paint.DrawRect( mt.Grow( 5 ), 6 );

			Paint.SetPen( Color.White );
			Paint.DrawText( mt, debugText, TextFlag.LeftTop );
		}
	}

	public override bool SelectMoveRow( int positions )
	{
		if ( SelectedItems.Count() == 0 )
		{
			var obj = positions < 0 ? Items.LastOrDefault() : Items.FirstOrDefault();

			if ( obj != null )
			{
				SelectItem( obj );
				ScrollTo( obj );
				Update();
			}

			return obj != null;
		}

		return SelectMove( positions * itemsPerRow );
	}


	public override void ScrollTo( object target )
	{
		var idx = ItemIndex( target );
		if ( idx < 0 ) return;

		var row = idx / itemsPerRow;
		var vpos = row * rowHeight;

		ScrollTo( vpos, rowHeight );
	}
}
