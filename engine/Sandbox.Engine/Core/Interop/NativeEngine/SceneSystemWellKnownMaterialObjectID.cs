namespace NativeEngine
{
	internal enum SceneSystemWellKnownMaterialObjectID : int
	{
		SCENESYSTEMMAT_POINTLIGHT = 0,      //< material for a simple point light
		SCENESYSTEMMAT_SPOT,                //< simple unshadowed spot light
		SCENESYSTEMMAT_SPOT_SHADOWED,       //< shadowed spot light
		SCENESYSTEMMAT_ENV_MAP,             //< environment map
		SCENESYSTEMMAT_VOLUME_SHADOWED,     //< volume-shadowed spot light
		SCENESYSTEMMAT_COPY_TO_BACKBUFFER,  //< copy from scratch to back buffer
		SCENESYSTEMMAT_DOWNSAMPLE,          //< various downsample shaders
		SCENESYSTEMMAT_SOLID_OVERLAY,       //< draw 3d solid/striped/checkered triangles
		SCENESYSTEMMAT_WIREFRAME_DEBUGOVERLAY,  //< draw lit 3d lines
		SCENESYSTEMMAT_WIREFRAME_OVERLAY,   //< draw 3d lines
		SCENESYSTEMMAT_SCREENSPACE_WIREFRAME, //< draw colored screenspace (2D) lines
		SCENESYSTEMMAT_TEXTURE_OVERLAY,     //< draw 2d textured quads
		SCENESYSTEMMAT_TOOLS_2D_GENERIC,    //< screenspace, textured

		SCENESYSTEMMAT_SUN_LIGHT,
		SCENESYSTEMMAT_DEBUG_SHOW_SUN_SHADOW_SPLITS,

		SCENESYSTEMMAT_DEBUG_UI,                //< draw screenspace UI
		SCENESYSTEMMAT_DEBUG_UI_ALPHATEXTURE,   //< draw screenspace UI, input texture only contains alpha

		SCENESYSTEMMAT_MORPH_MATERIAL,

		SCENESYSTEMMAT_OCCLUSION_PROXY,             //< issue an occlusion query by rendering depth
		SCENESYSTEMMAT_OCCLUSION_PROXY_COUNT_DRAW,  //< issue an occlusion query on how many total pixels could be rendered, assuming 100% visibility

		SCENESYSTEMMAT_OCCLUSION_PROXY_DEBUG_DRAW,  //< draw the occluder proxy for this occlusion query


		SCENESYSTEMMAT_VERTEXTINTED_SOLID_ZBUFFERED,                // a zbuffered solid material that supports per vertex tint
		SCENESYSTEMMAT_VERTEXTINTED_SOLID_UNZBUFFERED,              // a non-zbuffered solid material that supports per vertex tint

		SCENESYSTEMMAT_VERTEXTINTED_TRANSLUCENT_ZBUFFERED,          // a zbuffered translucent material that supports per vertex tint
		SCENESYSTEMMAT_VERTEXTINTED_TRANSLUCENT_UNZBUFFERED,        // a non-zbuffered translucent material that supports per vertex tint

		SCENESYSTEMMAT_REFLECTIVITY_90,                         // a simple material for world surfaces

		SCENESYSTEMMAT_GENERALFILTER,                           // the material used by the filtering functions in sceneutils
		SCENESYSTEMMAT_OCCLUDER_DEPTH_OVERLAY,                  // The material for showing the rasterized occluder depth debug overlay
		SCENESYSTEMMAT_OCCLUDER_VISUALIZATION,                  // The material for showing occluder geometry

		SCENESYSTEMMAT_COUNT
	}
}
