using Editor.MapDoc;
using Editor.MapEditor;
using System;

namespace Sandbox;

static class HandleTypes
{
	internal static uint MapDocument { get; } = StringToken.FindOrCreate( "MapDocument" );
	internal static uint MapGroup { get; } = StringToken.FindOrCreate( "MapGroup" );
	internal static uint MapInstance { get; } = StringToken.FindOrCreate( "MapInstance" );
	internal static uint MapNode { get; } = StringToken.FindOrCreate( "MapNode" );
	internal static uint MapGameObject { get; } = StringToken.FindOrCreate( "MapGameObject" );
	internal static uint MapEntity { get; } = StringToken.FindOrCreate( "MapEntity" );
	internal static uint MapMesh { get; } = StringToken.FindOrCreate( "MapMesh" );
	internal static uint MapStaticOverlay { get; } = StringToken.FindOrCreate( "MapStaticOverlay" );
	internal static uint MapPath { get; } = StringToken.FindOrCreate( "MapPath" );
	internal static uint MapPathNode { get; } = StringToken.FindOrCreate( "MapPathNode" );
	internal static uint MapView { get; } = StringToken.FindOrCreate( "MapView" );
	internal static uint MapWorld { get; } = StringToken.FindOrCreate( "MapWorld" );

	public static int RegisterHandle( IntPtr ptr, uint type )
	{
		if ( type == HandleTypes.MapDocument ) return HandleIndex.New<MapDocument>( ptr, ( h ) => new MapDocument( h ) );
		if ( type == HandleTypes.MapGroup ) return HandleIndex.New<MapGroup>( ptr, ( h ) => new MapGroup( h ) );
		if ( type == HandleTypes.MapInstance ) return HandleIndex.New<Editor.MapDoc.MapInstance>( ptr, ( h ) => new Editor.MapDoc.MapInstance( h ) );
		if ( type == HandleTypes.MapNode ) return HandleIndex.New<MapNode>( ptr, ( h ) => new MapNode( h ) );
		if ( type == HandleTypes.MapGameObject ) return HandleIndex.New<MapGameObject>( ptr, ( h ) => new MapGameObject( h ) );
		if ( type == HandleTypes.MapEntity ) return HandleIndex.New<MapEntity>( ptr, ( h ) => new MapEntity( h ) );
		if ( type == HandleTypes.MapMesh ) return HandleIndex.New<MapMesh>( ptr, ( h ) => new MapMesh( h ) );
		if ( type == HandleTypes.MapPath ) return HandleIndex.New<MapPath>( ptr, ( h ) => new MapPath( h ) );
		if ( type == HandleTypes.MapPathNode ) return HandleIndex.New<MapPathNode>( ptr, ( h ) => new MapPathNode( h ) );
		if ( type == HandleTypes.MapStaticOverlay ) return HandleIndex.New<MapStaticOverlay>( ptr, ( h ) => new MapStaticOverlay( h ) );
		if ( type == HandleTypes.MapView ) return HandleIndex.New<MapView>( ptr, ( h ) => new MapView( h ) );
		if ( type == HandleTypes.MapWorld ) return HandleIndex.New<MapWorld>( ptr, ( h ) => new MapWorld( h ) );

		return -1;
	}
}
