using System;
using System.Runtime.InteropServices;

namespace NativeMapDoc;

[StructLayout( LayoutKind.Sequential )]
internal struct EnumChildrenStackEntry
{
	public IntPtr Parent;
	public int CurrentChild;
}

[StructLayout( LayoutKind.Sequential )]
internal struct EnumChildrenPos
{
	//
	// This is so fucking stupid... But I can't get it to marshal as an unmanaged type otherwise
	//
	public EnumChildrenStackEntry Stack1;
	public EnumChildrenStackEntry Stack2;
	public EnumChildrenStackEntry Stack3;
	public EnumChildrenStackEntry Stack4;
	public EnumChildrenStackEntry Stack5;
	public EnumChildrenStackEntry Stack6;
	public EnumChildrenStackEntry Stack7;
	public EnumChildrenStackEntry Stack8;
	public EnumChildrenStackEntry Stack9;
	public EnumChildrenStackEntry Stack10;
	public EnumChildrenStackEntry Stack11;
	public EnumChildrenStackEntry Stack12;
	public EnumChildrenStackEntry Stack13;
	public EnumChildrenStackEntry Stack14;
	public EnumChildrenStackEntry Stack15;
	public EnumChildrenStackEntry Stack16;
	public EnumChildrenStackEntry Stack17;
	public EnumChildrenStackEntry Stack18;
	public EnumChildrenStackEntry Stack19;
	public EnumChildrenStackEntry Stack20;
	public EnumChildrenStackEntry Stack21;
	public EnumChildrenStackEntry Stack22;
	public EnumChildrenStackEntry Stack23;
	public EnumChildrenStackEntry Stack24;
	public EnumChildrenStackEntry Stack25;
	public EnumChildrenStackEntry Stack26;
	public EnumChildrenStackEntry Stack27;
	public EnumChildrenStackEntry Stack28;
	public EnumChildrenStackEntry Stack29;
	public EnumChildrenStackEntry Stack30;
	public EnumChildrenStackEntry Stack31;
	public EnumChildrenStackEntry Stack32;
	public int Depth;
	public int Order;
	public int Flags;
}
