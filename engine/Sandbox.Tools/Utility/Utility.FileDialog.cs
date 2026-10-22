using System.IO;
namespace Editor;

public static partial class EditorUtility
{
	/// <summary>
	/// Open a file save dialog. Returns null on cancel, else the absolute path of the target file.
	/// </summary>
	public static string SaveFileDialog( string title, string extension, string defaultPath )
	{
		extension = extension.Trim( '.' );

		var path = defaultPath;
		if ( path.Contains( "." ) ) path = Path.GetDirectoryName( path );

		var fd = new FileDialog( null );
		fd.Title = title;
		fd.Directory = path;
		fd.DefaultSuffix = $".{extension}";
		fd.SelectFile( Path.GetFileName( defaultPath ) );
		fd.SetFindFile();
		fd.SetModeSave();
		fd.SetNameFilter( $"{extension} (*.{extension})" );

		if ( !fd.Execute() )
			return null;

		return fd.SelectedFile;
	}

	/// <summary>
	/// Open a file open dialog. Returns null on cancel, else the absolute path of the target file.
	/// </summary>
	public static string OpenFileDialog( string title, string extension, string defaultPath )
	{
		extension = extension.Trim( '.' );

		var fd = new FileDialog( null );
		fd.Title = title;
		fd.Directory = Path.GetDirectoryName( defaultPath );
		fd.DefaultSuffix = $".{extension}";
		fd.SelectFile( Path.GetFileName( defaultPath ) );
		fd.SetFindFile();
		fd.SetModeOpen();
		fd.SetNameFilter( $"{extension} (*.{extension})" );

		if ( !fd.Execute() )
			return null;

		return fd.SelectedFile;
	}
}
