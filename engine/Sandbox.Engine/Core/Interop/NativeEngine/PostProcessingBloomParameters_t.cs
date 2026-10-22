
internal struct PostProcessingBloomParameters_t
{
	public BloomBlendMode_t m_blendMode = BloomBlendMode_t.BLOOM_BLEND_ADD;
	public float m_flBloomStrength = 2.0f;            // Strength of additive bloom blend mode
	public float m_flScreenBloomStrength = 1.0f;
	public float m_flBlurBloomStrength = 1.0f;
	public float m_flBloomThreshold = 0.0f;
	public float m_flBloomThresholdWidth = 1.0f;
	public float m_flSkyboxBloomStrength = 1.0f;
	public float m_flBloomStartValue = 1.0f;

	public float m_flBlurWeight0 = 0.2f;
	public float m_flBlurWeight1 = 0.2f;
	public float m_flBlurWeight2 = 0.2f;
	public float m_flBlurWeight3 = 0.2f;
	public float m_flBlurWeight4 = 0.2f;

	public Vector3 m_vBlurTint0 = Vector3.Zero;
	public Vector3 m_vBlurTint1 = Vector3.Zero;
	public Vector3 m_vBlurTint2 = Vector3.Zero;
	public Vector3 m_vBlurTint3 = Vector3.Zero;
	public Vector3 m_vBlurTint4 = Vector3.Zero;

	public enum BloomBlendMode_t : int
	{
		BLOOM_BLEND_ADD = 0,
		BLOOM_BLEND_SCREEN,
		BLOOM_BLEND_BLUR
	}

	public PostProcessingBloomParameters_t()
	{

	}
};
