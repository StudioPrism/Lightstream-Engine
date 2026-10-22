
namespace OVRLipSync;

internal enum Result
{
	Success = 0,
	Unknown = -2200,
	CannotCreateContext = -2201,
	InvalidParam = -2202,
	BadSampleRate = -2203,
	MissingDLL = -2204,
	BadVersion = -2205,
	UndefinedFunction = -2206
};

internal enum AudioDataType
{
	S16_Mono,
	S16_Stereo,
	F32_Mono,
	F32_Stereo
};

internal enum Viseme
{
	sil,
	PP,
	FF,
	TH,
	DD,
	kk,
	CH,
	SS,
	nn,
	RR,
	aa,
	E,
	ih,
	oh,
	ou,
	Count
};

internal enum Signals
{
	VisemeOn,
	VisemeOff,
	VisemeAmount,
	VisemeSmoothing,
	LaughterAmount
};

internal enum ContextProvider
{
	Original,
	Enhanced,
	Enhanced_with_Laughter,
};

internal struct Frame
{
	public int FrameNumber;
	public int FrameDelay;
	public IntPtr Visemes;
	public uint VisemesLength;
	public float LaughterScore;
	public IntPtr LaughterCategories;
	public uint LaughterCategoriesLength;
};
