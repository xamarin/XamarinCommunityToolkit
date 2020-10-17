using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views
{
	public class StateLayoutViewModel : BaseViewModel
	{
		ICommand fullscreenLoadingCommand;
		ICommand cycleStatesCommand;
		string customState;
		State currentState;
		State mainState;

		public State MainState
		{
			get => mainState;
			set => SetProperty(ref mainState, value);
		}

		public State CurrentState
		{
			get => currentState;
			set => SetProperty(ref currentState, value);
		}

		public string CustomState
		{
			get => customState;
			set => SetProperty(ref customState, value);
		}

		public ICommand FullscreenLoadingCommand
		{
			get => fullscreenLoadingCommand;
			set => SetProperty(ref fullscreenLoadingCommand, value);
		}

		public ICommand CycleStatesCommand
		{
			get => cycleStatesCommand;
			set => SetProperty(ref cycleStatesCommand, value);
		}

		public StateLayoutViewModel()
		{
			FullscreenLoadingCommand = new Command(async (x) =>
			{
				MainState = State.Loading;
				await Task.Delay(2000);
				MainState = State.None;
			});

			CycleStatesCommand = new Command(async (x) => await CycleStates());
		}

		async Task CycleStates()
		{
			CurrentState = State.Loading;
			await Task.Delay(3000);
			CurrentState = State.Saving;
			await Task.Delay(3000);
			CurrentState = State.Error;
			await Task.Delay(3000);
			CurrentState = State.Empty;
			await Task.Delay(3000);
			CurrentState = State.Custom;
			CustomState = "ThisIsCustomHi";
			await Task.Delay(3000);
			CustomState = "ThisIsCustomToo";
			await Task.Delay(3000);
			CurrentState = State.Success;
			await Task.Delay(3000);
			CurrentState = State.None;
		}
	}
}
