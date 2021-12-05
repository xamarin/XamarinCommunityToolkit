using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.TestCases
{
	public class TabViewItemBindingViewModel : BaseViewModel
	{
		public static TabViewItemBindingViewModel Current { get; } = new();

		int countClick;

		public int CountClick
		{
			get => countClick;
			set => SetProperty(ref countClick, value);
		}

		public ICommand ClickCommand => new AsyncCommand(Click);

		Task Click()
		{
			CountClick++;
			return Task.CompletedTask;
		}
	}
}