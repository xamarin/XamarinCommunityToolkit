using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views
{
	public class BadgeViewViewModel : BaseViewModel
	{
		int counter;

		public BadgeViewViewModel()
		{
			Counter = 3;

			IncreaseCommand = CommandFactory.Create(Increase);
			DecreaseCommand = CommandFactory.Create(Decrease);
		}

		public int Counter
		{
			get => counter;
			set
			{
				counter = value;
				OnPropertyChanged();
			}
		}

		public ICommand IncreaseCommand { get; }

		public ICommand DecreaseCommand { get; }

		void Increase() => Counter++;

		void Decrease()
		{
			if (Counter == 0)
				return;

			Counter--;
		}
	}
}