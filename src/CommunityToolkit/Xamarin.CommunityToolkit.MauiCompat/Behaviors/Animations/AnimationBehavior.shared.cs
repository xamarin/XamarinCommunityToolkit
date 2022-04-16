using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class AnimationBehavior : EventToCommandBehavior
	{
		public static readonly BindableProperty AnimationTypeProperty =
			BindableProperty.Create(nameof(AnimationType), typeof(AnimationBase), typeof(AnimationBehavior));

		internal static readonly BindablePropertyKey AnimateCommandPropertyKey =
 			BindableProperty.CreateReadOnly(
 				nameof(AnimateCommand),
 				typeof(ICommand),
 				typeof(AnimationBehavior),
 				default,
 				BindingMode.OneWayToSource,
 				defaultValueCreator: CreateAnimateCommand);

		public static readonly BindableProperty AnimateCommandProperty = AnimateCommandPropertyKey.BindableProperty;

		public AnimationBase? AnimationType
		{
			get => (AnimationBase?)GetValue(AnimationTypeProperty);
			set => SetValue(AnimationTypeProperty, value);
		}

		public ICommand AnimateCommand => (ICommand)GetValue(AnimateCommandProperty);

		bool isAnimating;
		TapGestureRecognizer? tapGestureRecognizer;

		protected override void OnAttachedTo(VisualElement bindable)
		{
			base.OnAttachedTo(bindable);

			if (!string.IsNullOrWhiteSpace(EventName) || !(bindable is View view))
				return;

			tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += OnTriggerHandled;
			view.GestureRecognizers.Clear();
			view.GestureRecognizers.Add(tapGestureRecognizer);
		}

		protected override void OnDetachingFrom(VisualElement bindable)
		{
			if (tapGestureRecognizer != null)
				tapGestureRecognizer.Tapped -= OnTriggerHandled;

			base.OnDetachingFrom(bindable);
		}

		protected override async void OnTriggerHandled(object? sender = null, object? eventArgs = null)
		{
			await OnAnimate();

			base.OnTriggerHandled(sender, eventArgs);
		}

		static object CreateAnimateCommand(BindableObject bindable)
			=> new AsyncCommand(((AnimationBehavior)bindable).OnAnimate);

		async Task OnAnimate()
		{
			if (isAnimating || View is not View typedView)
				return;

			isAnimating = true;

			if (AnimationType != null)
				await AnimationType.Animate(typedView);

			isAnimating = false;
		}
	}
}