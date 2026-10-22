internal enum sound_voice_layer_t : byte
{
	VOICE_LAYER_GAME = 0,       // These are sounds played by the game session (should transition as the game state transitions)
	VOICE_LAYER_UI = 1,         // sounds played by the UI (should transition as the UI transitions)
	VOICE_LAYER_TOOL = 2,       // sounds played outside the game by a tool.  The game should never stop or fade these because they belong to an external context
	VOICE_LAYER_ASYNC_LOAD = 3,     // sounds played inside the game meant to play during map transitions

	NUM_VOICE_LAYERS,

	VOICE_LAYER_NONE = 254,     // Implies we are addressing NONE of the available voice layers
	VOICE_LAYER_ALL = 255,      // Implies we are addressing ALL of the available voice layers

};
