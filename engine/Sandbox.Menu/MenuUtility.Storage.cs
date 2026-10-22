using System;
using System.IO;

namespace Sandbox;

public static partial class MenuUtility
{
	/// <summary>
	/// Allows to menu addon to interact with the downloaded file cache
	/// </summary>
	public static class Storage
	{
		public struct FileEntry
		{
			public string Filename { get; set; }
			public long Size { get; set; }
			public DateTime Created { get; set; }
			public DateTime LastAccessed { get; set; }
		}

		/// <summary>
		/// Get a list of all the local cache files (download/)
		/// </summary>
		public static IEnumerable<FileEntry> GetStorageFiles()
		{
			var path = EngineFileSystem.DownloadedFiles.GetFullPath( "/" );

			foreach ( var file in Directory.EnumerateFiles( path, "*", SearchOption.AllDirectories ) )
			{
				var f = new FileEntry();

				try
				{
					var info = new FileInfo( file );

					f.Filename = info.FullName;
					f.Size = info.Length;
					f.Created = info.CreationTime;
					f.LastAccessed = info.LastAccessTime;
				}
				catch ( FileNotFoundException )
				{
					continue;
				}

				yield return f;
			}
		}

		/// <summary>
		/// Delete all files that haven't been used since x date.
		/// </summary>
		public static async Task FlushAsync( DateTime beforeDate )
		{
			var path = EngineFileSystem.DownloadedFiles.GetFullPath( "/" );

			//
			// Run the guts of the logic in a thread to avoid hitching
			//
			await Task.Run( () =>
			{
				foreach ( var file in Directory.EnumerateFiles( path, "*", SearchOption.AllDirectories ) )
				{
					var info = new FileInfo( file );

					if ( info.LastAccessTime < beforeDate )
					{
						try
						{
							File.Delete( file );
						}
						catch ( Exception e )
						{
							Log.Error( e, $"Failed to delete file {file}" );
						}

					}
				}
			} );
		}
	}
}
