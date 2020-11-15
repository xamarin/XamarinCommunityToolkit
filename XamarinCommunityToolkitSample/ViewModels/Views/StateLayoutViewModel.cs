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
		LayoutState currentState;
		LayoutState mainState;

		public LayoutState MainState
		{
			get => mainState;
			set => SetProperty(ref mainState, value);
		}

		public LayoutState CurrentState
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
				MainState = LayoutState.Loading;
				await Task.Delay(2000);
				MainState = LayoutState.None;
			});

			CycleStatesCommand = new Command(async (x) => await CycleStates());
		}

		async Task CycleStates()
		{
			CurrentState = LayoutState.Loading;
			await Task.Delay(3000);
			CurrentState = LayoutState.Saving;
			await Task.Delay(3000);
			CurrentState = LayoutState.Error;
			await Task.Delay(3000);
			CurrentState = LayoutState.Empty;
			await Task.Delay(3000);
			CurrentState = LayoutState.Custom;
			CustomState = "ThisIsCustomHi";
			await Task.Delay(3000);
			CustomState = "ThisIsCustomToo";
			await Task.Delay(3000);
			CurrentState = LayoutState.Success;
			await Task.Delay(3000);
			CurrentState = LayoutState.None;
		}
	}
}