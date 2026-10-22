namespace Sandbox;

public sealed partial class CameraComponent
{
	public class AutoExposureSetup
	{
		public bool Enabled { get; set; } = false;
		public float Compensation { get; set; } = 0.0f;
		public float MinimumExposure { get; set; } = 1.0f;
		public float MaximumExposure { get; set; } = 3.0f;
		public float Rate { get; set; } = 1.0f;

		internal void Apply( SceneCamera scenecam )
		{
			if ( !Enabled )
			{
				scenecam.Tonemap.Enabled = false;
				return;
			}

			scenecam.Tonemap.Enabled = true;
			scenecam.Tonemap.ExposureCompensation = Compensation;
			scenecam.Tonemap.MinExposure = MinimumExposure;
			scenecam.Tonemap.MaxExposure = MaximumExposure;
			scenecam.Tonemap.Rate = Rate;

			// this Fade value is a multiple of Rate, and allows you to control the
			// speed of decent. I'm not enabling yet because it's confusing. I've found
			// that 16 here makes it kind of match the up rate. We should look into this
			// and give distinct up and down rates.
			// Sam: Changed to 1.0f, 16.0f was too fast when using an actual HDR image
			scenecam.Tonemap.Fade = 1.0f;
		}
	}

	/// <summary>
	/// Enables and configures auto exposure on the camera. This is usually controlled
	/// by the Tonemapping component. But if you're not using that, it can be controlled manually here.
	/// </summary>
	public AutoExposureSetup AutoExposure { get; } = new AutoExposureSetup();

}
