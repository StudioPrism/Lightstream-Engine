namespace Editor;

[CustomEditor( typeof( Sphere ) )]
public class SphereControlWidget : ControlWidget
{
	SerializedObject obj;

	public SphereControlWidget( SerializedProperty property ) : base( property )
	{
		property.TryGetAsObject( out obj );

		PaintBackground = false;
		Layout = Layout.Row();
		Layout.Spacing = 2;

		var radius = Layout.Add( Create( obj.GetProperty( "Radius" ) ) );
		var center = Layout.Add( Create( obj.GetProperty( "Center" ) ) );

		if ( radius is FloatControlWidget radiusWidget )
		{
			radiusWidget.MaximumWidth = 80;
			radiusWidget.Icon = "radio_button_checked";
			radiusWidget.Label = null;
			radiusWidget.MakeRanged( SerializedProperty );
		}

		foreach ( var child in center.Children )
		{
			if ( child is FloatControlWidget floatWidget )
			{
				floatWidget.MakeRanged( SerializedProperty );
			}
		}

	}
}
