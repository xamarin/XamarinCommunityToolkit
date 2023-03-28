using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.TestCases
{
	public class Issue1900ViewModel : BaseViewModel
	{
		int counter;

		public int Counter
		{
			get => counter;
			set => SetProperty(ref counter, value);
		}

		public ICommand ClickCommand => new AsyncCommand(Click);

		public Issue1900ViewModel()
		{
		}

		Task Click()
		{
			Counter++;
			return Task.CompletedTask;
		}
	}
}
