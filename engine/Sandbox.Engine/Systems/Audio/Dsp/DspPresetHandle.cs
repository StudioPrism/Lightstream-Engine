using System.Text.Json;

namespace Sandbox.Audio;

/// <summary>
/// A handle to a DspPreset
/// </summary>
public struct DspPresetHandle : IEquatable<DspPresetHandle>, IJsonConvert
{
	public string Name { get; set; }

	public override bool Equals( object obj ) => obj is DspPresetHandle handle && Equals( handle );
	public bool Equals( DspPresetHandle other ) => string.Equals( Name, other.Name, StringComparison.OrdinalIgnoreCase );
	public override int GetHashCode() => HashCode.Combine( Name );
	public override string ToString() => Name;
	public static bool operator ==( DspPresetHandle left, DspPresetHandle right ) => left.Equals( right );
	public static bool operator !=( DspPresetHandle left, DspPresetHandle right ) => !(left == right);
	public static implicit operator string( DspPresetHandle o ) => o.Name.ToLowerInvariant();
	public static implicit operator DspPresetHandle( string v ) => new DspPresetHandle { Name = v.ToLowerInvariant() };

	public static object JsonRead( ref Utf8JsonReader reader, Type typeToConvert )
	{
		if ( reader.TokenType == JsonTokenType.String )
		{
			return (DspPresetHandle)(reader.GetString());
		}

		return (DspPresetHandle)"unknown";
	}

	public static void JsonWrite( object value, Utf8JsonWriter writer )
	{
		DspPresetHandle target = (DspPresetHandle)value;
		writer.WriteStringValue( target.Name?.ToLowerInvariant() ?? "unknown" );
	}

	public static object[] GetDropdownSelection() => Sound.DspNames;
}
