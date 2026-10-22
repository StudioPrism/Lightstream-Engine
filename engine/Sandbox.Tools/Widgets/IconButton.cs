using System;

namespace Editor;

public class IconButton : Widget
{
	public string Icon { get; set; }
	public Action OnClick { get; set; }
	public float IconSize { get; set; } = 12.0f;
	public Color Background { get; set; }
	public Color Foreground { get; set; }

	public Color BackgroundActive { get; set; }
	public Color ForegroundActive { get; set; }

	/// <summary>
	/// If true we will toggle IsActive automatically
	/// </summary>
	public bool IsToggle { get; set; }



	bool _active;

	/// <summary>
	/// If IsToggle is true, this is toggled on press
	/// </summary>
	public bool IsActive
	{
		get => _active;
		set
		{
			if ( _active == value ) return;
			_active = value;
			Update();
			OnToggled?.Invoke( _active );
		}
	}

	public Action<bool> OnToggled { get; set; }

	public IconButton( string icon, Action onClick = null, Widget parent = null ) : base( parent )
	{
		Background = Theme.ButtonBackground;
		BackgroundActive = Theme.Primary;

		Foreground = Theme.TextButton;
		ForegroundActive = Theme.Text;

		this.Icon = icon;
		this.OnClick = onClick;

		Cursor = CursorShape.Finger;
		FixedHeight = Theme.RowHeight;
		FixedWidth = Theme.RowHeight;
	}

	protected override Vector2 SizeHint()
	{
		return MinimumWidth;
	}

	protected override void OnMouseClick( MouseEvent e )
	{
		base.OnMouseClick( e );

		if ( IsToggle )
		{
			IsActive = !IsActive;
		}

		OnClick?.Invoke();

		e.Accepted = true;
	}

	protected override void OnPaint()
	{
		Paint.ClearBrush();
		Paint.ClearPen();

		bool active = Enabled && IsActive;

		var bg = active ? BackgroundActive : Background;
		var fg = active ? ForegroundActive : Foreground;

		Paint.SetBrush( bg );
		Paint.DrawRect( LocalRect, 2.0f );

		Paint.ClearBrush();
		Paint.ClearPen();

		Paint.Pen = fg.WithAlphaMultiplied( Paint.HasMouseOver ? 1.0f : 0.7f );
		if ( !Enabled ) Paint.Pen = fg.WithAlphaMultiplied( 0.25f );

		Paint.DrawIcon( LocalRect, Icon, IconSize, TextFlag.Center );
	}

	public class WithNumber : IconButton
	{
		public int Number { get; set; }

		public WithNumber( string icon, Action onClick = null, Widget parent = null ) : base( icon, onClick, parent )
		{
			FixedWidth = Theme.RowHeight * 2;
		}

		protected override Vector2 SizeHint() => new Vector2( Theme.RowHeight * 2, Theme.RowHeight );

		protected override void OnPaint()
		{
			Paint.ClearBrush();
			Paint.ClearPen();

			float alpha = Paint.HasMouseOver ? 0.5f : 0.25f;

			if ( !Enabled )
				alpha = 0.1f;

			Paint.SetBrush( Background.WithAlphaMultiplied( alpha ) );
			Paint.DrawRect( LocalRect, 2.0f );

			Paint.ClearBrush();
			Paint.ClearPen();

			Paint.Pen = Foreground.WithAlphaMultiplied( Paint.HasMouseOver ? 1.0f : 0.7f );
			if ( !Enabled ) Paint.Pen = Foreground.WithAlphaMultiplied( 0.25f );

			var contentRect = LocalRect.Shrink( 8, 0 );

			Paint.DrawIcon( contentRect, Icon, IconSize, TextFlag.LeftCenter );

			Paint.SetHeadingFont( 8, 500 );
			Paint.DrawText( contentRect, Number.ToString(), TextFlag.RightCenter );
		}
	}

	public class WithCornerIcon : IconButton
	{
		/// <summary>
		/// Icon to show in the bottom right corner
		/// </summary>
		public string CornerIcon { get; set; } = "arrow_drop_down";

		/// <summary>
		/// The size of the icon in the corner
		/// </summary>
		public float CornerIconSize { get; set; } = 16.0f;

		/// <summary>
		/// The color of the icon in the corner
		/// </summary>
		public Color CornerIconColor { get; set; } = Color.White;

		/// <summary>
		/// The position of the icon in the corner
		/// </summary>
		public Vector2 CornerIconOffset { get; set; } = 2;

		public WithCornerIcon( string icon, Action onClick = null, Widget parent = null ) : base( icon, onClick, parent )
		{

		}

		protected override void OnPaint()
		{
			base.OnPaint();

			if ( !string.IsNullOrWhiteSpace( CornerIcon ) )
			{
				Paint.Pen = CornerIconColor;
				Paint.DrawIcon( LocalRect + CornerIconOffset, CornerIcon, CornerIconSize, TextFlag.RightBottom );
			}
		}
	}
}
