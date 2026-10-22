using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
	/// <summary>
	/// Wraps a stream containing a binary that has passed access control.
	/// </summary>
	public sealed class TrustedBinaryStream : Stream
	{
		private readonly Stream _baseStream;

		/// <summary>
		/// This should only be used by access control!
		/// </summary>
		internal static TrustedBinaryStream CreateInternal( byte[] baseStream )
		{
			return new TrustedBinaryStream( new MemoryStream( baseStream ) );
		}

		private TrustedBinaryStream( Stream baseStream )
		{
			_baseStream = baseStream;
		}

		public override void Flush()
		{
			_baseStream.Flush();
		}

		public override int Read( byte[] buffer, int offset, int count )
		{
			return _baseStream.Read( buffer, offset, count );
		}

		public override long Seek( long offset, SeekOrigin origin )
		{
			return _baseStream.Seek( offset, origin );
		}

		public override void SetLength( long value )
		{
			throw new NotImplementedException();
		}

		public override void Write( byte[] buffer, int offset, int count )
		{
			throw new NotImplementedException();
		}

		public override bool CanRead => _baseStream.CanRead;

		public override bool CanSeek => _baseStream.CanSeek;

		public override bool CanWrite => false;

		public override long Length => _baseStream.Length;

		public override long Position
		{
			get => _baseStream.Position;
			set => _baseStream.Position = value;
		}
	}
}
