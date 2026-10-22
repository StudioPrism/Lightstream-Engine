namespace Editor;

[CustomEditor( typeof( Transform ) )]
public class TransformControlWidget : ControlWidget
{
	SerializedObject obj;

	public override bool SupportsMultiEdit => true;

	public TransformControlWidget( SerializedProperty property ) : base( property )
	{
		property.TryGetAsObject( out obj );

		Layout = Layout.Column();
		Layout.Spacing = 2;

		// Should we seperate these out somehow?
		//Layout.Margin = new Sandbox.UI.Margin( 0, 2 );

		// Position
		{
			Layout.Add( Create( obj.GetProperty( "Position" ) ) );
		}

		// Rotation
		{
			Layout.Add( Create( obj.GetProperty( "Rotation" ) ) );
		}

		// Scale
		{
			Layout.Add( Create( obj.GetProperty( "Scale" ) ) );
		}
	}

	protected override void OnPaint()
	{
		//	Paint.ClearPen();
		//	Paint.SetBrush( Color.Lerp( ControlColor.Lighten( 1.5f ), Theme.Blue, 0.10f ) );
		//	Paint.DrawRect( LocalRect, ControlRadius );
	}
}
