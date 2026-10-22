using Sandbox;
using System;

namespace Sandbox
{
	/// <summary>
	/// Default model archetypes.
	/// These types are defined in "tools/model_archetypes.txt".
	/// </summary>
	public enum ModelArchetype
	{
		/// <summary>
		/// A static model. It can still have collisions, but they do not have physics.
		/// </summary>
		static_prop_model = 1,

		/// <summary>
		/// Animated model. Typically no physics.
		/// </summary>
		animated_model = 2,

		/// <summary>
		/// A generic physics enabled model.
		/// </summary>
		physics_prop_model = 4,

		/// <summary>
		/// A ragdoll type model.
		/// </summary>
		jointed_physics_model = 8,

		/// <summary>
		/// A physics model that can be broken into other physics models.
		/// </summary>
		breakable_prop_model = 16,

		/// <summary>
		/// A generic actor/NPC model.
		/// </summary>
		generic_actor_model = 32
	}
}

namespace Editor
{
	/// <summary>
	/// This is an entity that can be placed in Hammer.
	/// </summary>
	internal class HammerEntityAttribute : Attribute, IUninheritable
	{
	}

	/// <summary>
	/// This is a brush based entity class. It can only be a mesh tied to an entity.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	internal class SolidAttribute : Attribute
	{
	}

	/// <summary>
	/// This is a point class entity, but does support being a brush entity (a mesh tied to an entity).
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	internal class SupportsSolidAttribute : MetaDataAttribute
	{
		public override void AddTags( List<string> tags )
		{
			tags.Add( "SupportsSolids" );
		}
	}

	/// <summary>
	/// Marks this entity as a physics constraint.
	/// This disables pre-settle for all <see cref="PhysicsSimulatedAttribute">PhysicsSimulated</see> entities this entity's keyvalues reference in Hammer.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	internal class PhysicsConstraintAttribute : MetaDataAttribute
	{
		public override void AddTags( List<string> tags )
		{
			tags.Add( "PhysicsConstraint" );
		}
	}

	/// <summary>
	/// This is a path class, used with Hammer's Path Tool.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	internal class PathAttribute : MetaDataAttribute
	{
		string NodeClassName;
		bool SpawnEntities;

		/// <param name="nodeClassName">Class name of the node entity.</param>
		/// <param name="spawnEnts">If set to true, will actually create node entities. If set to false, node data will be serialized to a JSON key-value.</param>
		public PathAttribute( string nodeClassName = null, bool spawnEnts = false )
		{
			NodeClassName = nodeClassName;
			SpawnEntities = spawnEnts;
		}

		public override void AddMetaData( Dictionary<string, object> meta_data )
		{
			if ( !string.IsNullOrEmpty( NodeClassName ) ) meta_data["node_entity_class"] = NodeClassName;
			if ( SpawnEntities ) meta_data["spawn_node_entities"] = true;
		}
	}

	/// <summary>
	/// This is a path node class. May not necessarily be an entity.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	internal class PathNodeAttribute : Attribute
	{
	}

	/// <summary>
	/// Apply this material to the mesh when tying one to this class. Typically used for triggers.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	internal class AutoApplyMaterialAttribute : MetaDataAttribute
	{
		public string MaterialName { get; set; }

		public AutoApplyMaterialAttribute( string materialName = "materials/tools/toolstrigger.vmat" )
		{
			MaterialName = materialName;
		}

		public override void AddMetaData( Dictionary<string, object> meta_data )
		{
			meta_data["auto_apply_material"] = MaterialName;
		}
	}

	/// <summary>
	/// This makes it so the model, skin and bodygroups can be set and changed in Hammer.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	internal class ModelAttribute : Attribute
	{
		/// <summary>
		/// The default model to be set to.
		/// </summary>
		public string Model { get; set; } = "";

		/// <summary>
		/// The default body group to be set to.
		/// </summary>
		public string BodyGroup { get; set; } = "";

		/// <summary>
		/// The default material group to be set to.
		/// </summary>
		public string MaterialGroup { get; set; } = "default";

		/// <summary>
		/// Marks this entity as a representative of a certain model archetype.
		/// This makes this entity class appear in ModelDoc under given archetype(s), which will be used to decide which entity class to use when dragging models from Hammer's Asset browser.
		/// </summary>
		public ModelArchetype Archetypes { get; set; } = 0;
	}



