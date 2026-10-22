using System;
using Editor.NodeEditor;
using Facepunch.ActionGraphs;
using Sandbox;
using DisplayInfo = Sandbox.DisplayInfo;

namespace Editor.ActionGraphs;

public interface IPulseTarget
{
	void UpdatePulse( float scale, Color colorTint );
}

public abstract class DefaultEditor<TPlugUI, TActionPlug, TParam> : ValueEditor, IPulseTarget
	where TPlugUI : Plug
	where TActionPlug : ActionPlug<TParam>
	where TParam : Node.IParameter
{
	public NodeUI Node { get; }
	public TPlugUI PlugUI { get; }
	public TActionPlug ActionPlug { get; }
	public TParam Param { get; }

	public override bool HideLabel => false;

	protected float PulseScale { get; private set; } = 1f;
	protected Color ColorTint { get; private set; }

	protected DefaultEditor( NodeUI node, TPlugUI parent )
		: base( parent )
	{
		HoverEvents = true;
		Cursor = CursorShape.Finger;

		Node = node;
		PlugUI = parent;
		ActionPlug = (TActionPlug)parent.Inner;
		Param = ActionPlug.Parameter;

		ZIndex = -1;
	}

	public void UpdatePulse( float scale, Color colorTint )
	{
		PulseScale = scale;
		ColorTint = colorTint;
		Update();
	}

	protected override void OnMousePressed( GraphicsMouseEvent ev )
	{
		PlugUI.MousePressed( ev );
	}

	protected override void OnMouseReleased( GraphicsMouseEvent ev )
	{
		PlugUI.MouseReleased( ev );
	}

	protected override void OnMouseMove( GraphicsMouseEvent ev )
	{
		PlugUI.MouseMove( ev );
	}
}

public class DefaultInputEditor : DefaultEditor<PlugIn, ActionInputPlug, Node.Input>
{
	public override Rect BoundingRect => base.BoundingRect.Grow( _labelWidth + 24f, 0f, 0f, 0f );

	private float _labelWidth;
	private int _textHash;

	public DefaultInputEditor( NodeUI node, PlugIn parent )
		: base( node, parent )
	{
	}

	protected override void OnPaint()
	{
		var link = ActionPlug.InputLink;
		var type = ActionPlug.Type;

		var eventArgs = new BuildInputLabelEvent( (ActionGraphView)Node.Graph, ActionPlug );

		EditorEvent.Run( BuildInputLabelEvent.EventName, eventArgs );

		Enabled = eventArgs.Handled;
		Enabled &= eventArgs.Value is not null || eventArgs.Text is not null && eventArgs.Icon is not null;

		var prevTextHash = _textHash;
		var prevLabelWidth = _labelWidth;

		try
		{
			_labelWidth = 0f;

			if ( !Enabled )
				return;

			Paint.Antialiasing = true;
			Paint.TextAntialiasing = true;

			var shrink = 10f;
			var extraWidth = 0f;

			object rawValue = null;
			var text = eventArgs.Text ?? PaintHelper.FormatValue( type, eventArgs.Value, out extraWidth, out rawValue );
			var textSize = Paint.MeasureText( text ) + extraWidth;

			var valueRect = new Rect( LocalRect.Left - textSize.x - shrink * 2 - 20f, LocalRect.Top, textSize.x + shrink * 2,
					LocalRect.Height )
				.Shrink( 0f, 2f, 0f, 2f );

			if ( eventArgs.Icon is not null )
			{
				valueRect = valueRect.Grow( 20f, 0f, 0f, 0f );
			}

			var handleConfig = PlugUI.HandleConfig;

			handleConfig.Color = Color.Lerp( handleConfig.Color, ColorTint.WithAlpha( 1f ), ColorTint.a );

			Paint.SetPen( handleConfig.Color, 4f * PulseScale );
			Paint.DrawLine( LocalRect.Center.WithX( LocalRect.Left - 9f - 2f * PulseScale ), LocalRect.Center.WithX( LocalRect.Left - 32f ) );

			PaintHelper.DrawValue( handleConfig, valueRect, text, PulseScale, eventArgs.Icon, rawValue );

			_labelWidth = valueRect.Width;
			_textHash = text.FastHash();
		}
		finally
		{
			if ( _textHash != prevTextHash || Math.Abs( _labelWidth - prevLabelWidth ) > 0.01f )
			{
				PrepareGeometryChange();
			}
		}
	}

