namespace Sandbox.UI;

public class SelectionEvent : PanelEvent
{
	public Rect SelectionRect;
	public Vector2 StartPoint;
	public Vector2 EndPoint;

	public SelectionEvent( string event_name, Panel active ) : base( event_name, active )
	{
		Name = event_name;
		Target = active;
	}
}
