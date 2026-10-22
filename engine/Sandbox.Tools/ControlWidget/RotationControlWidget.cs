namespace Editor;

//
// This works a bit differently to most controls.
// We really want to edit an Angle instead of a Rotation, so 
// we store an _angle version on this control and access that.
// We watch the main rotation and update our local angle if
// that rotation changes.
// We are hoping to avoid converting a Rotation to an Angle as much
// as possible, because it can get a bit random looking. That isn't
// something you want when editing.
// I guess this is why everyone uses Angles instead.
//

[CustomEditor( typeof( Rotation ) )]
public class RotationControlWidget : ControlWidget
{
	Rotation _rotation;
	Angles _angles;

	Angles angles
	{
		get => _angles;

		set
		{
			if ( _angles == value ) return;

			_angles = value;
			_rotation = value.ToRotation();
			SerializedProperty.SetValue( _rotation );
		}
	}

	public override bool SupportsMultiEdit => true;

	public RotationControlWidget( SerializedProperty property ) : base( property )
	{
		Layout = Layout.Column();
		Layout.Spacing = 2;

		Think();

		// Create a serialized version of this widget, and bounce through 'angles'
		{
			// would be good to be able to do SerializedProperty.Create( getterfunc, setterfunc )

			var thisObj = this.GetSerialized();
			var prop = thisObj.GetProperty( nameof( angles ) );
			var editor = Create( prop );
			foreach ( var child in editor.Children )
			{
				if ( child is FloatControlWidget floatControl )
				{
					floatControl.MakeRanged( property );
				}
			}
			editor.HorizontalSizeMode = SizeMode.Flexible;
			Layout.Add( editor );
			thisObj.OnPropertyStartEdit += ( _ ) => property.NoteStartEdit( property );
			thisObj.OnPropertyFinishEdit += ( _ ) => property.NoteFinishEdit( property );
		}
	}

	protected override void OnPaint()
	{
		// nothing
	}

	public override void Think()
	{
		// rotation has changed externally, update our values
		var rot = SerializedProperty.GetValue<Rotation>();
		if ( _rotation != rot )
		{
			_rotation = rot;
			_angles = rot.Angles();
		}

		base.Think();
	}
}
