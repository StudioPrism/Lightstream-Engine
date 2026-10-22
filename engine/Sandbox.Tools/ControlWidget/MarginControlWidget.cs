using Sandbox.UI;

namespace Editor;

[CustomEditor( typeof( Margin ) )]
public class MarginControlWidget : ControlWidget
{
	SerializedObject obj;

	public MarginControlWidget( SerializedProperty property ) : base( property )
	{
		property.TryGetAsObject( out obj );

		Layout = Layout.Row();
		Layout.Spacing = 2;

		TryAddField( "Left", Theme.Red, "border_left" );
		TryAddField( "Top", Theme.Blue, "border_top" );
		TryAddField( "Right", Theme.Green, "border_right" );
		TryAddField( "Bottom", Theme.Yellow, "border_bottom" );
	}

	private void TryAddField( string propertyName, Color color, string icon )
	{
		var prop = obj.GetProperty( propertyName );
		if ( prop is null ) return;

		var control = Layout.Add( new FloatControlWidget( prop ) { HighlightColor = color, Label = null, Icon = icon } );
		control.MinimumWidth = Theme.RowHeight;
		control.HorizontalSizeMode = SizeMode.CanGrow | SizeMode.Expand;
		control.MakeRanged( SerializedProperty );
	}

	protected override void OnPaint()
	{
		// nothing
	}
}
