namespace Sandbox.UI;

public class CopyEvent : PanelEvent
{
	internal CopyEvent() : base( "copy" )
	{

	}
}

public class CutEvent : PanelEvent
{
	internal CutEvent() : base( "cut" )
	{

	}
}

public class PasteEvent : PanelEvent
{
	public string ClipboardValue { get; set; }

	internal PasteEvent( string value ) : base( "copy" )
	{
		ClipboardValue = value;
	}
}

public class EscapeEvent : PanelEvent
{

	internal EscapeEvent() : base( "escape" )
	{

	}
}
