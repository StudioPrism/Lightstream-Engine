using BenchmarkDotNet.Attributes;
using Sandbox.Internal;

[MemoryDiagnoser]
[ThreadingDiagnoser]
public class TypelibraryMark
{
	[Benchmark]
	public void AddAssembly()
	{
		var tl = new TypeLibrary();

		tl.AddAssembly( typeof( Sandbox.CookieContainer ).Assembly, true );
	}

}
