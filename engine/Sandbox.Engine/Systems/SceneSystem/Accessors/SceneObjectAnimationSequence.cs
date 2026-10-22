
using Sandbox;

/// <summary>
/// Hidden class. addon code should only ever access AnimationSequence.
/// </summary>
internal sealed class SceneObjectAnimationSequence : AnimationSequence
{
	readonly SceneModel Target;

	public SceneObjectAnimationSequence( SceneModel sceneObject )
	{
		Target = sceneObject;
	}

	public override float Duration => Target.animNative.GetSequenceDuration();

	public override bool IsFinished => Target.animNative.IsSequenceFinished();

	public override string Name
	{
		get => Target.animNative.GetSequence();
		set => Target.animNative.SetSequence( value );
	}

	public override float TimeNormalized
	{
		get => Target.animNative.GetSequenceCycle();
		set => Target.animNative.SetSequenceCycle( value );
	}

	public override float Time
	{
		get => TimeNormalized * Duration;
		set => TimeNormalized = value / Duration;
	}

	internal override bool Looping
	{
		set => Target.animNative.SetSequenceLooping( value );
	}

	internal override bool Blending
	{
		set => Target.animNative.SetSequenceBlending( value );
	}

	public override IReadOnlyList<string> SequenceNames => Target.Model.SequenceNames;
}
