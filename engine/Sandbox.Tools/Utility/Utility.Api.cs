namespace Editor;

public static partial class EditorUtility
{
	/// <summary>
	/// Translate input into language
	/// </summary>
	public static async Task<string> TranslateString( string input, string language )
	{
		try
		{
			string jsonString = await Backend.Utility.Translate( language, input );

			return Json.Deserialize<string>( jsonString );
		}
		catch
		{
			// ignore api errors
			return default;
		}
	}
}
