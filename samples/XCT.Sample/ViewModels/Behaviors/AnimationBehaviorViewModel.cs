using System.Windows.Input;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Behaviors
{
	public class AnimationBehaviorViewModel : BaseViewModel
	{
		ICommand? triggerAnimationCommand;

		public ICommand? TriggerAnimationCommand
		{
			get => triggerAnimationCommand;
			set => SetProperty(ref triggerAnimationCommand, value);
		}
	}
}