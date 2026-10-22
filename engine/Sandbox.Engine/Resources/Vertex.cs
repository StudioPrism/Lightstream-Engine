namespace Sandbox
{
	[Expose]
	public struct Vertex
	{
		[VertexLayout.Position]
		public Vector3 Position;

		[VertexLayout.Color]
		public Color32 Color;

		[VertexLayout.Normal]
		public Vector3 Normal;

		[VertexLayout.TexCoord]
		public Vector4 TexCoord0;

		[VertexLayout.TexCoord]
		public Vector4 TexCoord1;

		[VertexLayout.Tangent]
		public Vector4 Tangent;

		public Vertex( in Vector3 position ) : this( position, Color32.White )
		{
		}

		public Vertex( in Vector3 position, in Color32 color ) : this()
		{
			Position = position;
			Color = color;
		}

		public Vertex( in Vector3 position, in Vector4 texCoord0, in Color32 color ) : this()
		{
			Position = position;
			TexCoord0 = texCoord0;
			Color = color;
		}

		public Vertex( in Vector3 position, in Vector3 normal, in Vector3 tangent, in Vector4 texCoord0 ) : this()
		{
			Position = position;
			Normal = normal;
			Tangent = new Vector4( tangent.x, tangent.y, tangent.z, -1.0f );
			TexCoord0 = texCoord0;
			Color = Color32.White;
		}

		public static readonly VertexAttribute[] Layout =
		{
			new VertexAttribute( VertexAttributeType.Position, VertexAttributeFormat.Float32, 3 ),
			new VertexAttribute( VertexAttributeType.Color, VertexAttributeFormat.UInt8, 4 ),
			new VertexAttribute( VertexAttributeType.Normal, VertexAttributeFormat.Float32, 3 ),
			new VertexAttribute( VertexAttributeType.TexCoord, VertexAttributeFormat.Float32, 4, 0 ),
			new VertexAttribute( VertexAttributeType.TexCoord, VertexAttributeFormat.Float32, 4, 1 ),
			new VertexAttribute( VertexAttributeType.Tangent, VertexAttributeFormat.Float32, 4 ),
		};
	}
}
