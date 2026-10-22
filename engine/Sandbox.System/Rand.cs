using System;
using System.Collections.Generic;

namespace Sandbox;

/// <summary>
/// Hidden random class. This is secretly used by Game.Random, but being here 
/// allows all of our system functions to use the same Random instance.
/// </summary>
static class SandboxSystem
{
	[ThreadStatic]
	static Random _random;

	internal static Random Random
	{
		get
		{
			_random ??= new Random();
			return _random;
		}
	}

	/// <summary>
	/// Sets the seed for these static classes
	/// </summary>
	public static void SetRandomSeed( int seed )
	{
		_random = new Random( seed );
	}
}