	/// <summary>
	/// Declare a sprite to represent this entity in Hammer.
	/// </summary>
	/// <example>
	/// [EditorSprite( "editor/ai_goal_follow.vmat" )]
	/// </example>
	[AttributeUsage( AttributeTargets.Class )]
	internal class EditorSpriteAttribute : MetaDataAttribute
	{
		public string Sprite { get; set; }

		public EditorSpriteAttribute( string sprite )
		{
			Sprite = sprite;
		}

		public override void AddHelpers( List<Tuple<string, string[]>> helpers )
		{
			helpers.Add( Tuple.Create( "iconsprite", new string[] { Sprite } ) );
		}
	}

	/// <summary>
	/// Tells Hammer that this entity has a particle effect keyvalue that needs to be visualized.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	internal class ParticleAttribute : MetaDataAttribute
	{
		string Particle { get; set; }

		public ParticleAttribute( string particleKV = null )
		{
			Particle = particleKV;
		}

		public override void AddHelpers( List<Tuple<string, string[]>> helpers )
		{
			List<string> list = new List<string>();
			if ( !string.IsNullOrEmpty( Particle ) ) list.Add( Particle );

			helpers.Add( Tuple.Create( "particle", list.ToArray() ) );
		}
	}

	/// <summary>
	/// Indicate to the map builder that any meshes associated with the entity should have a mesh physics type.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	internal class PhysicsTypeOverrideMeshAttribute : MetaDataAttribute
	{
		public override void AddTags( List<string> tags )
		{
			tags.Add( "PhysicsTypeOverride_Mesh" );
		}
	}

	/// <summary>
	/// Indicate if the entity is simulated in game and should participate in the pre-settle simulation during map compile.
	/// Adds a pre-settle keyvalue to this entity class.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	internal class PhysicsSimulatedAttribute : Attribute
	{

	}

	/// <summary>
	/// Helper to render skybox in hammer
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	internal class SkyboxAttribute : MetaDataAttribute
	{
		public override void AddHelpers( List<Tuple<string, string[]>> helpers )
		{
			helpers.Add( Tuple.Create( "skybox", new string[] { } ) );
		}
	}

	/// <summary>
	/// Used to tell Hammer which automatic Visibility Groups an entity should belong to. See <see cref="VisGroupAttribute">VisGroupAttribute</see>.
	/// </summary>
	internal enum VisGroup
	{
		/// <summary>
		/// Entities that are primarily lights and that sort of thing.
		/// </summary>
		Lighting,

		/// <summary>
		/// The purpose of these entities is to emit light and not much else.
		/// </summary>
		Sound,

		/// <summary>
		/// Pure logic entities, typically not shown in-game.
		/// </summary>
		Logic,

		/// <summary>
		/// Any sort of trigger volume, these usually don't show up in-game.
		/// </summary>
		Trigger,

		/// <summary>
		/// Entities that are related to nav meshes.
		/// </summary>
		Navigation,

		/// <summary>
		/// The main reason these exist is to create particle systems.
		/// </summary>
		Particles,

		/// <summary>
		/// Physics enabled entities.
		/// </summary>
		Physics,

		/// <summary>
		/// Entities that do not move via physics but are still intractable with or otherwise non static.
		/// </summary>
		Dynamic
	}

	/// <summary>
	/// Makes the entity show up under given automatic visibility group in Hammer.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class, AllowMultiple = true )]
	internal class VisGroupAttribute : MetaDataAttribute
	{
		public VisGroup Group { get; set; }

		public VisGroupAttribute( VisGroup group )
		{
			Group = group;
		}

		public override void AddTags( List<string> tags )
		{
			tags.Add( Group.ToString() );
		}
	}

	/// <summary>
	/// Draws the movement direction in Hammer.
	/// </summary>
	/// <example>
	/// [DrawAngles( "movedir", "movedir_islocal" )]
	/// </example>
	[AttributeUsage( AttributeTargets.Class )]
	internal class DrawAnglesAttribute : MetaDataAttribute
	{
		public string AngleKV { get; set; }
		public string IsLocalKV { get; set; }

