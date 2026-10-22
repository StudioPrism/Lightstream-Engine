using System;

namespace Editor
{
	/// <summary>
	/// Split frame, allows dragging to resize panels
	/// </summary>
	public class Splitter : Frame
	{
		internal Native.QSplitter _splitter;

		internal Splitter()
		{
		}

		public Splitter( Widget parent )
		{
			var widget = Native.QSplitter.CreateSplitter( parent?._widget ?? default );
			NativeInit( widget );
		}

		internal override void NativeInit( IntPtr ptr )
		{
			_splitter = ptr;

			base.NativeInit( ptr );
		}

		internal override void NativeShutdown()
		{
			_frame = default;

			base.NativeShutdown();
		}

		public void AddWidget( Widget w )
		{
			_splitter.addWidget( w._widget );
		}

		public bool IsHorizontal
		{
			get => _splitter.orientation() == Orientation.Horizontal;
			set => _splitter.setOrientation( Orientation.Horizontal );
		}

		public bool IsVertical
		{
			get => _splitter.orientation() == Orientation.Vertical;
			set => _splitter.setOrientation( Orientation.Vertical );
		}

		public bool OpaqueResize
		{
			get => _splitter.opaqueResize();
			set => _splitter.setOpaqueResize( value );
		}

		public int HandleWidth
		{
			get => _splitter.handleWidth();
			set => _splitter.setHandleWidth( value );
		}

		public string SaveState() => _splitter.saveState();
		public void RestoreState( string state ) => _splitter.restoreState( state );

		public void SetStretch( int cell, int stretch )
		{
			_splitter.setStretchFactor( cell, stretch );
		}

		public void SetCollapsible( int index, bool collapsible )
		{
			_splitter.setCollapsible( index, collapsible );
		}
	}
}
