using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views
{
	public class BadgeViewViewModel : BaseViewModel
	{
		int counter;

		public BadgeViewViewModel() => Counter = 3;

		public int Counter
		{
			get => counter;

			set
			{
				counter = value;
				OnPropertyChanged();
			}
		}

		public ICommand IncreaseCommand => new Command(Increase);

		public ICommand DecreaseCommand => new Command(Decrease);

		void Increase() => Counter++;

		void Decrease()
		{
			if (Counter == 0)
				return;

			Counter--;
		}
	}
}