		public DrawAnglesAttribute()
		{
		}

		public DrawAnglesAttribute( string angleKV, string isLocalKV = null )
		{
			AngleKV = angleKV;
			IsLocalKV = isLocalKV;
		}

		public override void AddHelpers( List<Tuple<string, string[]>> helpers )
		{
			var parameters = new List<string>();
			if ( !string.IsNullOrEmpty( AngleKV ) ) parameters.Add( AngleKV );
			if ( !string.IsNullOrEmpty( IsLocalKV ) ) parameters.Add( IsLocalKV );
			helpers.Add( Tuple.Create( "drawangles", parameters.ToArray() ) );
		}
	}

	/// <summary>
	/// Draws the door movement and the final open position in Hammer.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	internal class DoorHelperAttribute : MetaDataAttribute
	{
		public string AngleKV { get; set; }
		public string IsLocalKV { get; set; }
		public string AngleTypeKV { get; set; }
		public string DistanceKV { get; set; }

		public DoorHelperAttribute( string angleKV, string isLocalKV, string angleTypeKV, string distanceKV )
		{
			AngleKV = angleKV;
			IsLocalKV = isLocalKV;
			AngleTypeKV = angleTypeKV;
			DistanceKV = distanceKV;
		}

		public override void AddHelpers( List<Tuple<string, string[]>> helpers )
		{
			helpers.Add( Tuple.Create( "door_helper", new string[] { AngleKV, IsLocalKV, AngleTypeKV, DistanceKV } ) );
		}
	}

	/// <summary>
	/// Adds the render color and other related options to the entity class in Hammer.
	/// </summary>
	/// <example>
	/// [RenderFields]
	/// </example>
	[AttributeUsage( AttributeTargets.Class )]
	internal class RenderFieldsAttribute : Attribute
	{

	}

	/// <summary>
	/// Draws a frustum that doesn't contribute to bounds calculations.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	internal class FrustumBoundlessAttribute : MetaDataAttribute
	{
		public string FovKV { get; set; }
		public string ZNearKV { get; set; }
		public string ZFarKV { get; set; }

		public FrustumBoundlessAttribute( string fovKV, string zNearKV, string zFarKV )
		{
			FovKV = fovKV;
			ZNearKV = zNearKV;
			ZFarKV = zFarKV;
		}

		public override void AddHelpers( List<Tuple<string, string[]>> helpers )
		{
			helpers.Add( Tuple.Create( "frustum_boundless", new string[] { FovKV, ZNearKV, ZFarKV } ) );
		}
	}

	/// <summary>
	/// Displays a sphere in Hammer with a radius tied to given property and with given color.
	/// The sphere's radius can be manipulated in Hammer's 2D views. You can have multiple of these.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class, AllowMultiple = true )]
	internal class SphereAttribute : MetaDataAttribute
	{
		public string VariableName { get; set; }
		public Color32 Color { get; set; }
		public bool SingleSelect { get; set; }

		/// <summary>
		/// If set to true, the sphere will appear as 3 circles in 3D view, rather than a wireframe sphere.
		/// </summary>
		public bool IsLean { get; set; }

		/// <param name="variableName">Name of the variable to use as sphere radius.</param>
		/// <param name="color">Color as an unsigned integer. For example 0xFF99CC, where 0xBBGGRR.</param>
		/// <param name="singleSelect">If this helper should show up when only 1 object is selected in Hammer.</param>
		public SphereAttribute( string variableName, uint color, bool singleSelect = false )
		{
			VariableName = variableName;
			Color = new Color32( color ) { a = 255 };
			SingleSelect = singleSelect;
		}

		/// <param name="variableName">Name of the variable to use as sphere radius.</param>
		/// <param name="color">Color as an unsigned integer. For example 0xFF99CC, where 0xBBGGRR.</param>
		/// <param name="singleSelect">If this helper should show up when only 1 object is selected in Hammer.</param>
		public SphereAttribute( string variableName, string color, bool singleSelect = false )
		{
			VariableName = variableName;
			Color = global::Color.Parse( color ) ?? global::Color.White;
			SingleSelect = singleSelect;
		}