	[Event( BuildInputLabelEvent.EventName )]
	public static void BuildDefaultInputLabel( BuildInputLabelEvent eventArgs )
	{
		if ( eventArgs.Handled ) return;
		if ( eventArgs.Input.IsLinked ) return;
		if ( eventArgs.Input.Definition.IsRequired ) return;
		if ( eventArgs.Input.Definition.Default is not { } defaultValue || defaultValue is Array ) return;

		eventArgs.Value = defaultValue;
		eventArgs.Handled = true;
	}

	[Event( BuildInputLabelEvent.EventName )]
	public static void BuildConstantInputLabel( BuildInputLabelEvent eventArgs )
	{
		if ( eventArgs.Handled ) return;
		if ( eventArgs.Link?.TryGetConstant( out var constValue ) is not true ) return;

		eventArgs.Value = constValue;
		eventArgs.Handled = true;
	}

	[Event( BuildInputLabelEvent.EventName )]
	public static void BuildVariableInputLabel( BuildInputLabelEvent eventArgs )
	{
		if ( eventArgs.Handled ) return;
		if ( eventArgs.Link?.TryGetVariable( out var variable ) is not true ) return;

		eventArgs.Text = variable.Name;
		eventArgs.Icon = "inbox";
		eventArgs.Handled = true;
	}

	[Event( BuildInputLabelEvent.EventName )]
	public static void BuildComponentInputLabel( BuildInputLabelEvent eventArgs )
	{
		if ( eventArgs.Handled ) return;
		if ( eventArgs.Link is null ) return;
		if ( eventArgs.Link.Source.Node.Parent != eventArgs.Plug.Node.Node ) return;
		if ( eventArgs.Link.Source.Node.Definition.Identifier != "scene.get" ) return;

		var displayInfo = DisplayInfo.ForType( eventArgs.Link.Source.Type );

		eventArgs.Text = displayInfo.Name;
		eventArgs.Icon = displayInfo.Icon ?? "category";
		eventArgs.Handled = true;
	}

	[Event( BuildInputLabelEvent.EventName )]
	public static void BuildLabelledInputLabel( BuildInputLabelEvent eventArgs )
	{
		if ( eventArgs.Handled ) return;
		if ( eventArgs.Link?.Source.GetNiceLabel() is not { } label ) return;

		eventArgs.Text = label;
		eventArgs.Icon = "link";
		eventArgs.Handled = true;
	}
}

public class DefaultOutputEditor : DefaultEditor<PlugOut, ActionOutputPlug, Node.Output>
{
	public override Rect BoundingRect => base.BoundingRect.Grow( 0f, 0f, _labelWidth + 24f, 0f );

	private float _labelWidth;

	public DefaultOutputEditor( NodeUI node, PlugOut parent )
		: base( node, parent )
	{

	}

	protected override void OnPaint()
	{
		var label = ActionPlug.Label;

		Enabled = label is not null;

		var prevLabelWidth = _labelWidth;

		try
		{
			_labelWidth = 0f;

			if ( !Enabled )
				return;

			Paint.Antialiasing = true;
			Paint.TextAntialiasing = true;

			var text = label;
			var isLabeled = true;
			var textSize = Paint.MeasureText( text );

			var valueRect = new Rect( LocalRect.Right + 20f, LocalRect.Top, textSize.x + 20f, LocalRect.Height )
				.Shrink( 0f, 2f, 0f, 2f );

			if ( isLabeled )
			{
				valueRect = valueRect.Grow( 0f, 0f, 20f, 0f );
			}

			var handleConfig = PlugUI.HandleConfig;

			handleConfig.Color = Color.Lerp( handleConfig.Color, ColorTint.WithAlpha( 1f ), ColorTint.a );

			Paint.SetPen( handleConfig.Color, 4f * PulseScale );
			Paint.DrawLine( LocalRect.Center.WithX( LocalRect.Right + 12f ), LocalRect.Center.WithX( LocalRect.Right + 20f ) );

			PaintHelper.DrawValue( handleConfig, valueRect, text, PulseScale, "link" );

			_labelWidth = valueRect.Width;
		}
		finally
		{
			if ( Math.Abs( _labelWidth - prevLabelWidth ) > 0.01f )
			{
				PrepareGeometryChange();
			}
		}
	}
}
