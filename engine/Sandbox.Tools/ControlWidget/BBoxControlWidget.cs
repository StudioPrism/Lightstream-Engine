namespace Editor;

[CustomEditor( typeof( BBox ) )]
public class BBoxControlWidget : ControlWidget
{
	SerializedObject obj;

	public BBoxControlWidget( SerializedProperty property ) : base( property )
	{
		property.TryGetAsObject( out obj );

		PaintBackground = false;
		Layout = Layout.Column();
		Layout.Spacing = 2;

		var mins = Layout.Add( Create( obj.GetProperty( "Mins" ) ) );
		var maxs = Layout.Add( Create( obj.GetProperty( "Maxs" ) ) );

		var children = new List<Widget>();
		children.AddRange( mins.Children );
		children.AddRange( maxs.Children );

		foreach ( var child in children )
		{
			if ( child is FloatControlWidget floatWidget )
			{
				floatWidget.MakeRanged( property );
			}
		}
	}
}
