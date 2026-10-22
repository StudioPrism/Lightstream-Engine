
namespace Sandbox.Audio;

/// <summary>
/// An instance of a DspPreset. The actual processor creates one of these from
/// a DspPreset, and then uses it to process the audio buffers.
/// </summary>
class DspInstance
{
	global::DspInstance _native;

	internal DspInstance( DspPreset source, int channels )
	{
		_native = source.GetNative().Instantiate( channels );
	}

	~DspInstance()
	{
		MainThread.QueueDispose( _native );
	}

	public void Process( MixBuffer input, MixBuffer output, int channel )
	{
		_native.ProcessChannel( input._native, output._native, channel );
	}

	internal void Dispose()
	{
		GC.SuppressFinalize( this );
		MainThread.QueueDispose( _native );
	}
}
