namespace Sandbox
{
	/// <summary>
	/// Represents <see cref="PhysicsBody">Physics body's</see> motion type.
	/// </summary>
	public enum PhysicsMotionType
	{
		/// <summary>
		/// Invalid type.
		/// </summary>
		Invalid = 0,

		/// <summary>
		/// Physically simulated body.
		/// </summary>
		Dynamic,

		/// <summary>
		/// Cannot move at all.
		/// </summary>
		Static,

		/// <summary>
		/// No physics simulation, but can be moved via setting position/rotation.
		/// </summary>
		Keyframed
	}
}
