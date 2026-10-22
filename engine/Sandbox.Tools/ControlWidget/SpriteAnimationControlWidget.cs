using System;

namespace Editor;

[CustomEditor( typeof( string ), NamedEditor = "sprite_animation_name" )]
public class SpriteAnimationControlWidget : ControlWidget
{
	Sprite Sprite
	{
		get
		{
			var spriteProperty = SerializedProperty.Parent.GetProperty( nameof( SpriteRenderer.Sprite ) );
			if ( spriteProperty is null )
				return null;

			return spriteProperty.GetValue<Sprite>( null );
		}
	}

	public SpriteAnimationControlWidget( SerializedProperty property ) : base( property )
	{
		Layout = Layout.Column();
		Layout.Spacing = 2;
		AcceptDrops = false;

		Rebuild();
	}

	void Rebuild()
	{
		Layout.Clear( true );

		var sprite = Sprite;
		if ( sprite is null ) return;

		if ( sprite.Animations.Count <= 0 )
		{
			// No animations on this sprite
			Layout.Add( new Label( "None" ) );
			return;
		}

		var comboBox = new ComboBox( this );
		var v = SerializedProperty.GetValue<string>();

		for ( int i = 0; i < sprite.Animations.Count; ++i )
		{
			var name = sprite.Animations[i].Name;
			comboBox.AddItem( name, onSelected: () => SerializedProperty.SetValue( name ), selected: string.Equals( v, name, StringComparison.OrdinalIgnoreCase ) );
		}

		Layout.Add( comboBox );
	}

	protected override int ValueHash
	{
		get
		{
			var hc = new HashCode();
			var sprite = Sprite;
			hc.Add( base.ValueHash );
			hc.Add( sprite );

			if ( sprite is not null )
			{
				var animCount = sprite.Animations?.Count ?? 0;
				for ( int i = 0; i < animCount; i++ )
				{
					hc.Add( Sprite.Animations[i].Name );
				}
			}

			return hc.ToHashCode();
		}
	}

	protected override void OnValueChanged()
	{
		Rebuild();
	}

	protected override void OnPaint()
	{
		// Do nothing
	}
}
