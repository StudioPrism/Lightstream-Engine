
unsafe struct MeshTraceInput
{
	public Vector3 start;
	public Vector3 end;
	public fixed uint tagRequire[8];
	public fixed uint tagAny[8];
	public fixed uint tagExclude[8];
	public int cullMode;
	public IntPtr filterDelegate;
};

struct MeshTraceOutput
{
	public float distance;
	public Vector3 position;
	public Vector3 normal;
	public IntPtr material;
	public Transform transform;
	public int sceneobjectHandle;
	public int triangleIndex;
	public Vector2 uv;

	public IntPtr v0;
	public IntPtr v1;
	public IntPtr v2;
};

unsafe struct TraceVertex_t
{
	// needs to match MeshVertex_t in imapbuilder
	public Vector3 m_vPosition;
	public Vector3 m_vNormal;
	public Vector4 m_vTangent;
	public Vector2 m_vTexCoord;
	public Vector4 m_vColor;
	public Vector2 m_vTexCoord2;
	public fixed float m_vVertexPaintBlendParams[8];
	public Vector4 m_vVertexPaintTintColor;

	// extra data
	public fixed ushort m_nBoneIndices[4]; // For posed prop conversion
	public fixed byte m_flBoneWeights[4]; // For posed prop conversion
};
