using System.Text.Json.Nodes;

namespace Sandbox;

partial class SkinnedModelRenderer
{
	SequenceAccessor _sequence;

	public bool ShouldShowSequenceEditor
	{
		get
		{
			if ( UseAnimGraph ) return false;
			if ( SceneModel.IsValid() == false ) return false;
			if ( Model.IsValid() == false ) return false;
			if ( Model.AnimationCount <= 0 ) return false;

			return true;
		}
	}

	public sealed class SequenceAccessor : IJsonPopulator
	{
		private readonly SkinnedModelRenderer _renderer;
		private string _name;
		private bool _looping = true;
		private bool _blending = false;

		[Hide]
		private AnimationSequence CurrentSequence
		{
			get
			{
				if ( !_renderer.IsValid() )
					return null;

				if ( !_renderer.SceneModel.IsValid() )
					return null;

				return _renderer.SceneModel.CurrentSequence;
			}
		}

		/// <inheritdoc cref="AnimationSequence.Duration"/>
		[Hide] public float Duration => CurrentSequence.Duration;

		/// <inheritdoc cref="AnimationSequence.IsFinished"/>
		[Hide] public bool IsFinished => CurrentSequence.IsFinished;

		/// <inheritdoc cref="AnimationSequence.TimeNormalized"/>
		[Hide] public float TimeNormalized { get => CurrentSequence.TimeNormalized; set => CurrentSequence.TimeNormalized = value; }

		/// <inheritdoc cref="AnimationSequence.Time"/>
		[Hide] public float Time { get => CurrentSequence.Time; set => CurrentSequence.Time = value; }

		/// <inheritdoc cref="AnimationSequence.SequenceNames"/>
		[Hide] public IReadOnlyList<string> SequenceNames => CurrentSequence.SequenceNames;

		/// <inheritdoc cref="AnimationSequence.Name"/>
		[Title( "Sequence" ), Editor( "Sequence" )]
		public string Name
		{
			get => _name;
			set
			{
				_name = value;

				if ( CurrentSequence is not null )
					CurrentSequence.Name = _name;
			}
		}

		/// <inheritdoc cref="AnimationSequence.Looping"/>
		public bool Looping
		{
			get => _looping;
			set
			{
				_looping = value;

				if ( CurrentSequence is not null )
					CurrentSequence.Looping = _looping;
			}
		}

		/// <inheritdoc cref="AnimationSequence.Blending"/>
		public bool Blending
		{
			get => _blending;
			set
			{
				_blending = value;

				if ( CurrentSequence is not null )
					CurrentSequence.Blending = _blending;
			}
		}

		/// <summary>
		/// Control playback rate of sequence.
		/// </summary>
		[Hide]
		public float PlaybackRate
		{
			get => _renderer.PlaybackRate;
			set => _renderer.PlaybackRate = value;
		}

		internal SequenceAccessor( SkinnedModelRenderer renderer )
		{
			_renderer = renderer;
		}

		internal void Apply()
		{
			var currentSequence = CurrentSequence;
			if ( currentSequence is null )
				return;

			currentSequence.Name = Name;
			currentSequence.Looping = Looping;
			currentSequence.Blending = Blending;
		}

		JsonNode IJsonPopulator.Serialize()
		{
			return new JsonObject
			{
				["Name"] = Name,
				["Looping"] = Looping,
				["Blending"] = Blending,
			};
		}

		void IJsonPopulator.Deserialize( JsonNode e )
		{
			if ( e is not JsonObject jso )
				return;

			Name = jso["Name"]?.GetValue<string>() ?? default;
			Looping = jso["Looping"]?.GetValue<bool>() ?? true;
			Blending = jso["Blending"]?.GetValue<bool>() ?? false;
		}
	}
}
