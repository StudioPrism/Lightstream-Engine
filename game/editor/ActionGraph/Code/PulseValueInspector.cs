
using Editor.NodeEditor;

namespace Editor.ActionGraphs;

public class PulseValueInspector : GraphicsItem
{
	private object _value;

	public object Value
	{
		get => _value;
		set
		{
			_value = value;

			Layout();
			Update();
		}
	}

	private readonly GraphView _graphView;
	private Vector2 _targetPos;

	public GraphicsItem Target { get; set; }

	public Vector2 TargetPosition
	{
		get => _targetPos;
		set
		{
			_targetPos = value;
			Layout();
		}
	}

	public PulseValueInspector( GraphView graphView )
	{
		_graphView = graphView;

		Focusable = false;
		Selectable = false;
		HoverEvents = false;

		ZIndex = 100f;
	}

	public void Layout()
	{
		var type = Value?.GetType() ?? typeof( object );
		var text = PaintHelper.FormatValue( type, Value, out var extraWidth, out object rawValue );

		Paint.SetDefaultFont();

		var textSize = Paint.MeasureText( text );

		Size = textSize + new Vector2( 20f, 10f );
		Position = TargetPosition - new Vector2( Size.x * 0.5f, Size.y + 10f );
	}

	public override bool Contains( Vector2 localPos )
	{
		return false;
	}

	protected override void OnPaint()
	{
		Paint.Antialiasing = true;
		Paint.TextAntialiasing = true;

		Paint.SetDefaultFont();

		var type = Value?.GetType() ?? typeof( object );
		var handleConfig = _graphView.GetHandleConfig( type );

		var text = PaintHelper.FormatValue( type, Value, out var extraWidth, out object rawValue );

		PaintHelper.DrawValue( handleConfig, LocalRect, text, 1f, null, rawValue: rawValue );
	}
}
