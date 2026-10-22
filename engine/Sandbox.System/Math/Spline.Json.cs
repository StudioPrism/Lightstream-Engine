using System.Text.Json.Serialization;

namespace Sandbox.Internal.JsonConvert
{
	/// <summary>
	/// We use a custom converter for <see cref="Spline.Point"/> to allow for more compact serialization.
	/// For example we ommit default values for a lot of properties.
	/// </summary>
	internal class SplinePointConverter : JsonConverter<Spline.Point>
	{
		public override Spline.Point Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
		{
			if ( reader.TokenType == JsonTokenType.StartObject )
			{
				reader.Read();

				Spline.Point point = new();
				while ( reader.TokenType != JsonTokenType.EndObject )
				{
					if ( reader.TokenType == JsonTokenType.PropertyName )
					{
						var name = reader.GetString();
						reader.Read();

						if ( name == "Pos" )
						{
							point.Position = JsonSerializer.Deserialize<Vector3>( ref reader, options );
						}

						if ( name == "In" )
						{
							point.In = JsonSerializer.Deserialize<Vector3>( ref reader, options );
						}

						if ( name == "Out" )
						{
							point.Out = JsonSerializer.Deserialize<Vector3>( ref reader, options );
						}

						if ( name == "Mode" )
						{
							point.Mode = JsonSerializer.Deserialize<Spline.HandleMode>( ref reader, options );
						}

						if ( name == "Roll" )
						{
							point.Roll = reader.GetSingle();
							reader.Read();
						}

						if ( name == "Scale" )
						{
							point.Scale = JsonSerializer.Deserialize<Vector3>( ref reader, options );
						}

						if ( name == "Up" )
						{
							point.Up = JsonSerializer.Deserialize<Vector3>( ref reader, options );
						}

						continue;

					}

					reader.Read();
				}

				return point;
			}

			return default;
		}

		public override void Write( Utf8JsonWriter writer, Spline.Point val, JsonSerializerOptions options )
		{
			writer.WriteStartObject();

			writer.WritePropertyName( "Pos" );
			JsonSerializer.Serialize( writer, val.Position, options );

			writer.WritePropertyName( "In" );
			JsonSerializer.Serialize( writer, val.In, options );

			writer.WritePropertyName( "Out" );
			JsonSerializer.Serialize( writer, val.Out, options );

			if ( val.Mode != Spline.HandleMode.Auto )
			{
				writer.WritePropertyName( "Mode" );
				JsonSerializer.Serialize( writer, val.Mode, options );
			}

			if ( val.Roll != 0 )
			{
				writer.WritePropertyName( "Roll" );
				writer.WriteNumberValue( val.Roll );
			}

			if ( val.Scale != Vector3.One )
			{
				writer.WritePropertyName( "Scale" );
				JsonSerializer.Serialize( writer, val.Scale, options );
			}

			if ( val.Up != Vector3.Up )
			{
				writer.WritePropertyName( "Up" );
				JsonSerializer.Serialize( writer, val.Up, options );
			}

			writer.WriteEndObject();
		}

	}
}
