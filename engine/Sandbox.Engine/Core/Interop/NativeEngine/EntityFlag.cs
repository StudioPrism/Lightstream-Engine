namespace Sandbox
{
	internal enum EntityFlag
	{
		FL_FAKECLIENT = (1 << 8),// Fake client, simulated server side; don't send network messages to them
	}
}
