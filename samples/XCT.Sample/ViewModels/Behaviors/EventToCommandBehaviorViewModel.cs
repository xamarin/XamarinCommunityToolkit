using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Behaviors
{
	public class EventToCommandBehaviorViewModel : BaseViewModel
	{
		ICommand incrementCommand;

		int clickCount;

		public ICommand IncrementCommand => incrementCommand ??= new Command(() => ClickCount++);

		public int ClickCount
		{
			get => clickCount;
			set => SetProperty(ref clickCount, value);
		}
	}
}