		/// <param name="variableName">Name of the variable to use as sphere radius.</param>
		/// <param name="red">Red component of the sphere's color.</param>
		/// <param name="green">Green component of the sphere's color.</param>
		/// <param name="blue">Blue component of the sphere's color.</param>
		/// <param name="singleSelect">If this helper should show up when only 1 object is selected in Hammer.</param>
		public SphereAttribute( string variableName = "radius", byte red = 255, byte green = 255, byte blue = 255, bool singleSelect = false )
		{
			VariableName = variableName;
			Color = new Color32( red, green, blue );
			SingleSelect = singleSelect;
		}

		/// <param name="radius">Range of the sphere to show.</param>
		/// <param name="red">Red component of the sphere's color.</param>
		/// <param name="green">Green component of the sphere's color.</param>
		/// <param name="blue">Blue component of the sphere's color.</param>
		/// <param name="singleSelect">If this helper should show up when only 1 object is selected in Hammer.</param>
		public SphereAttribute( float radius, byte red = 255, byte green = 255, byte blue = 255, bool singleSelect = false )
		{
			VariableName = $"{radius}"; // A bit cheaty
			Color = new Color32( red, green, blue );
			SingleSelect = singleSelect;
		}

		public override void AddHelpers( List<Tuple<string, string[]>> helpers )
		{
			var parameters = new List<string>();
			parameters.Add( VariableName );

			if ( Color != Color32.White || SingleSelect )
			{
				parameters.Add( $"{Color.r}" );
				parameters.Add( $"{Color.g}" );
				parameters.Add( $"{Color.b}" );
			}

			if ( SingleSelect ) parameters.Add( "singleSelect" );

			helpers.Add( Tuple.Create( IsLean ? "leansphere" : "sphere", parameters.ToArray() ) );
		}
	}

	/// <summary>
	/// Displays text in Hammer on the entity.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	internal class TextAttribute : MetaDataAttribute
	{
		public string OffsetVariable { get; set; }
		public string Text { get; set; }
		public bool Local { get; set; }

		/// <param name="text">The text to display.</param>
		/// <param name="offsetVariable">The name of the property that will act as the position of the text.</param>
		/// <param name="worldspace">Whether the position from the variable should be interpreted in world space (true) or in local space (false).</param>
		public TextAttribute( string text = "", string offsetVariable = "origin", bool worldspace = false )
		{
			OffsetVariable = offsetVariable;
			Text = text;
			Local = !worldspace;
		}

		public override void AddHelpers( List<Tuple<string, string[]>> helpers )
		{
			var parameters = new List<string>();
			parameters.Add( OffsetVariable );
			if ( !string.IsNullOrEmpty( Text ) ) parameters.Add( Text );

			helpers.Add( Tuple.Create( Local ? "text_local" : "text", parameters.ToArray() ) );
		}
	}

	/// <summary>
	/// Draws a line in Hammer. You can have multiple of this attribute.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class, AllowMultiple = true )]
	internal class LineAttribute : MetaDataAttribute
	{
		bool selectedOnly;
		string color;

		string startKey;
		string startKeyValue;
		string endKey;
		string endKeyValue;

		/// <summary>
		/// Draws lines between this entity and all entities which have a key named '<paramref name="startKey">startKey</paramref>' and its value matches
		/// the value of our '<paramref name="startKeyValue">startKeyValue</paramref>'.
		/// </summary>
		/// <param name="startKey">Name of the key to search on other entities. This typically will be 'targetname'.</param>
		/// <param name="startKeyValue">Name of our key whose value will be used to match other entities.</param>
		/// <param name="onlySelected">Only draw the line when the entity is selected.</param>
		public LineAttribute( string startKey, string startKeyValue, bool onlySelected = false/*, byte red = 255, byte green = 255, byte blue = 255*/ )
		{
			// The color is only shown in 2D views when onlySelected is enabled, so fuck it
			color = "255 255 255"; //$"{red} {green} {blue}";
			selectedOnly = onlySelected;
			this.startKey = startKey;
			this.startKeyValue = startKeyValue;
		}

