// Extensions for the CUtlBuffer type

internal partial struct CUtlBuffer
{
	public unsafe byte[] ToArray()
	{
		IntPtr p = Base();
		int size = TellMaxPut();

		return new Span<byte>( p.ToPointer(), size ).ToArray();
	}
};
