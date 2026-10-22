using Qt;
using System;

namespace Editor;

/// <summary>
/// Splitter that can be linked to move alongside others
/// </summary>
public class LinkableSplitter : Splitter
{
	internal Native.CQLinkableSplitter _linkableSplitter;

	public LinkableSplitter( Orientation orientation, Widget parent )
	{
		var widget = Native.CQLinkableSplitter.CreateSplitter( orientation, parent?._widget ?? default );
		NativeInit( widget );
	}

	internal override void NativeInit( IntPtr ptr )
	{
		_linkableSplitter = ptr;

		base.NativeInit( ptr );
	}

	internal override void NativeShutdown()
	{
		_frame = default;

		base.NativeShutdown();
	}

	public void LinkWith( LinkableSplitter other )
	{
		_linkableSplitter.LinkWith( other._linkableSplitter );
	}
}
