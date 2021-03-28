using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class RotateAnimation : AnimationBase
	{
		public static readonly BindableProperty RotationProperty =
            BindableProperty.Create(nameof(Rotation), typeof(double), typeof(AnimationBase), 180.0, BindingMode.TwoWay, defaultValueCreator: GetDefaulRotationProperty);

		public double Rotation
		{
			get => (double)GetValue(RotationProperty);
			set => SetValue(RotationProperty, value);
		}

		static object GetDefaulRotationProperty(BindableObject bindable)
			=> ((RotateAnimation)bindable).DefaultRotation;

		protected override uint DefaultDuration { get; set; } = 200;

		protected virtual double DefaultRotation { get; set; } = 180.0;

		public override Task Animate(View view) =>
            Device.InvokeOnMainThreadAsync(() =>
            {
                view.RotateTo(Rotation, Duration, Easing);
                view.Rotation = 0;
            });
    }

	public class RelRotateAnimation : AnimationBase
    {
        public static readonly BindableProperty RotationProperty =
            BindableProperty.Create(nameof(Rotation), typeof(double), typeof(RelRotateAnimation), default(double),
                BindingMode.TwoWay, null);

        public double Rotation
        {
            get => (double)GetValue(RotationProperty);
            set => SetValue(RotationProperty, value);
        }

        protected override uint DefaultDuration { get; set; } = 200;

        public override Task Animate(View view) =>
            Device.InvokeOnMainThreadAsync(() => view.RelRotateTo(Rotation, Duration, Easing));
    }

	public class RotateXAnimation : AnimationBase
    {
        public static readonly BindableProperty RotationProperty =
            BindableProperty.Create(nameof(Rotation), typeof(double), typeof(RotateXAnimation), default(double),
                BindingMode.TwoWay, null);

        public double Rotation
        {
            get => (double)GetValue(RotationProperty);
            set => SetValue(RotationProperty, value);
        }

        protected override uint DefaultDuration { get; set; } = 200;

        public override Task Animate(View view) =>
            Device.InvokeOnMainThreadAsync(() => view.RotateXTo(Rotation, Duration, Easing));
    }

	public class RotateYAnimation : AnimationBase
    {
        public static readonly BindableProperty RotationProperty =
            BindableProperty.Create(nameof(Rotation), typeof(double), typeof(RotateYAnimation), default(double),
                BindingMode.TwoWay, null);

        public double Rotation
        {
            get => (double)GetValue(RotationProperty);
            set => SetValue(RotationProperty, value);
        }

        protected override uint DefaultDuration { get; set; } = 200;

        public override Task Animate(View view) =>
            Device.InvokeOnMainThreadAsync(() => view.RotateYTo(Rotation, Duration, Easing));
    }
}