using System.Text.Json;

namespace Sandbox.Audio;

/// <summary>
/// A handle to a Mixer
/// </summary>
public struct MixerHandle : IEquatable<MixerHandle>, IJsonConvert
{
	public string Name { get; set; }
	public Guid Id { get; set; }

	public override bool Equals( object obj ) => obj is MixerHandle handle && Equals( handle );
	public bool Equals( MixerHandle other ) => string.Equals( Name, other.Name, StringComparison.OrdinalIgnoreCase );
	public override int GetHashCode() => HashCode.Combine( Name );
	public override string ToString() => Name;
	public static bool operator ==( MixerHandle left, MixerHandle right ) => left.Equals( right );
	public static bool operator !=( MixerHandle left, MixerHandle right ) => !(left == right);
	public static implicit operator string( MixerHandle o ) => o.Name.ToLowerInvariant();

	public static implicit operator MixerHandle( string v )
	{
		var m = new MixerHandle { Name = v.ToLowerInvariant() };
		if ( m.Get() is Mixer mixer )
		{
			m.Id = mixer.Id;
		}

		return m;
	}

	public static implicit operator MixerHandle( Guid guid )
	{
		var m = new MixerHandle { Id = guid };
		if ( m.Get() is Mixer mixer )
		{
			m.Id = mixer.Id;
		}

		return m;
	}

	public static implicit operator MixerHandle( Mixer mixer )
	{
		return new MixerHandle { Id = mixer.Id, Name = mixer.Name };
	}

	public static object JsonRead( ref Utf8JsonReader reader, Type typeToConvert )
	{
		if ( reader.TokenType == JsonTokenType.String )
		{
			return (MixerHandle)(reader.GetString());
		}

		if ( reader.TokenType == JsonTokenType.StartObject )
		{
			reader.Read();

			MixerHandle mh = default;
			while ( reader.TokenType != JsonTokenType.EndObject )
			{
				if ( reader.TokenType == JsonTokenType.PropertyName )
				{
					var name = reader.GetString();
					reader.Read();

					if ( name == "Id" )
					{
						mh.Id = reader.GetGuid();
						reader.Read();
						continue;
					}

					if ( name == "Name" )
					{
						mh.Name = reader.GetString();
						reader.Read();
						continue;
					}

					reader.Read();
					continue;

				}

				reader.Read();
			}

			return mh;
		}

		return (MixerHandle)"unknown";
	}

	public static void JsonWrite( object value, Utf8JsonWriter writer )
	{
		MixerHandle target = (MixerHandle)value;

		// we write this amount of stuff to help with lookups later
		writer.WriteStartObject();
		writer.WriteString( "Name", target.Name?.ToLowerInvariant() ?? "unknown" );
		writer.WriteString( "Id", target.Id );
		writer.WriteEndObject();
	}

	public static object[] GetDropdownSelection() => Sound.DspNames;

	public readonly Mixer Get( Mixer fallback = null )
	{
		Mixer mixer = Mixer.FindMixerByGuid( Id );
		if ( mixer is not null )
			return mixer;

		mixer = Mixer.FindMixerByName( Name );
		if ( mixer is not null )
			return mixer;

		return fallback;
	}
	public readonly Mixer GetOrDefault() => Get( Mixer.Default ?? Mixer.Master );
}
