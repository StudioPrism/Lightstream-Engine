namespace Editor;

[CustomEditor( typeof( Rect ) )]
[CustomEditor( typeof( RectInt ) )]
public class RectControlWidget : ControlWidget
{
	SerializedObject obj;

	public RectControlWidget( SerializedProperty property ) : base( property )
	{
		property.TryGetAsObject( out obj );

		Layout = Layout.Row();
		Layout.Spacing = 2;

		TryAddField( "Left", Theme.Red, "X" );
		TryAddField( "Top", Theme.Blue, "Y" );
		TryAddField( "Width", Theme.Green, "W" );
		TryAddField( "Height", Theme.Yellow, "H" );
	}

	private void TryAddField( string propertyName, Color color, string label )
	{
		var prop = obj.GetProperty( propertyName );
		if ( prop is null ) return;

		var control = Layout.Add( new FloatControlWidget( prop ) { HighlightColor = color, Label = label } );
		control.MinimumWidth = Theme.RowHeight;
		control.HorizontalSizeMode = SizeMode.CanGrow | SizeMode.Expand;
		control.MakeRanged( SerializedProperty );
	}

	protected override void OnPaint()
	{
		// nothing
	}
}
