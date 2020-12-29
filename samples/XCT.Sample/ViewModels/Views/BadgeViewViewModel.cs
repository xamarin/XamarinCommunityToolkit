using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views
{
	public class BadgeViewViewModel : BaseViewModel
	{
		int counter;
		private bool isRounded = true;

		public BadgeViewViewModel() => Counter = 3;

		public bool IsRounded
		{
			get => isRounded;
			set
			{
				isRounded = value;
				OnPropertyChanged();
			}
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