
struct audio_source_indexstate_t
{
	public uint m_nPacketIndex;
	public uint m_nBufferSampleOffset;
	public uint m_nSampleFracOffset;
	public uint m_nDelaySamples;
	public void Clear()
	{
		m_nPacketIndex = 0;
		m_nBufferSampleOffset = 0;
		m_nSampleFracOffset = 0;
		m_nDelaySamples = 0;
	}
};
