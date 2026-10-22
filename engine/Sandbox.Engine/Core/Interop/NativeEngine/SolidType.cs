namespace Sandbox
{
	internal enum SolidType : byte
	{
		SOLID_NONE = 0, // no solid model, cannot be enumerated by queries (EntitiesInBox will not find these)
		SOLID_BSP = 1,  // use whatever is in the model as collision, identical to SOLID_VPHYSICS (will be deprecated)
		SOLID_BBOX = 2, // use an AABB (world axis aligned), ignore collision in model
		SOLID_OBB = 3,  // use an OBB, ignore collision in model 
						//SOLID_OBB_YAW		= 4,	// an OBB, constrained so that it can only yaw, ignore collision in model
		SOLID_POINT = 5,    // a point, can't be solid to any other objects but can be enumerated by queries
		Physics = 6, // solid vphysics object, get vcollide from the model and collide with that
		SOLID_CAPSULE = 7,  // use a capsule shape, ignore collision in model (should replace AABB collision proxies)
		SOLID_LAST,
	}
}
