using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Sandbox;

//
// This process is fragile and stupid.
// For release we'll need to make this less likely to get stuck etc
// Ideally we'd open another socket for bigger file transfers, maybe use http routed though Steam networking somehow.
//

internal class FileRecv
{
	public string Filename { get; set; }
	public ulong Crc { get; set; }
	public uint TotalSize { get; set; }
	public bool IsComplete { get; set; }
	public int Id { get; set; }
	public DateTime Start { get; set; }

	System.IO.Stream Stream;

	public long DownloadedSize { get; private set; } = 0;

	internal void OnChunk( Span<byte> span )
	{
		if ( Filename == null )
			throw new System.Exception( "Getting File Chunk but no name!" );

		if ( Stream == null )
		{
			var targetName = DownloadedAssets.GetCacheName( Filename, Crc );
			Stream = EngineFileSystem.DownloadedFiles.OpenWrite( targetName );

			Log.Info( $"Downloading \"{Filename}\"" );
		}

		DownloadedSize += span.Length;
		Stream.Write( span );

		//(IMenuAddon.Current?.GetLoadingPanel() as ILoadingProgress)?.OnLoadProgress( $"Downloading \"{Filename}\"\n{Stream.Length.FormatBytes()} of {Size.FormatBytes()}" );

		if ( DownloadedSize == TotalSize )
		{
			Stream.Dispose();
			DownloadedAssets.AddFileAsync( Filename, DownloadedAssets.GetCacheName( Filename, Crc ) ).Wait();
			IsComplete = true;
		}
	}
}
