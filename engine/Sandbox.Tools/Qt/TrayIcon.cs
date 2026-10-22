using Sandbox;
using System;
using System.Collections.Generic;

namespace Editor
{
	/// <summary>
	/// Like a widget - but is drawn
	/// </summary>
	public class TrayIcon : QObject
	{
		internal CSystemTrayIcon _trayIcon;

		public TrayIcon( QObject parent )
		{
			InteropSystem.Alloc( this );
			NativeInit( CSystemTrayIcon.Create( parent?._object ?? default, this ) );
		}

		public bool Visible
		{
			get => _trayIcon.isVisible();
			set
			{
				if ( value ) _trayIcon.show(); else _trayIcon.hide();
			}
		}

		public void SetIcon( string icon )
		{
			_trayIcon.setIcon( icon );
		}

		public void ShowMessage( string title, string message, string icon, float seconds )
		{
			_trayIcon.showMessage( title, message, icon, (int)(seconds * 1000.0f) );
		}

		internal override void NativeInit( IntPtr ptr )
		{
			_trayIcon = ptr;

			base.NativeInit( ptr );
		}

		internal override void NativeShutdown()
		{
			_trayIcon = default;

			base.NativeShutdown();
		}

		internal void InternalActivated()
		{
			Log.Info( "Icon Activated" );
		}

		internal void InternalMessageClicked()
		{
			Log.Info( "Message Clicked" );
		}
	}
}
