using System.Threading.Tasks;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public abstract class AnimationBase<TView> : BindableObject
		where TView : View
	{
		public static readonly BindableProperty DurationProperty =
			BindableProperty.Create(nameof(Duration), typeof(uint), typeof(AnimationBase<TView>), default(uint),
				BindingMode.TwoWay, defaultValueCreator: GetDefaultDurationProperty);

		public uint Duration
		{
			get => (uint)GetValue(DurationProperty);
			set => SetValue(DurationProperty, value);
		}

		public static readonly BindableProperty EasingTypeProperty =
		   BindableProperty.Create(nameof(Easing), typeof(Easing), typeof(AnimationBase<TView>), Easing.Linear,
			   BindingMode.TwoWay);

		public Easing Easing
		{
			get => (Easing)GetValue(EasingTypeProperty);
			set => SetValue(EasingTypeProperty, value);
		}

		static object GetDefaultDurationProperty(BindableObject bindable)
			=> ((AnimationBase<TView>)bindable).DefaultDuration;

		protected abstract uint DefaultDuration { get; set; }

		public abstract Task Animate(TView? view);
	}

	public abstract class AnimationBase : AnimationBase<View>
	{
	}
}