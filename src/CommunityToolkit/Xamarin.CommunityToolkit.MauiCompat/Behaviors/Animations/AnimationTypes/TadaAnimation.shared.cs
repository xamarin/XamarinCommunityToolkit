using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Behaviors
{
	/// <summary>
	/// A 'Tada' animation. Results in:
	/// <list type="bullet">
	/// <item>reducing the scale of the view(s)</item>
	/// <item>wobbling the view(s) while scaling up</item>
	/// <item>scaling back down to 100%</item>
	/// </list>
	/// </summary>
	public class TadaAnimation : CompoundAnimationBase
	{
		/// <summary>
		/// The <see cref="BindableProperty"/> backing store for the <see cref="MaximumScale"/> property.
		/// </summary>
		public static readonly BindableProperty MaximumScaleProperty =
		   BindableProperty.Create(nameof(MaximumScale), typeof(double), typeof(TadaAnimation), 1.1, BindingMode.TwoWay);

		/// <summary>
		/// Gets or sets the maximum value the views will be scaled to during the animation.
		/// </summary>
		public double MaximumScale
		{
			get => (double)GetValue(MaximumScaleProperty);
			set => SetValue(MaximumScaleProperty, value);
		}

		/// <summary>
		/// The <see cref="BindableProperty"/> backing store for the <see cref="MinimumScale"/> property.
		/// </summary>
		public static readonly BindableProperty MinimumScaleProperty =
		   BindableProperty.Create(nameof(MinimumScale), typeof(double), typeof(TadaAnimation), 0.9, BindingMode.TwoWay);

		/// <summary>
		/// Gets or sets the minimum value the views will be scaled to during the animation.
		/// </summary>
		public double MinimumScale
		{
			get => (double)GetValue(MinimumScaleProperty);
			set => SetValue(MinimumScaleProperty, value);
		}

		/// <summary>
		/// The <see cref="BindableProperty"/> backing store for the <see cref="RotationAngle"/> property.
		/// </summary>
		public static readonly BindableProperty RotationAngleProperty =
		   BindableProperty.Create(nameof(RotationAngle), typeof(double), typeof(TadaAnimation), 3.0, BindingMode.TwoWay);

		/// <summary>
		/// Gets or sets the angle the views will be rotated by during the animation.
		/// </summary>
		public double RotationAngle
		{
			get => (double)GetValue(RotationAngleProperty);
			set => SetValue(RotationAngleProperty, value);
		}

		/// <inheritdoc />
		protected override uint DefaultDuration { get; set; } = 1000;

		/// <inheritdoc />
		protected override Animation CreateAnimation(params View[] views) => Create(RotationAngle, MinimumScale, MaximumScale, views);

		static Animation Create(double rotationAngle, double minimumScale, double maximumScale, params View[] views)
		{
			var animation = new Animation();

			foreach (var view in views)
			{
				animation.Add(0, 0.1, new Animation(v => view.Scale = v, 1, minimumScale));
				animation.Add(0.2, 0.3, new Animation(v => view.Scale = v, minimumScale, maximumScale));
				animation.Add(0.9, 1.0, new Animation(v => view.Scale = v, maximumScale, 1));

				animation.Add(0, 0.2, new Animation(v => view.Rotation = v, 0, -rotationAngle));
				animation.Add(0.2, 0.3, new Animation(v => view.Rotation = v, -rotationAngle, rotationAngle));
				animation.Add(0.3, 0.4, new Animation(v => view.Rotation = v, rotationAngle, -rotationAngle));
				animation.Add(0.4, 0.5, new Animation(v => view.Rotation = v, -rotationAngle, rotationAngle));
				animation.Add(0.5, 0.6, new Animation(v => view.Rotation = v, rotationAngle, -rotationAngle));
				animation.Add(0.6, 0.7, new Animation(v => view.Rotation = v, -rotationAngle, rotationAngle));
				animation.Add(0.7, 0.8, new Animation(v => view.Rotation = v, rotationAngle, -rotationAngle));
				animation.Add(0.8, 0.9, new Animation(v => view.Rotation = v, -rotationAngle, rotationAngle));
				animation.Add(0.9, 1.0, new Animation(v => view.Rotation = v, rotationAngle, 0));
			}

			return animation;
		}
	}
}