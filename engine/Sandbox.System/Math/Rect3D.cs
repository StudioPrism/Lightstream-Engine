using Sandbox.UI;
using System;
using System.Runtime.InteropServices;


/// <summary>
/// Generally used to describe the size of textures
/// </summary>
[StructLayout( LayoutKind.Sequential )]
internal struct Rect3D
{
	public int x, y, z;
	public int width, height, depth;

	public Rect3D( int x, int y, int z, int width, int height, int depth )
	{
		this.x = x;
		this.y = y;
		this.z = z;

		this.width = width;
		this.height = height;
		this.depth = depth;
	}

	void Clear()
	{
		x = 0;
		y = 0;
		z = 0;
		width = 0;
		height = 0;
		depth = 0;
	}

	int Size()
	{
		return width * height * depth;
	}

	bool Intersects( Rect3D other )
	{
		if ( (x + width <= other.x) || (other.x + other.width <= x) ) return false;
		if ( (y + height <= other.y) || (other.y + other.height <= y) ) return false;
		if ( (z + depth <= other.z) || (other.z + other.depth <= z) ) return false;

		return true;
	}
}
