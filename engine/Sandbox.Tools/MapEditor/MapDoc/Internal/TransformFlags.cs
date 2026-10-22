using System;

namespace Editor.MapDoc;

[Flags]
internal enum TransformFlags
{
	None = 0,
	LockMaterial = (1 << 0),
	LockMaterialScale = (1 << 1),
	LockMaterialComponent = (1 << 2),
	Local = (1 << 3),
	Scale = (1 << 4),
	ScaleUniform = (1 << 5),
	Rotate = (1 << 6),
	Extrude = (1 << 7),
	SkipChildren = (1 << 8),
};
