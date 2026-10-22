using System.Buffers;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;

namespace Sandbox.Utility;

/// <summary>
/// Provides progress information for operations that process blocks of data,
/// such as file uploads, downloads, or large data transfers.
/// </summary>
public struct DataProgress
{
	/// <summary>
	/// The number of bytes processed so far.
	/// </summary>
	public long ProgressBytes { get; set; }

	/// <summary>
	/// The total number of bytes to process.
	/// </summary>
	public long TotalBytes { get; set; }

	/// <summary>
	/// The number of bytes processed since the last progress update.
	/// </summary>
	public long DeltaBytes { get; set; }

	/// <summary>
	/// Progress as a fraction from 0.0 to 1.0.
	/// </summary>
	public float ProgressDelta => (float)((double)ProgressBytes / (double)TotalBytes).Clamp( 0.0, 1.0 );

	/// <summary>
	/// Callback delegate for receiving progress updates.
	/// </summary>
	public delegate void Callback( DataProgress progress );

	/// <summary>
	/// HTTP content wrapper that reports upload progress through DataProgress callbacks.
	/// Used internally for tracking file upload progress.
	/// </summary>
	internal class HttpContentStream : HttpContent
	{
		private readonly Stream stream;
		private readonly int bufferSize;
		private readonly long length;

		/// <summary>
		/// Callback invoked as data is being uploaded.
		/// </summary>
		public Callback Progress { get; set; }

		/// <summary>
		/// Creates a new HTTP content stream with progress tracking.
		/// </summary>
		/// <param name="stream">The source stream to upload</param>
		/// <param name="bufferSize">Size of buffer for chunked uploads (default 256KB)</param>
		public HttpContentStream( Stream stream, int bufferSize = 1024 * 256 )
		{
			this.stream = stream;
			this.bufferSize = bufferSize;
			this.length = stream.Length;
		}

		protected override async Task SerializeToStreamAsync( Stream stream, TransportContext context )
		{
			var buffer = new byte[bufferSize];
			int bytesRead;
			long bytesTransferred = 0;

			while ( (bytesRead = await this.stream.ReadAsync( buffer, 0, buffer.Length )) > 0 )
			{
				await stream.WriteAsync( buffer, 0, bytesRead );
				bytesTransferred += bytesRead;
				Progress?.Invoke( new DataProgress { TotalBytes = length, ProgressBytes = bytesTransferred, DeltaBytes = bytesRead } );
			}
		}

		protected override bool TryComputeLength( out long length )
		{
			length = this.length;
			return true;
		}
	}
}
