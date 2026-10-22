using Sandbox.Services;
using Sandbox.Utility;
using System;
using System.IO;
using System.Threading;

namespace Sandbox;

public static partial class SandboxToolExtensions
{
	/// <summary>
	/// Mark this package as a favourite
	/// </summary>
	public static async Task SetFavouriteAsync( this Package package, bool state )
	{
		var f = await Sandbox.Backend.Package.SetFavourite( package.FullIdent, state );
		if ( !f.Success ) return;

		var i = package.Interaction;
		i.Favourite = state;
		i.FavouriteCreated = DateTime.UtcNow;
		package.Interaction = i;

		package.Favourited = f.Total;
		EditorEvent.Run( "package.changed", package );
		EditorEvent.Run( "package.changed.favourite", package );
	}

	/// <summary>
	/// Add your vote for this package
	/// </summary>
	public static async Task SetVoteAsync( this Package package, bool up )
	{
		try
		{
			var r = await Sandbox.Backend.Package.SetRating( package.FullIdent, up ? 0 : 1 );
			if ( !r.Success ) return;

			var i = package.Interaction;
			i.Rating = up ? 0 : 1;
			i.RatingCreated = DateTime.UtcNow;
			package.Interaction = i;

			package.VotesUp = r.VotesUp;
			package.VotesDown = r.VotesDown;

			EditorEvent.Run( "package.changed", package );
			EditorEvent.Run( "package.changed.rating", package );
		}
		catch ( Refit.ApiException e )
		{
			Log.Warning( $"Couldn't rate {package.FullIdent} ({e.Message})" );
		}
	}

	/// <summary>
	/// Mark this package as a favourite
	/// </summary>
	public static async Task<bool> UploadFile( this Package package, string absolutePath, string relativePath, Sandbox.Utility.DataProgress.Callback progress, CancellationToken token = default )
	{
		var contents = await System.IO.File.ReadAllBytesAsync( absolutePath );
		return await UploadFile( package, contents, relativePath, progress, token );
	}

	/// <summary>
	/// Upload a file used by this package
	/// </summary>
	public static async Task<bool> UploadFile( this Package package, byte[] contents, string relativePath, Sandbox.Utility.DataProgress.Callback progress, CancellationToken token = default )
	{
		long crc = (long)Crc64.FromBytes( contents );
		int size = contents.Length;

		var hashedName = $"{Guid.NewGuid()}";

		var endpoint = await AccountInformation.GetUploadEndPointAsync( hashedName );

		using ( var stream = new MemoryStream( contents ) )
		{
			var upload = new FileUploadRequest();

			upload.Package = package.FullIdent;
			upload.Path = relativePath;
			upload.Crc = crc.ToString( "x" );
			upload.Size = size;

			if ( size > 1024 * 1024 * 2 )
			{
				Log.Trace( $"Uploading using container: {relativePath}" );

				if ( !await EditorUtility.PutAsync( stream, endpoint, progress, token ) )
				{
					Log.Warning( $"File upload failed" );
					return false;
				}

				upload.Blob = hashedName;

				// make the progress report additive to this size, so it doesn't go back to 0 bytes
				var op = progress;
				progress = p =>
				{
					op?.Invoke( new DataProgress { TotalBytes = size + p.TotalBytes, ProgressBytes = size + p.ProgressBytes } );
				};
			}
			else
			{
				Log.Trace( $"Uploading Direct: {relativePath}" );
				upload.Contents = Convert.ToBase64String( contents );
			}

			try
			{
				var r = await Backend.Package.UploadFile( upload );

				if ( r.Status != "OK" )
				{
					Log.Warning( $"Uploading '{relativePath}' failed: {r}" );
				}

				return r.Status == "OK";
			}
			catch ( System.Exception e )
			{
				Log.Warning( e, $"Uploading '{relativePath}' failed: {e.Message}" );
				return false;
			}
		}
	}

	/// <summary>
	/// Upload a video for this package
	/// </summary>
	public static async Task<bool> UploadVideo( this Asset asset, byte[] contents, bool isThumbVideo, bool hidden = false, string tag = null, Sandbox.Utility.DataProgress.Callback progress = null, CancellationToken token = default )
	{
		var meta = asset.Publishing;
		if ( meta == null ) return false;
		if ( meta.ProjectConfig == null ) return false;
		if ( meta.ProjectConfig.FullIdent == null ) return false;

		int size = contents.Length;
		var file = Guid.NewGuid().ToString();
		var endpoint = await AccountInformation.GetUploadEndPointAsync( file );

		using ( var stream = new MemoryStream( contents ) )
		{
			var upload = new VideoUploadRequest();

			upload.Package = meta.ProjectConfig.FullIdent;
			upload.Thumb = isThumbVideo;
			upload.Hidden = hidden;
			upload.Tag = tag;

			if ( size > 1024 * 1024 * 2 )
			{
				if ( !await EditorUtility.PutAsync( stream, endpoint, progress, token ) )
				{
					Log.Warning( $"File upload failed" );
					return false;
				}

				upload.File = file;

				// make the progress report additive to this size, so it doesn't go back to 0 bytes
				var op = progress;
				progress = p =>
				{
					op?.Invoke( new DataProgress { TotalBytes = size + p.TotalBytes, ProgressBytes = size + p.ProgressBytes } );
				};
			}
			else
			{
				upload.File = Convert.ToBase64String( contents );
			}

			try
			{
				var r = await Backend.Package.UploadVideo( upload );

				if ( r.Status != "OK" )
				{
					Log.Warning( $"Uploading video failed: {r}" );
				}

				return r.Status == "OK";
			}
			catch ( System.Exception e )
			{
				Log.Warning( e, $"Uploading video failed: {e.Message}" );
				return false;
			}
		}
	}

	static Dictionary<string, CancellationTokenSource> _updateDebounce = new();

	/// <summary>
	/// Update a value on this package
	/// </summary>
	public static async Task UpdateValue( this Package package, string key, string value, CancellationToken token = default )
	{
		if ( !package.IsRemote )
			return;

		// TODO - local CAN EDIT

		if ( _updateDebounce.TryGetValue( key, out var tcs ) )
		{
			tcs?.Cancel();
			tcs?.Dispose();
		}

		tcs = new CancellationTokenSource();
		_updateDebounce[key] = tcs;

		await Task.Delay( 1000 );

		if ( tcs.IsCancellationRequested || token.IsCancellationRequested )
			return;

		var p = await Sandbox.Backend.Package.Update( package.FullIdent, key, value );

		if ( package is RemotePackage remote )
		{
			remote.UpdateFromDto( p );
			Package.Cache( package, false );
		}
	}
}
