using Native;
using Sandbox;
using System;

namespace Editor;

/// <summary>
/// A widget layout. You can think of it as an invisible box of rows or columns, each one containing a widget, useful for automatic positioning and scaling.
/// </summary>
public class GridLayout : Layout
{
	internal Native.QGridLayout _gridlayout;

	public GridLayout() : base( Native.QGridLayout.Create( default ) )
	{
		Assert.True( _gridlayout.IsValid );
	}

	public sealed override T Add<T>( T widget )
	{
		if ( !widget.IsValid() ) throw new ArgumentNullException( nameof( widget ) );
		AddCell( 0, 0, widget );
		return widget;
	}

	public T AddCell<T>( int x, int y, T widget, int xSpan = 1, int ySpan = 1, TextFlag alignment = 0 ) where T : Widget
	{
		if ( !widget.IsValid() ) throw new ArgumentNullException( nameof( widget ) );
		_gridlayout.addWidget( widget._widget, y, x, ySpan, xSpan, (int)alignment );
		return widget;
	}

	public sealed override Layout Add( Layout layout )
	{
		if ( !layout.IsValid() ) throw new ArgumentNullException( nameof( layout ) );
		AddCell( 0, 0, layout );

		return layout;
	}

	public sealed override Layout Add( Layout layout, int stretch )
	{
		if ( !layout.IsValid() ) throw new ArgumentNullException( nameof( layout ) );
		AddCell( 0, 0, layout );
		return layout;
	}

	public Layout AddCell( int x, int y, Layout layout, int xSpan = 1, int ySpan = 1, TextFlag alignment = 0 )
	{
		if ( !layout.IsValid() ) throw new ArgumentNullException( nameof( layout ) );
		_gridlayout.addLayout( layout._layout, y, x, ySpan, xSpan, (int)alignment );
		return layout;
	}

	public void SetRowStretch( params float[] values )
	{
		if ( !_gridlayout.IsValid ) return;

		for ( int i = 0; i < values.Length; i++ )
			_gridlayout.setRowStretch( i, (int)values[i] );
	}

	public void SetColumnStretch( params float[] values )
	{
		if ( !_gridlayout.IsValid ) return;

		for ( int i = 0; i < values.Length; i++ )
			_gridlayout.setColumnStretch( i, (int)values[i] );
	}

	public float HorizontalSpacing
	{
		get => _gridlayout.IsValid ? _gridlayout.horizontalSpacing() : Spacing;
		set
		{
			if ( _gridlayout.IsValid )
			{
				_gridlayout.setHorizontalSpacing( (int)value );
				return;
			}

			Spacing = value;
		}
	}

	public float VerticalSpacing
	{
		get => _gridlayout.IsValid ? (float)_gridlayout.verticalSpacing() : Spacing;
		set
		{
			if ( _gridlayout.IsValid )
			{
				_gridlayout.setVerticalSpacing( (int)value );
				return;
			}

			Spacing = value;
		}

	}

	public void SetMinimumRowHeight( int row, int height )
	{
		if ( !_gridlayout.IsValid ) return;
		_gridlayout.setRowMinimumHeight( row, height );
	}

	public void SetMinimumColumnWidth( int column, int width )
	{
		if ( !_gridlayout.IsValid ) return;
		_gridlayout.setColumnMinimumWidth( column, width );
	}

	public Rect GetCellRect( int x, int y )
	{
		if ( !_gridlayout.IsValid ) return InnerRect;
		return _gridlayout.cellRect( x, y ).Rect;
	}

	internal override void NativeInit( IntPtr ptr )
	{
		base.NativeInit( ptr );

		_gridlayout = (QGridLayout)_layout;
	}

	internal override void NativeShutdown()
	{
		_gridlayout = default;

		base.NativeShutdown();
	}
}
