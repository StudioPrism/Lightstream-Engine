using System.Text.Json.Serialization;
namespace Sandbox;

[Obsolete]
[Expose]
public struct ParticleControlPoint
{
	public ControlPointValueInput Value { readonly get; set; }

	public string StringCP { readonly get; set; }

	[JsonInclude]
	public Vector3 VectorValue { readonly get; set; }

	[JsonInclude]
	public float FloatValue { readonly get; set; }

	[JsonInclude]
	public Color ColorValue { readonly get; set; }

	[JsonInclude]
	public GameObject GameObjectValue { readonly get; set; }

	public readonly object OutputValue()
	{
		switch ( Value )
		{
			case ControlPointValueInput.Vector3:
				return VectorValue;

			case ControlPointValueInput.Float:
				return new Vector3( FloatValue, 0, 0 );

			case ControlPointValueInput.Color:
				return new Vector3( ColorValue.r, ColorValue.g, ColorValue.b );

			case ControlPointValueInput.GameObject:
				return GameObjectValue.IsValid() ? GameObjectValue.WorldTransform : Transform.Zero;
			default:
				return Vector3.Zero;
		}
	}

	[Obsolete]
	public enum ControlPointValueInput
	{
		GameObject,
		Vector3,
		Float,
		Color

	}
}
