

namespace Sandbox.Razor
{
	[System.Obsolete]
	public class RenderTreeBuilderOld
	{

	}
}


#if false

///
/// Mocking up this so that intellisense will find it and think that everything is okay
///
namespace Microsoft.AspNetCore.Components
{

	/// <summary>
	/// Associates an event argument type with an event attribute name.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class, AllowMultiple = true, Inherited = true )]
	public sealed class EventHandlerAttribute : Attribute
	{
		/// <summary>
		/// Constructs an instance of <see cref="EventHandlerAttribute"/>.
		/// </summary>
		/// <param name="attributeName"></param>
		/// <param name="eventArgsType"></param>
		public EventHandlerAttribute( string attributeName, Type eventArgsType ) : this( attributeName, eventArgsType, false, false )
		{
		}

		/// <summary>
		/// Constructs an instance of <see cref="EventHandlerAttribute"/>.
		/// </summary>
		/// <param name="attributeName"></param>
		/// <param name="eventArgsType"></param>
		/// <param name="enableStopPropagation"></param>
		/// <param name="enablePreventDefault"></param>
		public EventHandlerAttribute( string attributeName, Type eventArgsType, bool enableStopPropagation, bool enablePreventDefault )
		{
			if ( attributeName == null )
			{
				throw new ArgumentNullException( nameof( attributeName ) );
			}

			if ( eventArgsType == null )
			{
				throw new ArgumentNullException( nameof( eventArgsType ) );
			}

			AttributeName = attributeName;
			EventArgsType = eventArgsType;
			EnableStopPropagation = enableStopPropagation;
			EnablePreventDefault = enablePreventDefault;
		}

		/// <summary>
		/// Gets the attribute name.
		/// </summary>
		public string AttributeName { get; }

		/// <summary>
		/// Gets the event argument type.
		/// </summary>
		public Type EventArgsType { get; }

		/// <summary>
		/// Gets the event's ability to stop propagation.
		/// </summary>
		public bool EnableStopPropagation { get; }

		/// <summary>
		/// Gets the event's ability to prevent default event flow.
		/// </summary>
		public bool EnablePreventDefault { get; }
	}

	public class RouteAttribute :  System.Attribute
	{
		public RouteAttribute( string url )
		{

		}
	}

	public class ComponentBase : Panel
	{

	}

	namespace Rendering
	{
		public class RenderTreeBuilder : RenderTree
		{
			internal RenderTreeBuilder() : base ( null )
			{

			}
		}
	}
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.AspNetCore.Components.Web
{

	// Media events
	[EventHandler( "oncanplay", typeof( EventArgs ), true, true )]
	[EventHandler( "oncanplaythrough", typeof( EventArgs ), true, true )]
	[EventHandler( "oncuechange", typeof( EventArgs ), true, true )]
	[EventHandler( "ondurationchange", typeof( EventArgs ), true, true )]
	[EventHandler( "onemptied", typeof( EventArgs ), true, true )]
	[EventHandler( "onpause", typeof( EventArgs ), true, true )]
	[EventHandler( "onplay", typeof( EventArgs ), true, true )]
	[EventHandler( "onplaying", typeof( EventArgs ), true, true )]
	[EventHandler( "onratechange", typeof( EventArgs ), true, true )]
	[EventHandler( "onseeked", typeof( EventArgs ), true, true )]
	[EventHandler( "onseeking", typeof( EventArgs ), true, true )]
	[EventHandler( "onstalled", typeof( EventArgs ), true, true )]
	[EventHandler( "onstop", typeof( EventArgs ), true, true )]
	[EventHandler( "onsuspend", typeof( EventArgs ), true, true )]
	[EventHandler( "ontimeupdate", typeof( EventArgs ), true, true )]
	[EventHandler( "onvolumechange", typeof( EventArgs ), true, true )]
	[EventHandler( "onwaiting", typeof( EventArgs ), true, true )]

	// General events
	[EventHandler( "onactivate", typeof( EventArgs ), true, true )]
	[EventHandler( "onbeforeactivate", typeof( EventArgs ), true, true )]
	[EventHandler( "onbeforedeactivate", typeof( EventArgs ), true, true )]
	[EventHandler( "ondeactivate", typeof( EventArgs ), true, true )]
	[EventHandler( "onended", typeof( EventArgs ), true, true )]
	[EventHandler( "onfullscreenchange", typeof( EventArgs ), true, true )]
	[EventHandler( "onfullscreenerror", typeof( EventArgs ), true, true )]
	[EventHandler( "onloadeddata", typeof( EventArgs ), true, true )]
	[EventHandler( "onloadedmetadata", typeof( EventArgs ), true, true )]
	[EventHandler( "onpointerlockchange", typeof( EventArgs ), true, true )]
	[EventHandler( "onpointerlockerror", typeof( EventArgs ), true, true )]
	[EventHandler( "onreadystatechange", typeof( EventArgs ), true, true )]
	[EventHandler( "onscroll", typeof( EventArgs ), true, true )]
	[EventHandler( "onfuckedinthehead", typeof( EventArgs ), true, true )]

	[EventHandler( "ontoggle", typeof( EventArgs ), true, true )]
	public static class EventHandlers
	{
	}

}




#endif
