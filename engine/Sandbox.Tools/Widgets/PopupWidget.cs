using System;

namespace Editor;

/// <summary>
/// A popup widget that automatically deletes itself once it stops being visible
/// </summary>
public class PopupWidget : Widget
{
	public bool PreventDestruction = false;

	public Action OnLostFocus { get; set; }

	public PopupWidget( Widget widget ) : base( widget )
	{
		WindowFlags = WindowFlags.Popup;
		DeleteOnClose = true;
		MouseTracking = true;
	}

	protected override void OnVisibilityChanged( bool visible )
	{
		base.OnVisibilityChanged( visible );

		if ( !visible && !PreventDestruction )
		{
			OnLostFocus?.Invoke();
			Destroy();
		}
	}

	protected override void OnPaint()
	{
		Paint.Antialiasing = true;

		Paint.SetPen( Theme.WidgetBackground.Lighten( 0.3f ), 2 );
		Paint.SetBrush( Theme.WidgetBackground );
		Paint.DrawRect( LocalRect.Shrink( 2 ), 2 );

		Paint.SetPen( Theme.WidgetBackground.Darken( 0.5f ), 1 );
		Paint.ClearBrush();
		Paint.DrawRect( LocalRect.Shrink( 1 ), 2 );
	}

	public void OpenAtCursor( bool animate = true, Vector2? offset = null )
	{
		var offsetActual = Vector2.Zero;
		if ( offset.HasValue ) offsetActual = offset.Value;

		var pos = Application.CursorPosition + offsetActual;

		OpenAt( pos, animate );
	}

	public void OpenAt( Vector2 position, bool animate = true, Vector2? animateOffset = null )
	{
		if ( animate )
			WindowOpacity = 0;

		Position = position;

		Show();

		ConstrainToScreen();

		if ( animate )
		{
			var offset = animateOffset ?? new Vector2( 0, -8 );
			Animate.Add( this, 0.2f, Position.y + offset.y, Position.y, y => { Position = Position.WithY( y ); OnMoved(); }, "ease-out" );
			Animate.Add( this, 0.2f, Position.x + offset.x, Position.x, x => { Position = Position.WithX( x ); OnMoved(); }, "ease-out" );
			Animate.Add( this, 0.1f, 0, 1, y => WindowOpacity = y, "linear" );
		}
	}

	/// <summary>
	/// Open the window this many pixels below the cursor.
	/// </summary>
	public void OpenBelowCursor( float distance, float centering = 0.5f )
	{
		Show();
		Position = Application.CursorPosition - Size * new Vector2( centering, 0.0f ) + new Vector2( 0, distance );
		ConstrainToScreen();
	}
}



