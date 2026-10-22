namespace Sandbox.Navigation.Generation;

[SkipHotload]
internal static class Constants
{
	public const int NOT_CONNECTED = 0x3f;
	public const int NULL_AREA = 0;
	public const int WALKABLE_AREA = 1;
	public const int SPAN_MAX_HEIGHT = 0xffff;
	public const int SPAN_HEIGHT_BITS = 16;
	public const int VERTS_PER_POLYGON = 6;
	public const ushort MESH_NULL_IDX = 0xffff;
}


[SkipHotload]
internal static class ContourRegionFlags
{
	public const int CONTOUR_REG_MASK = 0xFFFF;
	public const int BORDER_REG = 0x8000;
	public const int BORDER_VERTEX = 0x10000;
	public const int AREA_BORDER = 0x20000;
}
