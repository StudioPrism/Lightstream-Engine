namespace Sandbox;

[Expose]
public enum MultisampleAmount
{
	Multisample2x = 0,      //< 2x multisampling
	Multisample4x = 1,      //< 4x multisampling
	Multisample6x = 2,      //< 6x multisampling
	Multisample8x = 3,      //< 8x multisampling
	Multisample16x = 4,     //< 16x multisampling

	[Hide]
	MultisampleScreen = 5,  //< Use the same multisampling as the screen/frame buffer
	MultisampleNone = 6,    //< No multisampling
};
