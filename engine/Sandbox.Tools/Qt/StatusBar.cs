using System;

namespace Editor
{
	public class StatusBar : Widget
	{
		internal Native.QStatusBar _statusbar;

		internal StatusBar( Native.QStatusBar widget ) : base( false )
		{
			NativeInit( widget );
		}

		public StatusBar( Widget parent ) : base( false )
		{
			Sandbox.InteropSystem.Alloc( this );
			NativeInit( CStatusBar.Create( parent?._widget ?? default, this ) );

			SizeGrip = false;
		}

		internal override void NativeInit( IntPtr ptr )
		{
			_statusbar = ptr;

			base.NativeInit( ptr );
		}
		internal override void NativeShutdown()
		{
			base.NativeShutdown();

			_statusbar = default;
		}

		public void AddWidgetLeft( Widget w, int stretch = 0 )
		{
			_statusbar.addWidget( w._widget, stretch );
		}

		public void RemoveWidget( Widget w )
		{
			_statusbar.removeWidget( w._widget );
		}

		public void AddWidgetRight( Widget w, int stretch = 0 )
		{
			_statusbar.addPermanentWidget( w._widget, stretch );
		}

		public void ShowMessage( string text, float seconds = 5.0f )
		{
			_statusbar.showMessage( text, (int)(seconds * 1000.0f) );
		}

		public bool SizeGrip
		{
			get { return _statusbar.isSizeGripEnabled(); }
			set { _statusbar.setSizeGripEnabled( value ); }
		}

	}
}
