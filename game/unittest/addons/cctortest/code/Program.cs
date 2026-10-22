using Sandbox;
using System;

namespace StaticCtorTest;

#nullable enable

public class Example
{
	public static Exception ThrownException { get; private set; }

	static Example()
	{
		try
		{
			// Should throw an InvalidOperationException
			TypeLibrary.GetType<Example>();
		}
		catch (Exception ex)
		{
			ThrownException = ex;
		}
	}
}
