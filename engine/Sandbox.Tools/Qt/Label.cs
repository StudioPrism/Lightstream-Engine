using Native;
using System;

namespace Editor
{
	/// <summary>
	/// A simple text label.
	/// </summary>
	public class Label : Widget
	{
		internal Native.QLabel _label;

		/// <summary>
		/// The text of this label.
		/// </summary>
		public string Text
		{
			get => _label.text();
			set => _label.setText( value );
		}

		public bool WordWrap
		{
			get => _label.wordWrap();
			set => _label.setWordWrap( value );
		}

		public bool TextSelectable
		{
			get => (_label.textInteractionFlags() & TextInteractionFlags.TextSelectableByMouse) != 0;
			set
			{
				var flags = _label.textInteractionFlags();
				if ( value )
				{
					flags |= TextInteractionFlags.TextSelectableByMouse;
					flags |= TextInteractionFlags.TextSelectableByKeyboard;
				}
				else
				{
					flags &= ~TextInteractionFlags.TextSelectableByMouse;
					flags &= ~TextInteractionFlags.TextSelectableByKeyboard;

				}

				_label.setTextInteractionFlags( flags );
			}
		}

		/// <summary>
		/// If true, clicking a html link on this label will open the website.
		/// This is true by default.
		/// </summary>
		public bool OpenExternalLinks
		{
			get => _label.openExternalLinks();
			set => _label.setOpenExternalLinks( value );
		}

		public float Indent
		{
			get => _label.indent();
			set => _label.setIndent( (int)value );
		}

		public float Margin
		{
			get => _label.margin();
			set => _label.setMargin( (int)value );
		}

		Color _color = new Color( 0.6f, 0.6f, 0.6f );

		public Color Color
		{
			get => _color;
			set
			{
				if ( _color == value ) return;

				_color = value;
				SetStyles( $"color: {Color.Hex};" );
			}
		}

		public TextFlag Alignment
		{
			get => (TextFlag)_label.alignment();
			set => _label.setAlignment( (int)value );
		}

		public Label( Widget parent = null ) : base( false )
		{
			Sandbox.InteropSystem.Alloc( this );
			var ptr = CLabel.CreateLabel( parent?._widget ?? default, this );
			NativeInit( ptr );
			Indent = 0;
			OpenExternalLinks = true;
		}

		public Label( string text, Widget parent = null ) : base( false )
		{
			Sandbox.InteropSystem.Alloc( this );
			var ptr = CLabel.CreateLabel( parent?._widget ?? default, this );
			NativeInit( ptr );

			Text = text;
			Indent = 0;
			OpenExternalLinks = true;
		}

		internal override void NativeInit( IntPtr ptr )
		{
			_label = ptr;

			base.NativeInit( ptr );
		}
		internal override void NativeShutdown()
		{
			base.NativeShutdown();

			_label = default;
		}

		public class Title : Label
		{
			public Title( Widget parent = null ) : this( null, parent ) { }

			public Title( string title, Widget parent = null ) : base( title, parent )
			{
				SetProperty( "type", "title" );
				WordWrap = true;
			}
		}

		public class Subtitle : Label
		{
			public Subtitle( Widget parent = null ) : this( null, parent ) { }

			public Subtitle( string title, Widget parent = null ) : base( title, parent )
			{
				SetProperty( "type", "subtitle" );
				WordWrap = true;
			}
		}

		public class Small : Label
		{
			public Small( Widget parent = null ) : this( null, parent ) { }

			public Small( string title, Widget parent = null ) : base( title, parent )
			{
				SetProperty( "type", "small" );
				WordWrap = true;
			}
		}

		public class Body : Label
		{
			public Body( Widget parent = null ) : this( null, parent ) { }

			public Body( string title, Widget parent = null ) : base( title, parent )
			{
				SetProperty( "type", "body" );
				WordWrap = true;
			}
		}

		public class Header : Label
		{
			public Header( Widget parent = null ) : this( null, parent ) { }

			public Header( string title, Widget parent = null ) : base( title, parent )
			{
				SetStyles( "font-weight: bold;" );
				FixedHeight = Theme.RowHeight;
				WordWrap = true;
			}
		}
	}
}
