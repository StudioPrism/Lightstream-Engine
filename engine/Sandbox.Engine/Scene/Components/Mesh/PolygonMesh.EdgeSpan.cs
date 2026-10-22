using HalfEdgeMesh;

namespace Sandbox;

public partial class PolygonMesh
{
	private class EdgeSpan
	{
		private static bool FindVertexListInFace( PolygonMesh mesh, FaceHandle hFace, HalfEdgeHandle hStartFaceVertex, HalfEdgeHandle hEndFaceVertex, out List<VertexHandle> outVertices )
		{
			outVertices = new List<VertexHandle>();

			if ( mesh is null || hFace == FaceHandle.Invalid )
				return false;

			var hVertexStart = mesh.GetVertexConnectedToFaceVertex( hStartFaceVertex );
			outVertices.Add( hVertexStart );

			var hCurrentFaceVertex = mesh.GetNextVertexInFace( hStartFaceVertex );

			while ( hCurrentFaceVertex != hStartFaceVertex )
			{
				var hVertex = mesh.GetVertexConnectedToFaceVertex( hCurrentFaceVertex );
				outVertices.Add( hVertex );

				if ( hCurrentFaceVertex == hEndFaceVertex )
					return true;

				hCurrentFaceVertex = mesh.GetNextVertexInFace( hCurrentFaceVertex );
			}

			outVertices.Clear();
			return false;
		}

		public bool InitializeFromFace( PolygonMesh mesh, FaceHandle hFace, HalfEdgeHandle hStartVertex, HalfEdgeHandle hEndVertex )
		{
			// Build the list of vertices contained in the span
			if ( FindVertexListInFace( mesh, hFace, hStartVertex, hEndVertex, out var vertices ) == false )
				return false;

			return InitializeFromVertices( mesh, vertices );
		}

		public bool InitializeFromVertices( PolygonMesh mesh, IReadOnlyList<VertexHandle> vertices )
		{
			if ( vertices.Count < 2 )
				return false;

			Vertices.Clear();
			VertexParameters.Clear();

			Mesh = mesh;
			Vertices = new List<VertexHandle>( vertices );
			var numVertices = Vertices.Count;

			//
			// Get the positions of each vertex
			//
			var vertexPositions = new Vector3[numVertices];

			for ( int vertexIndex = 0; vertexIndex < numVertices; ++vertexIndex )
			{
				vertexPositions[vertexIndex] = mesh.GetVertexPosition( Vertices[vertexIndex] );
			}

			//
			// Compute the parameter of each vertex along the edge
			//
			VertexParameters.Clear();
			VertexParameters.Add( 0.0f );

			float sumLength = 0.0f;
			for ( int i = 1; i < numVertices; ++i )
			{
				sumLength += vertexPositions[i].Distance( vertexPositions[i - 1] );
				VertexParameters.Add( sumLength );
			}

			for ( int i = 1; i < numVertices; ++i )
			{
				VertexParameters[i] /= sumLength;
			}

			return true;
		}

		public void AddVertices( int numVerticesToAdd, out int[] outVertices )
		{
			outVertices = new int[numVerticesToAdd];
			Array.Fill( outVertices, -1 );

			var step = 1.0f / (numVerticesToAdd + 1);
			var tolerance = step / 100.0f;

			int lastEdgeIndex = 0;
			for ( int vertexToAddIndex = 0; vertexToAddIndex < numVerticesToAdd; ++vertexToAddIndex )
			{
				var param = step * (vertexToAddIndex + 1);

				var edgeIndex = FindEdgeContainingParameter( param, lastEdgeIndex );
				if ( edgeIndex < 0 )
					continue;

				var paramA = VertexParameters[edgeIndex + 0];
				var paramB = VertexParameters[edgeIndex + 1];
				var hVertexA = Vertices[edgeIndex + 0];
				var hVertexB = Vertices[edgeIndex + 1];

				int vertexIndex;
				if ( param.AlmostEqual( paramA, tolerance ) )
				{
					vertexIndex = edgeIndex;
				}
				else if ( param.AlmostEqual( paramB, tolerance ) )
				{
					vertexIndex = edgeIndex + 1;
				}
				else
				{
					var edgeParam = MathX.Clamp( (param - paramA) / (paramB - paramA), 0.0f, 1.0f );

					Mesh.AddVertexToEdge( hVertexA, hVertexB, edgeParam, out var hNewVertex );

					vertexIndex = edgeIndex + 1;
					Vertices.Insert( vertexIndex, hNewVertex );
					VertexParameters.Insert( vertexIndex, param );
				}

				lastEdgeIndex = vertexIndex;
				outVertices[vertexToAddIndex] = vertexIndex;
			}
		}

		public void Reverse()
		{
			Vertices.Reverse();
			VertexParameters.Reverse();

			var numVertices = VertexParameters.Count;
			for ( int i = 0; i < numVertices; ++i )
			{
				VertexParameters[i] = 1.0f - VertexParameters[i];
			}
		}

		public void GetAllEdges( List<HalfEdgeHandle> outEdges )
		{
			var numEdges = Vertices.Count - 1;
			for ( int edgeIndex = 0; edgeIndex < numEdges; ++edgeIndex )
			{
				outEdges.Add( Mesh.FindEdgeConnectingVertices( Vertices[edgeIndex], Vertices[edgeIndex + 1] ) );
			}
		}

		public HalfEdgeHandle GetEdge( int edgeIndex )
		{
			return Mesh.FindEdgeConnectingVertices( Vertices[edgeIndex], Vertices[edgeIndex + 1] );
		}

		public VertexHandle GetVertex( int vertexIndex )
		{
			return Vertices[vertexIndex];
		}

		public PolygonMesh Mesh { get; private set; }

		public int NumVertices => Vertices.Count;
		public int NumEdges => Vertices.Count - 1;

		private int FindEdgeContainingParameter( float value, int startIndex )
		{
			var numEdges = Vertices.Count - 1;
			for ( int i = startIndex; i < numEdges; ++i )
			{
				if ( (value >= VertexParameters[i]) && (value <= VertexParameters[i + 1]) )
				{
					return i;
				}
			}
			return -1;
		}

		private List<VertexHandle> Vertices = new();
		private readonly List<float> VertexParameters = new();
	}
}
