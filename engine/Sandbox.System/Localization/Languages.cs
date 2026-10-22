namespace Sandbox.Localization;

/// <summary>
/// A list of supported languages and metadata surrounding them
/// </summary>
public static class Languages
{
	internal static List<LanguageInformation> All = new List<LanguageInformation>()
	{
		new LanguageInformation( "Arabic", "ar", rightToLeft: true ),
		new LanguageInformation( "Bulgarian", "bg" ),
		new LanguageInformation( "Simplified Chinese", "zh-cn" ),
		new LanguageInformation( "Traditional Chinese", "zh-tw" ),
		new LanguageInformation( "Czech", "cs" ),
		new LanguageInformation( "Danish", "da" ),
		new LanguageInformation( "Dutch", "nl" ),
		new LanguageInformation( "English", "en" ),
		new LanguageInformation( "Finnish", "fi" ),
		new LanguageInformation( "French", "fr" ),
		new LanguageInformation( "German", "de" ),
		new LanguageInformation( "Greek", "el" ),
		new LanguageInformation( "Hungarian", "hu" ),
		new LanguageInformation( "Italian", "it" ),
		new LanguageInformation( "Japanese", "ja" ),
		new LanguageInformation( "Korean", "ko" ),
		new LanguageInformation( "Norwegian", "no" ),
		new LanguageInformation( "Pirate", "en-pt", "en" ),
		new LanguageInformation( "Polish", "pl" ),
		new LanguageInformation( "Portuguese", "pt" ),
		new LanguageInformation( "Portuguese-Brazil", "pt-br", "pt" ),
		new LanguageInformation( "Romanian", "ro" ),
		new LanguageInformation( "Russian", "ru" ),
		new LanguageInformation( "Spanish-Spain", "es" ),
		new LanguageInformation( "Spanish-Latin America", "es-419", "es" ),
		new LanguageInformation( "Swedish", "sv" ),
		new LanguageInformation( "Thai", "th" ),
		new LanguageInformation( "Turkish", "tr" ),
		new LanguageInformation( "Ukrainian", "uk" ),
		new LanguageInformation( "Vietnamese", "vn" ),
	};

	/// <summary>
	/// Enumerate all languages, in no particular order
	/// </summary>
	public static IEnumerable<LanguageInformation> List => All;

	/// <summary>
	/// Find a language by shortname, or full name
	/// </summary>
	public static LanguageInformation Find( string key )
	{
		var found = All.FirstOrDefault( x => string.Equals( x.Abbreviation, key, StringComparison.OrdinalIgnoreCase ) );
		if ( found != null ) return found;

		found = All.FirstOrDefault( x => string.Equals( x.Title, key, StringComparison.OrdinalIgnoreCase ) );
		if ( found != null ) return found;

		return null;
	}
}

public class LanguageInformation
{
	/// <summary>
	/// Title of the localization language.
	/// </summary>
	public string Title { get; }

	/// <summary>
	/// ISO 639-1 code of the language, with optional ISO 3166-1 alpha-2 country specifiers. (for example "en-GB" for British English)
	/// </summary>
	public string Abbreviation { get; }

	/// <summary>
	/// If set, the <see cref="Abbreviation"/> of the parent language. For example, Pirate English is based on English.
	/// </summary>
	public string Parent { get; }

	/// <summary>
	/// Whether the language is typed right to left, such as the Arabic language.
	/// </summary>
	public bool RightToLeft { get; }

	public LanguageInformation( string title, string abbreviation, string parent = null, bool rightToLeft = false )
	{
		Title = title;
		Abbreviation = abbreviation;
		Parent = parent;
		RightToLeft = rightToLeft;
	}
}
