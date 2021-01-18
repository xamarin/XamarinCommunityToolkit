using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.UI.Views;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views
{
	public class StateLayoutViewModel : BaseViewModel
	{
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

		public ICommand FullscreenLoadingCommand { get; }

		public ICommand CycleStatesCommand { get; }

		public StateLayoutViewModel()
		{
			FullscreenLoadingCommand = CommandHelper.Create(async () =>
			{
				MainState = LayoutState.Loading;
				await Task.Delay(2000);
				MainState = LayoutState.None;
			});
			CycleStatesCommand = CommandHelper.Create(CycleStates);
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