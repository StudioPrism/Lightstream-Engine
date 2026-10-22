using System.Runtime.InteropServices;

namespace Sandbox;

[StructLayout( LayoutKind.Sequential )]
public struct SimpleVertex
{
	public SimpleVertex( Vector3 position, Vector3 normal, Vector3 tangent, Vector2 texcoord )
	{
		this.position = position;
		this.normal = normal;
		this.tangent = tangent;
		this.texcoord = texcoord;
	}

	[VertexLayout.Position]
	public Vector3 position;

	[VertexLayout.Normal]
	public Vector3 normal;

	[VertexLayout.Tangent]
	public Vector3 tangent;

	[VertexLayout.TexCoord]
	public Vector2 texcoord;

	public static readonly VertexAttribute[] Layout =
	{
		new ( VertexAttributeType.Position, VertexAttributeFormat.Float32, 3 ),
		new ( VertexAttributeType.Normal, VertexAttributeFormat.Float32, 3 ),
		new ( VertexAttributeType.Tangent, VertexAttributeFormat.Float32, 3 ),
		new ( VertexAttributeType.TexCoord, VertexAttributeFormat.Float32, 2 )
	};
}
