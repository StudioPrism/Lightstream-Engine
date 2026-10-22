using System.Buffers;
using System.IO;
using System.Threading.Tasks;

namespace Sandbox.Utility;

/// <summary>
/// Generates 32-bit <a href="https://en.wikipedia.org/wiki/Cyclic_redundancy_check">Cyclic Redundancy Check</a> (CRC32) checksums.
/// Used for data integrity verification and fast hashing.
/// </summary>
public static class Crc32
{
	// IEEE 802.3 polynomial (reversed)
	private const uint generator = 0xEDB88320;

	private static uint[] checksumTable;

	/// <summary>
	/// Gets the lazily-initialized CRC32 lookup table for fast computation.
	/// </summary>
	private static uint[] ChecksumTable
	{
		get
		{
			if ( checksumTable != null )
				return checksumTable;

			checksumTable = Enumerable.Range( 0, 256 ).Select( i =>
			{
				var tableEntry = (uint)i;
				for ( var j = 0; j < 8; ++j )
				{
					tableEntry = ((tableEntry & 1) != 0)
						? (generator ^ (tableEntry >> 1))
						: (tableEntry >> 1);
				}
				return tableEntry;
			} ).ToArray();

			return checksumTable;
		}
	}

	/// <summary>
	/// Generates a CRC32 checksum from a byte stream.
	/// </summary>
	/// <param name="byteStream">The input to generate a checksum for.</param>
	/// <returns>The generated CRC32.</returns>
	public static uint FromBytes( IEnumerable<byte> byteStream )
	{
		var ct = ChecksumTable;
		return ~byteStream.Aggregate( 0xFFFFFFFF, ( checksumRegister, currentByte ) => (ct[(checksumRegister & 0xFF) ^ currentByte] ^ (checksumRegister >> 8)) );
	}

	/// <summary>
	/// Generates a CRC32 checksum from a string.
	/// </summary>
	/// <param name="str">The input to generate a checksum for.</param>
	/// <returns>The generated CRC32.</returns>
	public static uint FromString( string str )
	{
		return FromBytes( Encoding.ASCII.GetBytes( str ) );
	}

	/// <summary>
	/// Generates a CRC32 checksum from a stream asynchronously.
	/// </summary>
	/// <param name="stream">The input to generate a checksum for.</param>
	/// <returns>The generated CRC32.</returns>
	public static async Task<uint> FromStreamAsync( Stream stream )
	{
		var bufferLen = 1024 * 1024 * 8;

		if ( stream.CanSeek && stream.Length < bufferLen )
			bufferLen = (int)stream.Length + 1;

		var buffer = ArrayPool<byte>.Shared.Rent( bufferLen );

		var ct = ChecksumTable;
		uint val = 0xFFFFFFFF;

		while ( true )
		{
			var read = await stream.ReadAsync( buffer, 0, buffer.Length );
			if ( read == 0 ) break;

			for ( int i = 0; i < read; i++ )
			{
				var currentByte = buffer[i];
				val = (ct[(val & 0xFF) ^ currentByte] ^ (val >> 8));
			}
		}

		ArrayPool<byte>.Shared.Return( buffer );

		return ~val;
	}
}