		/// <summary>
		/// Draws lines between all entities, starting from each entity that has a key named '<paramref name="startKey">startKey</paramref>' and its value matches
		/// the value of our '<paramref name="startKeyValue">startKeyValue</paramref>' and going to each entity that has a key named <paramref name="endKey">endKey</paramref>
		/// with a value of '<paramref name="endKeyValue">endKeyValue</paramref>'s value.
		/// </summary>
		/// <param name="startKey">Name of the key to search on other entities. This typically will be 'targetname'.</param>
		/// <param name="startKeyValue">Name of our key whose value will be used to match other entities.</param>
		/// <param name="endKey">Name of the key to search on other entities.</param>
		/// <param name="endKeyValue">Name of our key whose value will be used to match other entities.</param>
		/// <param name="onlySelected">Only draw the line when the entity is selected.</param>
		public LineAttribute( string startKey, string startKeyValue, string endKey, string endKeyValue, bool onlySelected = false ) : this( startKey, startKeyValue, onlySelected )
		{
			this.endKey = endKey;
			this.endKeyValue = endKeyValue;
		}

		public override void AddHelpers( List<Tuple<string, string[]>> helpers )
		{
			var name = "line";
			if ( selectedOnly ) name = "selected_line";

			var args = new List<string>
			{
				color,
				startKey,
				startKeyValue
			};
			if ( !string.IsNullOrEmpty( endKey ) && !string.IsNullOrEmpty( endKeyValue ) )
			{
				args.Add( endKey );
				args.Add( endKeyValue );
			}

			helpers.Add( Tuple.Create( name, args.ToArray() ) );
		}
	}

