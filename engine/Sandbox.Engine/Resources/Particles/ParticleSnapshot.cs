namespace Sandbox;

/// <summary>
/// A particle snapshot that can be created procedurally.
/// Contains a set of vertices that particle effects can address.
/// </summary>
[Obsolete]
public sealed partial class ParticleSnapshot : Resource
{
	public override bool IsValid => true;

	/// <summary>
	/// A vertex to update a particle snapshot with.
	/// </summary>
	public struct Vertex
	{
		public Vector3 Position;
		public Vector3 Normal;
		public Color Color;
		public float CreationTime;
		public float Radius;
		public float ForceScale;
	}

	/// <summary>
	/// Create new empty procedural particle snapshot.
	/// </summary>
	public ParticleSnapshot()
	{

	}

	/// <summary>
	/// Update this snapshot with a list of vertices.
	/// </summary>
	public unsafe void Update( Span<Vertex> vertices )
	{

	}

	~ParticleSnapshot()
	{
	}
}
