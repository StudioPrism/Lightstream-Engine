
internal static class OOBChecks
{
	public static void ThrowIfBoneOutOfBounds( int bone, int boneCount, string argName )
	{
		if ( bone >= 0 && bone < boneCount ) return;

		if ( boneCount == 0 ) throw new ArgumentOutOfRangeException( argName, $"Tried to access out of range bone index {bone}, model has no bones!" );
		throw new ArgumentOutOfRangeException( argName, $"Tried to access out of range bone index {bone}, range is 0-{boneCount - 1}" );
	}
}
