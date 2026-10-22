

Class Attributes:

	
	[Handles:Sandbox.ManagedTypeName] 
	
	Set a class to use a handle system. A good example here is Sandbox.PhysicsBody. On creation we
	call a managed function to get a handle (an int) and we store that in the IPhysicsBody in native.
	Any time we pass a IPhysicsBody from native to managed, we pass the int and look up the real object.
	In the constructor of the physics object in native we call into native to destroy the handle. Which
	ensures that the pointer is cleared and can't try to be used.