using System;

namespace Editor
{
	public class Option : QObject
	{
		internal Native.QAction _action;

		/// <inheritdoc cref="OnTriggered"/>
		public Action Triggered;

		/// <inheritdoc cref="OnToggled"/>
		public Action<bool> Toggled;

		/// <summary>
		/// A method to get the checked state. Called periodically to update the status
		/// </summary>
		public Func<bool> FetchCheckedState;

		/// <summary>
		/// Text for this option.
		/// </summary>
		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;
				UpdateText();
			}
		}
		string _text;

		/// <summary>
		/// Text to display if <see cref="Text"/> is empty.
		/// </summary>
		public string IconText
		{
			get => _action.iconText();
			set => _action.setIconText( value );
		}

		/// <summary>
		/// Whether this option is a toggle option. <see cref="Checked"/>.
		/// </summary>
		public bool Checkable
		{
			get => _action.isCheckable();
			set => _action.setCheckable( value );
		}

		/// <summary>
		/// Whether this option is toggled/checked. <see cref="Checkable"/>.
		/// </summary>
		public bool Checked
		{
			get => _action.isChecked();
			set => _action.setChecked( value );
		}

		[Obsolete( $"Use {nameof( ToolTip )}" )]
		public string Tooltip
		{
			get => ToolTip;
			set => ToolTip = value;
		}

		private string _tooltip;

		/// <inheritdoc cref="Widget.ToolTip"/>
		public string ToolTip
		{
			get => _tooltip;
			set => _action.setToolTip( _tooltip = value );
		}

		[Obsolete( $"Use {nameof( StatusTip )}" )]
		public string StatusText
		{
			get => StatusTip;
			set => StatusTip = value;
		}

		/// <inheritdoc cref="Widget.StatusTip"/>
		public string StatusTip
		{
			get => _action.statusTip();
			set => _action.setStatusTip( value );
		}

		/// <summary>
		/// Whether this option can be clicked. Will also be visually different.
		/// </summary>
		public bool Enabled
		{
			get => _action.isEnabled();
			set => _action.setEnabled( value );
		}


		[Obsolete( "Please use ShortcutName, which takes a shortcut ident (such as editor.save) instead of keys (such as CTRL+S)." )]
		public string Shortcut { get => _shortcutName; set => _shortcutName = value; }

		public string ShortcutName
		{
			get => _shortcutName;
			set
			{
				_shortcutName = value;
				UpdateText();
			}
		}
		string _shortcutName;

		string _icon;
		Pixmap _iconImage;

		/// <summary>
		/// The icon for this option.
		/// </summary>
		public string Icon
		{
			get => _icon;
			set
			{
				if ( _icon == value ) return;

				_icon = value;
				_action.setIcon( _icon );
			}
		}

		/// <summary>
		/// The icon for this option.
		/// </summary>
		public Pixmap IconImage
		{
			get => _iconImage;
			set
			{
				if ( _iconImage == value ) return;

				_iconImage = value;
				_action.setIconFromPixmap( _iconImage.ptr );
			}
		}

		internal Option( IntPtr ptr )
		{
			NativeInit( ptr );
		}


		public Option( QObject parent, string title = null, string icon = null, Action action = null )
		{
			Sandbox.InteropSystem.Alloc( this );
			NativeInit( Native.CAction.Create( parent?._object ?? default, this ) );

			if ( title != null ) Text = title;
			if ( icon != null ) Icon = icon;
			if ( action != null ) Triggered = action;
		}

		public Option( QObject parent, string title, Pixmap icon, Action action = null )
		{
			Sandbox.InteropSystem.Alloc( this );
			NativeInit( Native.CAction.Create( parent?._object ?? default, this ) );

			if ( title != null ) Text = title;
			if ( icon != null ) IconImage = icon;
			if ( action != null ) Triggered = action;
		}

		public Option( string title = null, string icon = null, Action action = null ) : this( null, title, icon, action )
		{
		}

		internal override void NativeInit( IntPtr ptr )
		{
			_action = ptr;
			base.NativeInit( ptr );
		}

		internal override void NativeShutdown()
		{
			base.NativeShutdown();

			_action = default;
			Triggered = default;
			Toggled = default;
		}

		/// <summary>
		/// Called when this option was clicked..
		/// </summary>
		protected virtual void OnTriggered()
		{
			Triggered?.Invoke();
		}

		internal void InternalTriggered()
		{
			OnTriggered();
		}

		[Event( "keybinds.update" )]
		void UpdateText()
		{
			if ( string.IsNullOrEmpty( _shortcutName ) )
			{
				_action.setText( _text );
				return;
			}

			var keys = EditorShortcuts.GetDisplayKeys( _shortcutName );

			// Don't do this anymore... Since this *actually* registers an extra shortcut
			// _action.setShortcut( _shortcut );

			// Instead add the shortcut to the text with a tab
			_action.setText( $"{_text}\t{keys}" );

			// Also set the tooltip as we did before
			_action.setToolTip( $"{_tooltip} ( {keys} )" );
		}

		/// <summary>
		/// Called when this option was toggled.
		/// </summary>
		protected virtual void OnToggled( bool b )
		{
			Toggled?.Invoke( b );
		}

		internal void InternalToggled( bool b )
		{
			OnToggled( b );
		}

		internal void AboutToShow()
		{
			if ( !_action.IsValid )
				return;

			if ( FetchCheckedState is not null )
			{
				Checked = FetchCheckedState();
			}
		}

		/// <summary>
		/// Sets an icon for the option via a raw image.
		/// </summary>
		public void SetIcon( Pixmap pixmap )
		{
			_action.setIconFromPixmap( pixmap.ptr );
		}
	}
}
