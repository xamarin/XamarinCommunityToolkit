using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Behaviors
{
	public class EventToCommandBehaviorViewModel : BaseViewModel
	{
		int clickCount;

		public EventToCommandBehaviorViewModel() =>
			IncrementCommand = CommandFactory.Create(() => ClickCount++);

		public int ClickCount
		{
			get => clickCount;
			set => SetProperty(ref clickCount, value);
		}

		public ICommand IncrementCommand { get; }
	}
}