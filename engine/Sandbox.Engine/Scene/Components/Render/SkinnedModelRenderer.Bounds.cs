namespace Sandbox;

partial class SkinnedModelRenderer
{
	internal override BBox GetWorldBoundsInternal()
	{
		if ( SceneModel.IsValid() )
			return SceneModel.animNative.m_worldBounds;

		return base.GetWorldBoundsInternal();
	}

	internal override BBox GetLocalBoundsInternal()
	{
		if ( SceneModel.IsValid() )
			return SceneModel.animNative.m_localBounds;

		return base.GetLocalBoundsInternal();
	}
}
