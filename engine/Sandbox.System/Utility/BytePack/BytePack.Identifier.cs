namespace Sandbox;

internal partial class BytePack
{
	/// <summary>
	/// A header before each variable
	/// </summary>
	internal enum Identifier : byte
	{
		Null,

		Object,
		String,

		// natives
		Bool,
		Byte,
		SByte,
		Int,
		UInt,
		Float,
		Double,
		Decimal,
		Short,
		UShort,
		Guid,
		TimeSpan,
		DateTime,
		DateTimeOffset,
		Long,
		ULong,
		Char,

		// collections
		ArrayValue,
		Array,
		List,
		Dictionary,

		// runtime types
		Runtime,

		// common game
		Vector2,
		Vector2Int,
		Vector3,
		Vector3Int,
		Vector4,
		Rotation,
		Angles,
		Transform,
		Color,
		Color32,
		BBox,
		Sphere,
		Plane,
		Rect,
		Ray,
		Line,
		Matrix,
		SteamId
	}
}
