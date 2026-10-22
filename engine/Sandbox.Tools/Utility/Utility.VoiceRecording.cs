using System.Runtime.InteropServices;
namespace Editor;

public static partial class EditorUtility
{
	public static class VoiceRecording
	{
		const string deviceName = "sbrec";

		[DllImport( "winmm.dll", EntryPoint = "mciSendString", CharSet = CharSet.Auto )]
		internal static extern int mciSendString( string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback );

		/// <summary>
		/// Start recording data from microphone
		/// </summary>
		public static void Start( int samples = 44100, int bytesPerSecond = 192000 )
		{
			mciSendString( $"open new type WAVEAudio alias {deviceName}", "", 0, 0 );
			mciSendString( $"set {deviceName} time format ms bitspersample 16 channels 1 samplespersec {samples} bytespersec {bytesPerSecond} alignment 2", "", 0, 0 );
			mciSendString( $"record {deviceName}", "", 0, 0 );
		}

		/// <summary>
		/// Stop recording data from microphone
		/// </summary>
		public static void Stop()
		{
			mciSendString( $"stop {deviceName}", "", 0, 0 );
		}

		/// <summary>
		/// Flush any recorded data so we don't have it kept in memory
		/// </summary>
		public static void Flush()
		{
			mciSendString( $"close {deviceName}", "", 0, 0 );
		}

		/// <summary>
		/// Grab any recorded voice data and save it as a WAV file
		/// </summary>
		public static bool Save( string path )
		{
			return mciSendString( $"save {deviceName} \"{path}\"", "", 0, 0 ) == 0;
		}
	}
}
