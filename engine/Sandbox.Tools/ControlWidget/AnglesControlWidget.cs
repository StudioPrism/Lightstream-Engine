namespace Editor;

[CustomEditor( typeof( Angles ) )]
public class AnglesControlWidget : ControlWidget
{
	public override bool SupportsMultiEdit => true;

	public AnglesControlWidget( SerializedProperty property ) : base( property )
	{
		property.TryGetAsObject( out var obj );

		Layout = Layout.Row();
		Layout.Spacing = 2;

		TryAddField( obj, "pitch", Theme.Red, "height", "Pitch" );
		TryAddField( obj, "yaw", Theme.Green, "360", "Yaw" );
		TryAddField( obj, "roll", Theme.Blue, "sync", "Roll" );
	}

	private void TryAddField( SerializedObject obj, string propertyName, Color color, string icon, string tooltip )
	{
		var prop = obj.GetProperty( propertyName );
		if ( prop is null ) return;

		var control = Layout.Add( new FloatControlWidget( prop ) { HighlightColor = color, Icon = icon, Label = null, ToolTip = tooltip } );
		control.MinimumWidth = Theme.RowHeight;
		control.HorizontalSizeMode = SizeMode.CanGrow | SizeMode.Expand;
		control.MakeRanged( SerializedProperty );
	}

	protected override void OnPaint()
	{
		// nothing
	}
}
