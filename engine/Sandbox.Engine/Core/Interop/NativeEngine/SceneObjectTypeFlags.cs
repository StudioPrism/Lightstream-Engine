
using System;

namespace NativeEngine
{
	[Flags]
	internal enum ESceneObjectTypeFlags : uint
	{
		NONE = 0x00,

		FROM_POOL = 0x02,       //< was allocated from our pool
		HAS_MESH_INSTANCE_DATA = 0x04,      //< this object has allocated it's meshinstance data through scenesystem
		DELETE_MESH_INSTANCE_DATA = 0x08,       //< we need to delete this mesh instance data when the sceneobject is deleted because we own it
		NOT_BATCHABLE = 0x10,       //< this object is not batchable by material for some reason ( example: has dynamic attributes that affect rendering )

		/// For objects that can't be considered to be "owned" by the world they are in because they
		/// are owned by a manager. All this flag does is cause a warning when such an object is still
		/// in the world at world deletion time (a leak).
		SHOULD_BE_DELETED_BEFORE_WORLD = 0x20,

		/// if this flag is set, then the object will not be deleted when deleting the world, and will not be queued for delete. It's assumed that this object is going to be deleted inside of the destructor of another sceneobject
		OWNED_BY_ANOTHER_SCENEOBJECT = 0x40,

		/// We have a mixture of alpha-blended and non-alpha blended draws
		PARTIALLY_ALPHA_BLENDED = 0x80,

		/// A unique batch flag that allows objects to draw in a separate batch from their original group
		UNIQUE_BATCH_GROUP = 0x100
	}
}
