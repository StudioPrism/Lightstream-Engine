using System;
using System.Drawing;
using Editor.NodeEditor;
using Facepunch.ActionGraphs;
using Sandbox;

namespace Editor.ActionGraphs;

internal static class PaintHelper
{
	public static string FormatValue( Type type, object value, out float extraWidth, out object rawValue )
	{
		extraWidth = 0f;
		rawValue = value;

		if ( type == typeof( Signal ) )
		{
			rawValue = "Signal";
		}

		switch ( rawValue )
		{
			case null when !type.IsValueType:
				return "null";

			case string str:
				return $"\"{str}\"";

			case Resource resource:
				return resource.ResourcePath;

			case Color32 color32:
				rawValue = (Color)color32;
				extraWidth = 20f;

				return color32.a >= 255
					? color32.Hex
					: $"{(color32 with { a = 255 }).Hex}, {color32.a * 100f / 255f:F0}%";

			case Color color:
				extraWidth = 20f;

				return color.a >= 0.995f
					? color.WithAlpha( 1f ).Hex
					: $"{color.WithAlpha( 1f ).Hex}, {color.a * 100:F0}%";

			case float floatVal:
				return $"{floatVal:F2}";

			case double doubleVal:
				return $"{doubleVal:F2}";

			case Vector2 vec2:
				return $"x: {vec2.x:F2}, y: {vec2.y:F2}";

			case Vector3 vec3:
				return $"x: {vec3.x:F2}, y: {vec3.y:F2}, z: {vec3.z:F2}";

			case Vector4 vec4:
				return $"x: {vec4.x:F2}, y: {vec4.y:F2}, z: {vec4.z:F2}, w: {vec4.w:F2}";

			case Rotation rot:
				rawValue = (Angles)rot;
				return $"p: {rot.Pitch():F2}, y: {rot.Yaw():F2}, r: {rot.Roll():F2}";

			case Angles angles:
				return $"p: {angles.pitch:F2}, y: {angles.yaw:F2}, r: {angles.roll:F2}";

			default:
				return $"{rawValue}";
		}
	}

	public static void DrawValue( HandleConfig handleConfig, Rect valueRect, string text, float pulseScale = 1f, string icon = null, object rawValue = null )
	{
		var bg = Theme.ControlBackground;
		var fg = Theme.TextControl;

		var borderColor = handleConfig.Color.Desaturate( 0.2f ).Darken( 0.3f );

		if ( pulseScale > 1f )
		{
			bg = Color.Lerp( bg, borderColor, (pulseScale - 1f) * 0.25f );
		}

		Paint.SetPen( borderColor, 2f * (pulseScale * 0.5f + 0.5f) );
		Paint.SetBrush( bg );
		Paint.DrawRect( valueRect, 2 );

		if ( rawValue is Color color )
		{
			ColorPalette.PaintSwatch( color, new Rect( valueRect.Left + 3f, valueRect.Top + 3f, 14f, 14f ), false, radius: 2, disabled: false );
			valueRect = valueRect.Shrink( 14f, 0f, 0f, 0f );
		}

		Paint.SetPen( fg );

		if ( !string.IsNullOrEmpty( icon ) )
		{
			Paint.DrawIcon( new Rect( valueRect.Left + 8f, valueRect.Top, 16f, valueRect.Height ), icon, 16f );
			valueRect = valueRect.Shrink( 20f, 0f, 0f, 0f );
		}

		Paint.DrawText( valueRect, text, TextFlag.Center );
	}
}
