namespace Sandbox.Audio;

[AttributeUsage( AttributeTargets.Property )]
public class AudioDistanceFloatAttribute : EditorAttribute
{
	public AudioDistanceFloatAttribute() : base( "audiodistance" )
	{
	}
}
