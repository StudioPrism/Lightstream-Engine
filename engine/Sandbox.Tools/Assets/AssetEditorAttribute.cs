using Sandbox;
using System;

namespace Editor;

/// <summary>
/// Used in conjunction with IAssetEditor to declare a window that can edit an asset type
/// </summary>
[AttributeUsage( AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true )]
public class EditorForAssetTypeAttribute : System.Attribute
{
	public string Extension { get; private set; }

	public EditorForAssetTypeAttribute( string extension )
	{
		Extension = extension;
	}
}
