using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;

//
// Does it matter that our style classes are stored in a HashSet<string> and not a HashSet<int>
// Answer: yes it fucking does
//
// |         Method |      Mean |     Error |    StdDev 
// |--------------- |----------:|----------:|----------:
// | RegularHashSet | 17.592 ns | 0.2151 ns | 0.2012 ns 
// |    CaseHashSet | 16.023 ns | 0.2322 ns | 0.2172 ns 
// |     IntHashSet |  2.935 ns | 0.0774 ns | 0.0761 ns 

[MemoryDiagnoser]
[ThreadingDiagnoser]
public class StringtHashSet
{
	HashSet<string> subset;
	HashSet<int> subsetInt;
	HashSet<string> hashSetRegular;
	HashSet<string> hashSetCaseins;
	HashSet<int> hashSetInt;

	[GlobalSetup]
	public void Setup()
	{
		hashSetRegular = new HashSet<string>();
		subset = new HashSet<string>();
		subsetInt = new HashSet<int>();
		hashSetCaseins = new HashSet<string>( StringComparer.OrdinalIgnoreCase );
		hashSetInt = new HashSet<int>();

		for ( int i = 0; i < 100; i++ )
		{
			var str = Guid.NewGuid().ToString().Substring( 0, 10 );

			hashSetRegular.Add( str );
			hashSetCaseins.Add( str );
			hashSetInt.Add( str.GetHashCode() );
		}

		for ( int i = 0; i < 10; i++ )
		{
			var str = Guid.NewGuid().ToString().Substring( 0, 10 );
			subset.Add( str );
			subsetInt.Add( str.GetHashCode() );
		}
	}

	[Benchmark]
	public void RegularHashSet()
	{
		hashSetRegular.IsSubsetOf( subset );
	}

	[Benchmark]
	public void CaseHashSet()
	{
		hashSetRegular.IsSubsetOf( subset );
	}

	[Benchmark]
	public void IntHashSet()
	{
		hashSetInt.IsSubsetOf( subsetInt );
	}

}
