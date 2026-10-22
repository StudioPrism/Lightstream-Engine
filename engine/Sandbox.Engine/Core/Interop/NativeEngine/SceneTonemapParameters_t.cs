using System.Runtime.InteropServices;

[StructLayout( LayoutKind.Sequential )]
internal unsafe struct SceneTonemapParameters_t
{
	public float m_flAutoExposureMin;
	public float m_flAutoExposureMax;
	public float m_flExposureCompensationScalar;

	public float m_flTonemapPercentTarget;            // Legacy params for S1 back-compatibility
	public float m_flTonemapPercentBrightPixels;
	public float m_flTonemapMinAvgLum;

	public float m_flRate;                            // Legacy params for S1 back-compatibility
	public float m_flAccelerateExposureDown;

	public float m_flExposureAdaptationSpeedUp;       // Increasing exposure speed in f-stop/sec (making dark scene brighter)
	public float m_flExposureAdaptationSpeedDown;     // Decreasing exposure speed in f-stop/sec (making bright scene darker)

	public float m_flTonemapEVSmoothingRange;         // When the target exposure and the current exposure are in this range, logarithmically approach the target

	public SceneTonemapParameters_t()
	{
		m_flAutoExposureMin = -1f;
		m_flAutoExposureMax = -1f;
		m_flExposureCompensationScalar = -1f;
		m_flTonemapPercentTarget = -1f;
		m_flTonemapPercentBrightPixels = -1f;
		m_flTonemapMinAvgLum = -1f;
		m_flRate = -1f;
		m_flAccelerateExposureDown = -1f;
		m_flExposureAdaptationSpeedUp = -1f;
		m_flExposureAdaptationSpeedDown = -1f;
		m_flTonemapEVSmoothingRange = -1f;
	}
};
