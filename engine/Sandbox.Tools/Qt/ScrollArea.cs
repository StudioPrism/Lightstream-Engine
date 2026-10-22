using System;

namespace Editor
{
	/// <summary>
	/// A widget that can scroll its <see cref="Canvas"/>.
	/// </summary>
	public class ScrollArea : Frame
	{
		internal Native.QScrollArea _scrollarea;

		internal ScrollArea( IntPtr widget )
		{
			NativeInit( widget );
		}

		public ScrollBar VerticalScrollbar { get; init; }
		public ScrollBar HorizontalScrollbar { get; init; }

		public ScrollArea( Widget parent )
		{
			var ptr = Native.QScrollArea.CreateQScrollArea( parent?._widget ?? default );
			NativeInit( ptr );

			_scrollarea.setWidgetResizable( true );

			VerticalScrollbar = new ScrollBar( _scrollarea.verticalScrollBar() );
			HorizontalScrollbar = new ScrollBar( _scrollarea.horizontalScrollBar() );
		}

		Widget _canvas;

		/// <summary>
		/// The content widget to scroll.
		/// </summary>
		public Widget Canvas
		{
			get => _canvas;
			set
			{
				if ( value is not null )
				{
					Assert.False( value == this, "Tried to set scrollarea canvas to itself!" );
					Assert.False( value.IsAncestorOf( this ), "Tried to set scrollarea canvas to a parent of itself!" );
				}

				_canvas = value;
				_scrollarea.setWidget( value?._widget ?? default );

				value?._widget.setAutoFillBackground( false );
			}
		}

		internal override void NativeInit( IntPtr ptr )
		{
			_scrollarea = ptr;

			base.NativeInit( ptr );
		}

		internal override void NativeShutdown()
		{
			_scrollarea = default;

			base.NativeShutdown();
		}

		public ScrollbarMode HorizontalScrollbarMode
		{
			get => _scrollarea.horizontalScrollBarPolicy();
			set => _scrollarea.setHorizontalScrollBarPolicy( value );
		}

		public ScrollbarMode VerticalScrollbarMode
		{
			get => _scrollarea.verticalScrollBarPolicy();
			set => _scrollarea.setVerticalScrollBarPolicy( value );
		}

		public void MakeVisible( Widget widget )
		{
			_scrollarea.ensureWidgetVisible( widget._widget, 50, 50 );
		}

		public void MakeVisible( Vector3 pos )
		{
			_scrollarea.ensureVisible( pos.x.CeilToInt(), pos.y.CeilToInt(), 50, 50 );
		}
	}
}
