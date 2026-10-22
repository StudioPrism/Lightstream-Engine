using NativeEngine;
using Sandbox;
using System.Runtime.InteropServices;

[StructLayout( LayoutKind.Sequential )]
internal struct ManagedRenderSetup_t
{
	public IRenderContext renderContext;
	public ISceneView sceneView;
	public ISceneLayer sceneLayer;
	public ImageFormat colorImageFormat;
	public RenderMultisampleType msaaLevel;
	public SceneSystemPerFrameStats_t stats;
};
