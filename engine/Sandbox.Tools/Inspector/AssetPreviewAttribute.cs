using System;

namespace Editor;

[AttributeUsage( AttributeTargets.Class, AllowMultiple = true )]
public class AssetPreviewAttribute : Attribute
{
	public string Extension { get; }

	public AssetPreviewAttribute( string extension )
	{
		Extension = extension;
	}
}
