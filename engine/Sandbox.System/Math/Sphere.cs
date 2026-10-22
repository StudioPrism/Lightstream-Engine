using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
namespace Sandbox;

/// <summary>
/// Represents a sphere.
/// </summary>
[StructLayout( LayoutKind.Sequential )]
public struct Sphere
{
	/// <summary>
	/// A sphere centered at the origin, with radius 1.
	/// </summary>
	public static Sphere Unit { get; } = new( 0f, 1f );

	public Sphere( Vector3 center, float radius )
	{
		Center = center;
		Radius = radius;
	}

	public override readonly string ToString()
	{
		return $"Center({Center}), Radius({Radius})";
	}

	/// <summary>
	/// Center of the sphere.
	/// </summary>
	[JsonInclude]
	public Vector3 Center;

	/// <summary>
	/// Radius of the sphere.
	/// </summary>
	[JsonInclude]
	public float Radius;

	/// <summary>
	/// Performs an intersection test between this sphere and given ray.
	/// </summary>
	public readonly bool Trace( Ray ray, float maxDistance, out float distance )
	{
		distance = 0;

		var dirToCenter = ray.Position - Center;
		var v = (ray.Forward.Dot( Center - ray.Position ));
		var disc = Radius * Radius - ((dirToCenter.Dot( dirToCenter )) - v * v);

		if ( disc >= 0.0f )
		{
			var time = (v - MathF.Sqrt( disc )) / maxDistance;
			distance = maxDistance * time;
			return (time >= 0.0f && time <= 1.0f);
		}

		return false;
	}

	/// <summary>
	/// Performs an intersection test between this sphere and given ray.
	/// </summary>
	public readonly bool Trace( Ray ray, float maxDistance )
	{
		var dirToCenter = ray.Position - Center;
		var v = (ray.Forward.Dot( Center - ray.Position ));
		var disc = Radius * Radius - ((dirToCenter.Dot( dirToCenter )) - v * v);

		if ( disc >= 0.0f )
		{
			var time = (v - MathF.Sqrt( disc )) / maxDistance;
			return (time >= 0.0f && time <= 1.0f);
		}

		return false;
	}

	/// <summary>
	/// Returns true if sphere contains point. False if the point falls outside the sphere.
	/// </summary>
	public bool Contains( in Vector3 value )
	{
		return (value - Center).Length <= Radius;
	}


	/// <summary>
	/// Volume of this sphere
	/// </summary>
	[JsonIgnore]
	public readonly float Volume
	{
		get
		{
			return (4.0f / 3.0f) * MathF.PI * Radius * Radius * Radius;
		}
	}

	/// <summary>
	/// Get the volume of this sphere
	/// </summary>
	[Obsolete( "Use Sphere.Volume instead." )]
	public readonly float GetVolume()
	{
		return Volume;
	}

	/// <summary>
	/// Calculates the shortest distance from the specified local position to the nearest edge of the object.
	/// </summary>
	public readonly float GetEdgeDistance( Vector3 localPos )
	{
		float distanceToCenter = (localPos - Center).Length;
		return MathF.Abs( distanceToCenter - Radius );
	}

	/// <summary>
	/// Returns a random point within this sphere.
	/// </summary>
	[JsonIgnore]
	public readonly Vector3 RandomPointInside
	{
		get
		{
			return Center + System.Random.Shared.VectorInSphere( Radius );
		}
	}

	/// <summary>
	/// Returns a random point on the edge of this sphere.
	/// </summary>
	[JsonIgnore]
	public readonly Vector3 RandomPointOnEdge
	{
		get
		{
			return Center + System.Random.Shared.VectorInSphere( 1 ).Normal * Radius;
		}
	}
}
