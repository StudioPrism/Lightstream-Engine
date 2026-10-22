using Sandbox;
using System;
using System.Reflection;
namespace Editor;


internal static partial class EntityParser
{
	internal static MapClass ParseType( Type type )
	{
		// Sanity check, should never happen
		if ( !type.HasBaseType( "Editor.MapEditor.EntityDefinitions.HammerEntityDefinition" ) && !type.HasAttribute( typeof( PathNodeAttribute ) ) )
		{
			throw new ArgumentException( $"Unsupported Hammer Entity type: {type}" );
		}

		//
		// I really hate how Point/Solid are completely different class types...
		// We can simplify this in the future most likely
		//
		var classType = GameDataClassType.GenericPointClass;
		if ( type.HasAttribute( typeof( SolidAttribute ) ) )
			classType = GameDataClassType.GenericSolidClass;
		else if ( type.HasAttribute( typeof( PathAttribute ) ) )
			classType = GameDataClassType.PathClass;
		else if ( type.HasAttribute( typeof( PathNodeAttribute ) ) )
			classType = GameDataClassType.PathNodeClass;

		var displayInfo = DisplayInfo.ForMember( type );

		var mapClass = new MapClass( displayInfo.ClassName )
		{
			Type = type,
			ClassType = classType,
			DisplayName = displayInfo.Name,
			Description = displayInfo.Description,
			Icon = displayInfo.Icon,
			Category = displayInfo.Group,
		};

		AddBaseProperties( mapClass );
		ParseProperties( type, mapClass );

		ParseAttributes( type, mapClass );

		return mapClass;
	}

	internal static void ParseAttributes( Type type, MapClass mapClass )
	{
		foreach ( var meta in type.GetCustomAttributes<MetaDataAttribute>() )
		{
			meta.AddHelpers( mapClass.EditorHelpers );
			meta.AddTags( mapClass.Tags );
			meta.AddMetaData( mapClass.Metadata );
		}

		//
		// This can all be shoved into EditorModelAttribute most likely
		//

		var editorModelAttr = type.GetCustomAttribute<EditorModelAttribute>();
		if ( editorModelAttr != null )
		{
			List<string> parameters = new()
			{
				editorModelAttr.Model.Replace( ".vmdl_c", ".vmdl" )
			};

			if ( editorModelAttr.CastShadows ) parameters.Add( "castshadows" );
			if ( editorModelAttr.FixedBounds ) parameters.Add( "fixedbounds" );
			if ( editorModelAttr.StaticColor != Color.White )
			{
				var clrS = editorModelAttr.StaticColor.ToColor32();
				var clrD = editorModelAttr.DynamicColor.ToColor32();

				parameters.Add( "lightModeTint" );
				parameters.Add( $"{clrS.r} {clrS.g} {clrS.b}" );
				parameters.Add( $"{clrD.r} {clrD.g} {clrD.b}" );
			}

			mapClass.EditorHelpers.Add( new Tuple<string, string[]>( "editormodel", parameters.ToArray() ) );
		}

		//
		// Anything that adds variables we take care of here, we shouldn't be exposing this to public API
		// This is mainly just engine glue
		//

		var modelAttr = type.GetCustomAttribute<ModelAttribute>();
		if ( modelAttr != null )
		{
			mapClass.Variables.Add( new MapClassVariable()
			{
				Name = "model",
				LongName = "World Model",
				Description = "The model this entity should use.",
				DefaultValue = modelAttr.Model,
				PropertyType = typeof( Model ),
				Metadata = new() { { "hide_when_solid", "true" }, { "report", "true" } }
			} );

			mapClass.Variables.Add( new MapClassVariable()
			{
				Name = "skin",
				LongName = "Skin",
				GroupName = "Rendering",
				Description = "Some models have multiple versions of their textures, called skins.",
				DefaultValue = modelAttr.MaterialGroup,
				PropertyType = typeof( string ),
				PropertyTypeOverride = "materialgroup",
				Metadata = new() { { "hide_when_solid", "true" } }
			} );

			mapClass.Variables.Add( new MapClassVariable()
			{
				Name = "bodygroups",
				LongName = "Body Groups",
				GroupName = "Rendering",
				Description = "Some models have multiple variations of certain items, such as characters having different hair styles, etc.",
				DefaultValue = modelAttr.MaterialGroup,
				PropertyType = typeof( string ),
				PropertyTypeOverride = "bodygroupchoices",
				Metadata = new() { { "hide_when_solid", "true" } }
			} );

			mapClass.EditorHelpers.Add( new Tuple<string, string[]>( "model", new string[] { } ) );

			if ( modelAttr.Archetypes > 0 )
			{
				var archetypes = new List<string>();
				foreach ( ModelArchetype archetype in Enum.GetValues( typeof( ModelArchetype ) ) )
				{
					if ( (modelAttr.Archetypes & archetype) != 0 )
					{
						archetypes.Add( archetype.ToString() );
					}
				}

				mapClass.Metadata["model_archetypes"] = archetypes.ToArray();
			}
		}

		if ( type.HasAttribute( typeof( CanBeClientsideOnlyAttribute ) ) )
		{
			mapClass.Variables.Add( new MapClassVariable()
			{
				Name = "clientSideEntity",
				LongName = "Create as client-only entity",
				GroupName = "Miscellaneous",
				Description = "If set, the entity will spawn on client only.",
				DefaultValue = false,
				PropertyType = typeof( bool )
			} );
		}

		if ( type.HasAttribute( typeof( PhysicsSimulatedAttribute ) ) )
		{
			mapClass.Tags.Add( "PhysicsSimulated" );

			mapClass.Variables.Add( new MapClassVariable()
			{
				Name = "skipPreSettle",
				LongName = "Skip pre-settle",
				GroupName = "Physics",
				Description = "If set this entity will not particpate in the physics pre-settle during compile, but will start awake in game.",
				DefaultValue = false,
				PropertyType = typeof( bool )
			} );
		}

		if ( type.HasAttribute( typeof( RenderFieldsAttribute ) ) )
		{
			mapClass.Variables.Add( new MapClassVariable()
			{
				Name = "rendercolor",
				LongName = "Color (R G B A)",
				Description = "The color tint of this entity.",
				DefaultValue = Color.White,
				PropertyType = typeof( Color ),
				Metadata = new() { { "alpha", "true" } }
			} );
		}
	}
}
