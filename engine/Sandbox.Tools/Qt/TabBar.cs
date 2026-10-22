using System;

namespace Editor
{
	public class TabBar : Widget
	{
		Native.QTabBar _tabbar;

		internal TabBar( Native.QTabBar widget ) : base( false )
		{
			NativeInit( widget );
		}

		internal override void NativeInit( IntPtr ptr )
		{
			_tabbar = ptr;

			base.NativeInit( ptr );
		}

		internal override void NativeShutdown()
		{
			base.NativeShutdown();

			_tabbar = default;
		}
	}
}