	/// <summary>
	/// Draws a line in Hammer from the entity's origin to a point that can be moved with a gizmo and is stored in this property.
	/// </summary>
	[AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
	internal class PointLineAttribute : Attribute
	{
		/// <summary>
		/// Write local to entity coordinates to the provided key. Default is to write in world space.
		/// </summary>
		public bool Local { get; set; }

		public PointLineAttribute()
		{
		}
	}

	/// <summary>
	/// For point entities without visualization (model/sprite), sets the size of the box the entity will appear as in Hammer.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	internal class BoxSizeAttribute : MetaDataAttribute
	{
		public string Mins { get; set; }
		public string Maxs { get; set; }

		public BoxSizeAttribute( float size )
		{
			Mins = $"{-size} {-size} {-size}";
			Maxs = $"{size} {size} {size}";
		}

		public BoxSizeAttribute( float minsX, float minsY, float minsZ )
		{
			Mins = $"{minsX} {minsY} {minsZ}";
			Maxs = "";
		}

		public BoxSizeAttribute( float minsX, float minsY, float minsZ, float maxsX, float maxsY, float maxsZ )
		{
			Mins = $"{minsX} {minsY} {minsZ}";
			Maxs = $"{maxsX} {maxsY} {maxsZ}";
		}

		public override void AddHelpers( List<Tuple<string, string[]>> helpers )
		{
			helpers.Add( Tuple.Create( "size", new string[] { Mins, Maxs } ) );
		}
	}

	/// <summary>
	/// Creates a resizable box helper in Hammer which outputs the size of the bounding box defined by the level designer into given keys/properties.
	/// You can have multiple of this attribute.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class, AllowMultiple = true )]
	internal class BoundsHelperAttribute : MetaDataAttribute
	{
		/// <summary>
		/// Key (classname) of the entity to store the "Mins" of the bounding box.
		/// </summary>
		public string MinsKey { get; set; }

		/// <summary>
		/// Key (classname) of the entity to store the "Maxs" of the bounding box.
		/// </summary>
		public string MaxsKey { get; set; }

		/// <summary>
		/// Key (classname) of the entity to store the bounding box as an "extents".
		/// This replaces <see cref="MinsKey"/> and <see cref="MaxsKey"/> and assumes the entity is in the middle of the bounds.
		/// The output value will be the total size of the bounds on each axis.
		/// </summary>
		public string ExtentsKey { get; set; }

		/// <summary>
		/// Always move the entity to the center of the bounds.
		/// </summary>
		public bool AutoCenter { get; set; }

		/// <summary>
		/// Make the bounds AABB (true), not OBB (false). Basically ignores rotation.
		/// </summary>
		public bool IsWorldAligned { get; set; }

		/// <summary>
		/// Creates a box helper that outputs the size of the bounding box defined by the level designer as mins and maxs
		/// </summary>
		/// <param name="minsKey">The internal key name to output "mins" size to.</param>
		/// <param name="maxsKey">The internal key name to output "maxs" size to.</param>
		/// <param name="autoCenter">If set to true, editing this box in Hammer will automatically move the entity to the center of the box.</param>
		/// <param name="worldAliged">If set, the helper box will ignore entity rotation.</param>
		public BoundsHelperAttribute( string minsKey, string maxsKey, bool autoCenter = false, bool worldAliged = false )
		{
			MinsKey = minsKey;
			MaxsKey = maxsKey;
			AutoCenter = autoCenter;
			IsWorldAligned = worldAliged;
		}

		/// <summary>
		/// Creates a box helper that outputs the size of the bounding box defined by the level designer as extents (maxs - mins).
		/// This assumes the entity is in the center of the box.
		/// </summary>
		/// <param name="extentsKey">The internal key name to output "extents" size to. This is the result of (maxs - mins).</param>
		/// <param name="worldAliged">If set, the helper box will ignore entity rotation.</param>
		public BoundsHelperAttribute( string extentsKey, bool worldAliged = false )
		{
			ExtentsKey = extentsKey;
			AutoCenter = false;
			IsWorldAligned = worldAliged;
		}

		public override void AddHelpers( List<Tuple<string, string[]>> helpers )
		{
			var type = "oriented";
			if ( IsWorldAligned ) type = "world_aligned";

			if ( !string.IsNullOrEmpty( ExtentsKey ) )
			{
				helpers.Add( Tuple.Create( $"centered_box_{type}", new string[] { ExtentsKey } ) );
				return;
			}

			var parameters = new List<string>() { MinsKey, MaxsKey };
			if ( AutoCenter ) parameters.Add( "AutoCenter" );

			helpers.Add( Tuple.Create( $"box_{type}", parameters.ToArray() ) );
		}
	}

	/// <summary>
	/// Creates a resizable box helper that represents an orthographic projection from the entity's origin in Hammer.
	/// The size of the bounding box as defined by the level designer is put into given keys/properties.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	internal class OrthoBoundsHelperAttribute : MetaDataAttribute
	{
		public string RangeKey { get; set; }
		public string WidthKey { get; set; }
		public string HeightKey { get; set; }

		public OrthoBoundsHelperAttribute( string rangeKey, string widthKey, string heightKey )
		{
			RangeKey = rangeKey;
			WidthKey = widthKey;
			HeightKey = heightKey;
		}

		public override void AddHelpers( List<Tuple<string, string[]>> helpers )
		{
			helpers.Add( Tuple.Create( "lightortho", new string[] { RangeKey, WidthKey, HeightKey } ) );
		}
	}

	/// <summary>
	/// Adds a property in Hammer that dictates whether the entity will be spawned on server or client.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	internal class CanBeClientsideOnlyAttribute : Attribute
	{
	}

	/// <summary>
	/// Makes value of this property appear in the Source File column of the Entity Report dialog in Hammer.
	/// There can be only one of such properties.
	/// </summary>
	[AttributeUsage( AttributeTargets.Property )]
	internal class EntityReportSourceAttribute : FieldMetaDataAttribute
	{
		public override void AddMetaData( Dictionary<string, string> meta_data )
		{
			meta_data["report"] = "true";
		}
	}

	#region InternalAttributes

	/// <summary>
	/// Internally marks this class in Hammer as a post processing entity for preview purposes.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	internal class PostProcessingVolumeAttribute : MetaDataAttribute
	{
		public override void AddHelpers( List<Tuple<string, string[]>> helpers )
		{
			helpers.Add( Tuple.Create( "postprocessing", new string[] { "postprocessing" } ) );
		}
	}

	/// <summary>
	/// Internally marks this class in Hammer as a tonemap entity for preview purposes.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	internal class ToneMapAttribute : MetaDataAttribute
	{
		public override void AddHelpers( List<Tuple<string, string[]>> helpers )
		{
			helpers.Add( Tuple.Create( "tonemap", new string[] { } ) );
		}
	}

	/// <summary>
	/// Internally marks this class in Hammer as a light.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	internal class LightAttribute : MetaDataAttribute
	{
		public override void AddHelpers( List<Tuple<string, string[]>> helpers )
		{
			helpers.Add( Tuple.Create( "light", new string[] { } ) );
		}
	}

	/// <summary>
	/// The light_spot visualizer.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	internal class LightConeAttribute : MetaDataAttribute
	{
		public override void AddHelpers( List<Tuple<string, string[]>> helpers )
		{
			helpers.Add( Tuple.Create( "lightcone", new string[] { } ) );
		}
	}

	/// <summary>
	/// Marks this entity as global, there should only be one entity with this global name in the map.
	/// Used internally for Preview purposes.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	internal class GlobalAttribute : MetaDataAttribute
	{
		internal string Global;

		public GlobalAttribute( string global )
		{
			Global = global;
		}

		public override void AddHelpers( List<Tuple<string, string[]>> helpers )
		{
			helpers.Add( Tuple.Create( "global", new string[] { Global } ) );
		}
	}

	/// <summary>
	/// Adds a simple parameterless helper to the entity class.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	internal class SimpleHelperAttribute : MetaDataAttribute
	{
		internal string Helper;

		public SimpleHelperAttribute( string helper )
		{
			Helper = helper;
		}

		public override void AddHelpers( List<Tuple<string, string[]>> helpers )
		{
			helpers.Add( Tuple.Create( Helper, new string[] { } ) );
		}
	}

	/// <summary>
	/// Used by light_environment entity internally.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	internal class BakeAmbientLightAttribute : MetaDataAttribute
	{
		internal string AmbientColor;

		public BakeAmbientLightAttribute( string clrKey )
		{
			AmbientColor = clrKey;
		}

		public override void AddHelpers( List<Tuple<string, string[]>> helpers )
		{
			helpers.Add( Tuple.Create( "bakeambientlight", new string[] { AmbientColor } ) );
		}
	}

	/// <summary>
	/// Used by light_environment entity internally.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	internal class BakeAmbientOcclusionAttribute : MetaDataAttribute
	{
		public override void AddHelpers( List<Tuple<string, string[]>> helpers )
		{
			helpers.Add( Tuple.Create( "bakeambientocclusion", new string[] { "ambient_occlusion", "max_occlusion_distance", "fully_occluded_fraction", "occlusion_exponent" } ) );
		}
	}

	/// <summary>
	/// Used by light_environment entity internally.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	internal class BakeSkyLightAttribute : MetaDataAttribute
	{
		public override void AddHelpers( List<Tuple<string, string[]>> helpers )
		{
			helpers.Add( Tuple.Create( "bakeskylight", new string[] { "skycolor", "skyintensity", "lower_hemisphere_is_black", "skytexture", "skytexturescale", "skyambientbounce", "sunlightminbrightness" } ) );
		}
	}

	/// <summary>
	/// Allows hammer to bake resources, mostly used for cubemaps and light probes
	/// </summary>
	[AttributeUsage( AttributeTargets.Class, AllowMultiple = true )]
	internal class BakeResourceAttribute : MetaDataAttribute
	{
		public string ResourceParameterName { get; set; }
		public string ResourceTypeName { get; set; }
		public string ResourceNamePrefix { get; set; }
		public string ToolObjectName { get; set; }

		public BakeResourceAttribute( string resourceParameterName, string resourceTypeName, string resourceNamePrefix, string toolObjectName )
		{
			ResourceParameterName = resourceParameterName;
			ResourceTypeName = resourceTypeName;
			ResourceNamePrefix = resourceNamePrefix;
			ToolObjectName = toolObjectName;
		}

		public override void AddHelpers( List<Tuple<string, string[]>> helpers )
		{
			helpers.Add( Tuple.Create( "bakeresource", new string[] { ResourceParameterName, ResourceTypeName, ResourceNamePrefix, ToolObjectName } ) );
		}
	}

	#endregion

}
