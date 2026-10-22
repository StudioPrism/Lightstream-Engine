using System;
using System.Collections.Generic;

namespace Editor;

/// <summary>
/// Base class for dropdown control widgets with multi-select support.
/// </summary>
public abstract class DropdownControlWidget : ControlWidget
{
	public override bool IsControlActive => base.IsControlActive || _menu.IsValid();
	public override bool IsControlButton => true;
	public override bool IsControlHovered => base.IsControlHovered || _menu.IsValid();

	protected PopupWidget _menu;
	protected bool IsMultiSelect { get; set; }

	public DropdownControlWidget( SerializedProperty property ) : base( property )
	{
		Cursor = CursorShape.Finger;
		Layout = Layout.Row();
		Layout.Spacing = 2;
	}

	protected override void PaintControl()
	{
		var color = IsControlHovered ? Theme.Blue : Theme.TextControl;
		if ( IsControlDisabled ) color = color.WithAlpha( 0.5f );

		var rect = LocalRect.Shrink( 8, 0 );

		if ( SerializedProperty.IsMultipleDifferentValues )
		{
			Paint.SetPen( Theme.MultipleValues );
			Paint.DrawText( rect, "Multiple Values", TextFlag.LeftCenter );
		}
		else
		{
			PaintDisplayText( rect, color );
		}

		Paint.SetPen( color );
		Paint.DrawIcon( rect, "Arrow_Drop_Down", 17, TextFlag.RightCenter );
	}

	/// <summary>
	/// Override to paint the display text in the control
	/// </summary>
	protected abstract void PaintDisplayText( Rect rect, Color color );

	public override void StartEditing()
	{
		if ( IsControlDisabled ) return;

		if ( !_menu.IsValid )
		{
			OpenMenu();
		}
	}

	protected override void OnMouseClick( MouseEvent e )
	{
		if ( IsControlDisabled ) return;

		if ( e.LeftMouseButton && !_menu.IsValid() )
		{
			OpenMenu();
		}
	}

	protected override void OnDoubleClick( MouseEvent e )
	{
		// nothing
	}

	void OpenMenu()
	{
		PropertyStartEdit();

		_menu = new PopupWidget( null );
		_menu.Layout = Layout.Column();
		_menu.MinimumWidth = ScreenRect.Width;
		_menu.MaximumWidth = ScreenRect.Width;
		_menu.OnLostFocus += PropertyFinishEdit;

		var scroller = _menu.Layout.Add( new ScrollArea( this ), 1 );
		scroller.Canvas = new Widget( scroller )
		{
			Layout = Layout.Column(),
			VerticalSizeMode = SizeMode.CanGrow | SizeMode.Expand,
			MaximumWidth = ScreenRect.Width
		};

		PopulateMenu( scroller.Canvas );

		_menu.Position = ScreenRect.BottomLeft;
		_menu.Visible = true;
		_menu.AdjustSize();
		_menu.ConstrainToScreen();
		_menu.OnPaintOverride = PaintMenuBackground;

		if ( scroller.VerticalScrollbar.Minimum != scroller.VerticalScrollbar.Maximum )
		{
			scroller.Canvas.MaximumWidth -= 8;
		}
	}

	/// <summary>
	/// Override to populate the menu with options
	/// </summary>
	protected abstract void PopulateMenu( Widget canvas );

	/// <summary>
	/// Helper to add a menu option
	/// </summary>
	protected Widget AddMenuOption( Widget canvas, string displayName, string icon, Func<bool> isSelected, Action onSelect )
	{
		var option = canvas.Layout.Add( new DropdownMenuOption( displayName, icon, isSelected ) );
		option.MouseLeftPress = () =>
		{
			onSelect();
			_menu.Update();

			if ( !IsMultiSelect )
			{
				_menu.Close();
			}
		};
		return option;
	}

	bool PaintMenuBackground()
	{
		Paint.SetBrushAndPen( Theme.ControlBackground );
		Paint.DrawRect( Paint.LocalRect, 0 );
		return true;
	}
}

file class DropdownMenuOption : Widget
{
	string displayName;
	string icon;
	Func<bool> isSelectedFunc;

	public DropdownMenuOption( string displayName, string icon, Func<bool> isSelectedFunc ) : base( null )
	{
		this.displayName = displayName;
		this.icon = icon;
		this.isSelectedFunc = isSelectedFunc;

		Layout = Layout.Row();
		Layout.Margin = 8;
		VerticalSizeMode = SizeMode.CanGrow;

		if ( !string.IsNullOrWhiteSpace( icon ) )
		{
			Layout.Add( new IconButton( icon ) { Background = Color.Transparent, TransparentForMouseEvents = true, IconSize = 18 } );
			Layout.AddSpacingCell( 8 );
		}

		Layout.Add( new Label( displayName ) );
	}

	protected override void OnPaint()
	{
		var isSelected = isSelectedFunc?.Invoke() ?? false;
		if ( Paint.HasMouseOver || isSelected )
		{
			Paint.SetBrushAndPen( Theme.Blue.WithAlpha( isSelected ? 0.3f : 0.1f ) );
			Paint.DrawRect( LocalRect.Shrink( 2 ), 2 );
		}
	}
}
