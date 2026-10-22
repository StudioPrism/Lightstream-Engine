HEADER
{
	Description = "";
}

FEATURES
{
	#include "common/features.hlsl"
}

MODES
{
	Forward();
	Depth( S_MODE_DEPTH );
	ToolsShadingComplexity( "tools_shading_complexity.shader" );
}

COMMON
{
	#ifndef S_ALPHA_TEST
	#define S_ALPHA_TEST 1
	#endif

	#ifndef S_TRANSLUCENT
	#define S_TRANSLUCENT 0
	#endif

	#include "common/shared.hlsl"
	#include "procedural.hlsl"

	#define S_UV2 1
	#define CUSTOM_MATERIAL_INPUTS
}

struct VertexInput
{
	#include "common/vertexinput.hlsl"
	float4 vColor : COLOR0 < Semantic( Color ); >;
};

struct PixelInput
{
	#include "common/pixelinput.hlsl"
	float3 vPositionOs : TEXCOORD14;
	float3 vNormalOs : TEXCOORD15;
	float4 vTangentUOs_flTangentVSign : TANGENT < Semantic( TangentU_SignV ); >;
	float4 vColor : COLOR0;
	float4 vTintColor : COLOR1;
};

VS
{
	#include "common/vertex.hlsl"

	PixelInput MainVs( VertexInput v )
	{
		PixelInput i = ProcessVertex( v );

		i.vPositionOs = v.vPositionOs.xyz;
		i.vColor = v.vColor;

		ExtraShaderData_t extraShaderData = GetExtraPerInstanceShaderData( v.nInstanceTransformID );
		i.vTintColor = extraShaderData.vTint;

		VS_DecodeObjectSpaceNormalAndTangent( v, i.vNormalOs, i.vTangentUOs_flTangentVSign );

		return FinalizeVertex( i );
	}
}

PS
{
	#include "common/pixel.hlsl"

	SamplerState g_sSampler0 < Filter( ANISO ); AddressU( WRAP ); AddressV( WRAP ); >;

	CreateInputTexture2D( AlbedoMap, Srgb, 8, "None", "_color", ",0/,0/0", Default3( 1.0, 1.0, 1.0 ) );
	CreateInputTexture2D( OpacityMap, Srgb, 8, "None", "_trans", ",0/,0/0", Default3( 1.0, 1.0, 1.0 ) );
	CreateInputTexture2D( NormalMap, Srgb, 8, "NormalizeNormals", "_normal", ",0/,0/0", Default3( 0.5, 0.5, 1.0 ) );
	CreateInputTexture2D( SpecularMap, Srgb, 8, "None", "_rough", ",0/,0/0", Default3( 0.0, 0.0, 0.0 ) );
	CreateInputTexture2D( EmissiveMap, Srgb, 8, "None", "", ",0/,0/0", Default3( 0.0, 0.0, 0.0 ) );

	Texture2D g_tAlbedoMap   < Channel( RGBA, Box( AlbedoMap ), Srgb );   OutputFormat( DXT5 ); SrgbRead( True ); >;
	Texture2D g_tOpacityMap  < Channel( RGBA, Box( OpacityMap ), Srgb );  OutputFormat( DXT5 ); SrgbRead( True ); >;
	Texture2D g_tNormalMap   < Channel( RGBA, Box( NormalMap ), Srgb );   OutputFormat( DXT5 ); SrgbRead( True ); >;
	Texture2D g_tSpecularMap < Channel( RGBA, Box( SpecularMap ), Srgb ); OutputFormat( DXT5 ); SrgbRead( True ); >;
	Texture2D g_tEmissiveMap < Channel( RGBA, Box( EmissiveMap ), Srgb ); OutputFormat( DXT5 ); SrgbRead( True ); >;

	float g_flEmissiveMod < UiGroup( ",0/,0/0" ); Default1( 1 ); Range1( 0, 100 ); >;

	float4 MainPs( PixelInput i ) : SV_Target0
	{
		Material m = Material::Init();

		m.Metalness         = 0;
		m.AmbientOcclusion  = 1;
		m.TintMask          = 1;
		m.Transmission      = 0;

		float4 albedo       = Tex2DS( g_tAlbedoMap,   g_sSampler0, i.vTextureCoords.xy );
		float4 opacity      = Tex2DS( g_tOpacityMap,  g_sSampler0, i.vTextureCoords.xy );
		float4 normal       = Tex2DS( g_tNormalMap,   g_sSampler0, i.vTextureCoords.xy );
		float4 specular     = Tex2DS( g_tSpecularMap, g_sSampler0, i.vTextureCoords.xy );
		float4 emissive     = Tex2DS( g_tEmissiveMap, g_sSampler0, i.vTextureCoords.xy );

		m.Albedo            = albedo.xyz * i.vTintColor.rgb;
		m.Opacity           = opacity.x;
		m.Normal            = normal.xyz;
		m.Roughness         = 1.0 - specular.a;
		m.Emission          = emissive.rgb * g_flEmissiveMod;

		m.Normal            = TransformNormal( m.Normal, i.vNormalWs, i.vTangentUWs, i.vTangentVWs );
		m.WorldTangentU     = i.vTangentUWs;
		m.WorldTangentV     = i.vTangentVWs;
		m.TextureCoords     = i.vTextureCoords.xy;

		return ShadingModelStandard::Shade( i, m );
	}
}
