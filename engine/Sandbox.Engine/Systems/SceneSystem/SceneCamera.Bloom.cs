using System;

namespace Sandbox;

public partial class SceneCamera
{
	/// <summary>
	/// Access tonemapping properties of camera
	/// </summary>
	public BloomAccessor Bloom { get; internal set; }

	public class BloomAccessor
	{
		private readonly SceneCamera Camera;
		internal PostProcessingBloomParameters_t Parameters = default;

		internal BloomAccessor( SceneCamera camera )
		{
			Camera = camera;
			Parameters = new PostProcessingBloomParameters_t();

			BlurTint0 = Color.White;
			BlurTint1 = Color.White;
			BlurTint2 = Color.White;
			BlurTint3 = Color.White;
			BlurTint4 = Color.White;
		}

		/// <summary>
		/// Enable or disable exposure.
		/// </summary>
		public bool Enabled { get; set; } = false;

		[Expose]
		public enum BloomMode
		{
			Additive,
			Screen,
			Blur
		}


		public BloomMode Mode
		{
			get => (BloomMode)Parameters.m_blendMode;
			set => Parameters.m_blendMode = (PostProcessingBloomParameters_t.BloomBlendMode_t)value;
		}


		public float Strength
		{
			get => Parameters.m_flBloomStrength;
			set
			{
				Parameters.m_flBloomStrength = value;
				Parameters.m_flScreenBloomStrength = value;
				Parameters.m_flBlurBloomStrength = value;
			}
		}

		public float Threshold
		{
			get => Parameters.m_flBloomThreshold;
			set => Parameters.m_flBloomThreshold = value;
		}

		public float ThresholdWidth
		{
			get => Parameters.m_flBloomThresholdWidth;
			set => Parameters.m_flBloomThresholdWidth = value;
		}

		public float SkyboxStrength
		{
			get => Parameters.m_flSkyboxBloomStrength;
			set => Parameters.m_flSkyboxBloomStrength = value;
		}

		public float BlurWeight0
		{
			get => Parameters.m_flBlurWeight0;
			set => Parameters.m_flBlurWeight0 = value;
		}

		public float BlurWeight1
		{
			get => Parameters.m_flBlurWeight1;
			set => Parameters.m_flBlurWeight1 = value;
		}

		public float BlurWeight2
		{
			get => Parameters.m_flBlurWeight2;
			set => Parameters.m_flBlurWeight2 = value;
		}

		public float BlurWeight3
		{
			get => Parameters.m_flBlurWeight3;
			set => Parameters.m_flBlurWeight3 = value;
		}

		public float BlurWeight4
		{
			get => Parameters.m_flBlurWeight4;
			set => Parameters.m_flBlurWeight4 = value;
		}

		public Color BlurTint0
		{
			get => Parameters.m_vBlurTint0;
			set => Parameters.m_vBlurTint0 = value;
		}

		public Color BlurTint1
		{
			get => Parameters.m_vBlurTint1;
			set => Parameters.m_vBlurTint1 = value;
		}

		public Color BlurTint2
		{
			get => Parameters.m_vBlurTint2;
			set => Parameters.m_vBlurTint2 = value;
		}

		public Color BlurTint3
		{
			get => Parameters.m_vBlurTint3;
			set => Parameters.m_vBlurTint3 = value;
		}

		public Color BlurTint4
		{
			get => Parameters.m_vBlurTint4;
			set => Parameters.m_vBlurTint4 = value;
		}


	}
}
