using System.Text.Json.Serialization;

namespace Sandbox.Internal.JsonConvert
{
	internal class RangedFloatConverter : JsonConverter<RangedFloat>
	{
		public override RangedFloat Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
		{
			if ( reader.TokenType == JsonTokenType.String )
			{
				return RangedFloat.Parse( reader.GetString() );
			}

			if ( reader.TokenType == JsonTokenType.Number )
			{
				return new RangedFloat( reader.GetSingle() );
			}

			if ( reader.TokenType == JsonTokenType.StartArray )
			{
				reader.Read();

				RangedFloat v = default;

				if ( reader.TokenType == JsonTokenType.Number )
				{
					v.Min = reader.GetSingle();
					reader.Read();
				}

				if ( reader.TokenType == JsonTokenType.Number )
				{
					v.Max = reader.GetSingle();
					reader.Read();
				}

				if ( reader.TokenType == JsonTokenType.Number )
				{
					v.Range = (RangedFloat.RangeType)(int)reader.GetSingle();
					reader.Read();
				}

				while ( reader.TokenType != JsonTokenType.EndArray )
				{
					reader.Read();
				}

				return v;
			}

			Log.Warning( $"RangedFloatFromJson - unable to read from {reader.TokenType}" );

			return default;
		}

		public override void Write( Utf8JsonWriter writer, RangedFloat val, JsonSerializerOptions options )
		{
			writer.WriteStringValue( val.ToString() );
		}
	}
}
