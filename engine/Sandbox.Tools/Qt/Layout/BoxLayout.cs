using Native;
using Sandbox;
using System;

namespace Editor;

/// <summary>
/// A widget layout. You can think of it as an invisible box of rows or columns, each one containing a widget, useful for automatic positioning and scaling.
/// </summary>
public sealed class BoxLayout : Layout
{
	internal Native.QBoxLayout _boxlayout;

	internal enum Direction
	{
		LeftToRight,
		RightToLeft,
		TopToBottom,
		BottomToTop,
	};

	internal BoxLayout( Direction direction, Widget parent ) : base( Native.QBoxLayout.Create( direction, parent?._widget ?? default ) )
	{
		Assert.True( _boxlayout.IsValid );
	}

	/// <summary>
	/// Add a spacing item
	/// </summary>
	public override void AddSpacingCell( float size )
	{
		_boxlayout.addSpacing( (int)size );
	}

	/// <summary>
	/// Add a stretch item
	/// </summary>
	public override void AddStretchCell( int stretch = 0 )
	{
		if ( stretch < 0 )
			stretch = 0;

		_boxlayout.addStretch( stretch );
	}

	public int GetCellStretch( int index )
	{
		return _boxlayout.stretch( index );
	}

	public void SetCellStretch( int index, int stretch )
	{
		_boxlayout.setStretch( index, stretch );
	}

	public void SetCellStretch( Widget widget, int stretch )
	{
		if ( !widget.IsValid() ) throw new ArgumentNullException( nameof( widget ) );
		_boxlayout.setStretchFactor( widget._widget, stretch );
	}
	public void SetCellStretch( Layout layout, int stretch )
	{
		if ( !layout.IsValid() ) throw new ArgumentNullException( nameof( layout ) );
		_boxlayout.setStretchFactor( layout._layout, stretch );
	}

	public override T Add<T>( T widget, int stretch = 0 )
	{
		if ( !widget.IsValid() ) throw new ArgumentNullException( nameof( widget ) );
		_boxlayout.addWidget( widget._widget, stretch );
		return widget;
	}

	public override Layout Add( Layout layout )
	{
		if ( !layout.IsValid() ) throw new ArgumentNullException( nameof( layout ) );
		_boxlayout.addLayout( layout._layout, 0 );
		return layout;
	}

	public override Layout Add( Layout layout, int stretch )
	{
		if ( !layout.IsValid() ) throw new ArgumentNullException( nameof( layout ) );

		_boxlayout.addLayout( layout._layout, stretch );
		return layout;
	}

	internal override void NativeInit( IntPtr ptr )
	{
		base.NativeInit( ptr );

		_boxlayout = (QBoxLayout)_layout;
	}

	internal override void NativeShutdown()
	{
		_boxlayout = default;
		base.NativeShutdown();
	}


}
