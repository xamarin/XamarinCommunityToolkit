using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views
{
	public class ContentButtonViewModel : BaseViewModel
	{
		int counter = 0;

		public int Counter
		{
			get => counter;
			set => SetProperty(ref counter, value);
		}

		ICommand tappedCommand;

		public ICommand TappedCommand => tappedCommand ??= new Command(p =>
		{
			Counter++;
		});
	}
}
