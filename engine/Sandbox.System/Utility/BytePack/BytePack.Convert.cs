namespace Sandbox;

internal partial class BytePack
{
	/// <summary>
	/// Allows classes to specify how they are to be serialized and deserialized through BytePack.
	/// </summary>
	public interface ISerializer
	{
		/// <summary>
		/// Read from a <see cref="ByteStream"/> and return an object.
		/// </summary>
		/// <param name="bs">The incoming byte stream.</param>
		/// <param name="targetType">The expected type.</param>
		/// <returns></returns>
		static abstract object BytePackRead( ref ByteStream bs, Type targetType );

		/// <summary>
		/// Write a value to an outgoing <see cref="ByteStream"/>.
		/// </summary>
		/// <param name="value">The value to be serialized.</param>
		/// <param name="bs">The outgoing byte stream.</param>
		static abstract void BytePackWrite( object value, ref ByteStream bs );
	}
}
