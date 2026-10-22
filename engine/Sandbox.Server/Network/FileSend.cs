using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Sandbox.Diagnostics;

namespace Sandbox;

internal class FileSend : System.IDisposable
{
	private static readonly Logger log = new Logger( "FileSend" );
	List<int> FileQueue = new List<int>();

	System.IO.Stream Stream;

	/// <summary>
	/// Add a file to download
	/// </summary>
	internal void Queue( int fileId )
	{
		// TODO - validity check

		FileQueue.Add( fileId );
	}

	public void Dispose()
	{
		Stream?.Dispose();
		Stream = null;

		FileQueue.Clear();
	}
}
