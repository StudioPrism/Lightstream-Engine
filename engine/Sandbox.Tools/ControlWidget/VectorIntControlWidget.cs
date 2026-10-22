namespace Editor;

[CustomEditor( typeof( Vector2Int ) )]
[CustomEditor( typeof( Vector3Int ) )]
public class VectorIntControlWidget : ControlWidget
{
	SerializedObject Target;
	SerializedProperty Property;
	IntegerControlWidget FirstControl;

	public override bool SupportsMultiEdit => true;

	public VectorIntControlWidget( SerializedProperty property ) : base( property )
	{
		Property = property;
		property.TryGetAsObject( out Target );
		if ( Target is null )
		{
			Log.Warning( $"Error when trying to get {property} as object" );
			return;
		}

		if ( Target is null )
			return;

		Layout = Layout.Row();
		Layout.Spacing = 2;

		FirstControl = TryAddField( "x", Theme.Red, "X" );
		TryAddField( "y", Theme.Green, "Y" );
		TryAddField( "z", Theme.Blue, "Z" );
		TryAddField( "w", Theme.Yellow, "W" );
	}

	private IntegerControlWidget TryAddField( string propertyName, Color color, string text )
	{
		var prop = Target.GetProperty( propertyName );
		if ( prop is null ) return null;

		var control = Layout.Add( new IntegerControlWidget( prop ) { HighlightColor = color, Label = text } );

		control.MinimumWidth = Theme.RowHeight;
		control.HorizontalSizeMode = SizeMode.CanGrow | SizeMode.Expand;
		control.MakeRanged( Property );

		return control;
	}

	public override void StartEditing()
	{
		FirstControl?.StartEditing();
	}

	protected override void OnPaint()
	{
		// nothing
	}

}
