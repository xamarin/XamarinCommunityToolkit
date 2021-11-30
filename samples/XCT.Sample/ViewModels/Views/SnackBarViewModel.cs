using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views
{
	public class SnackBarViewModel : BaseViewModel
	{
		bool isToasting;

		public bool IsToasting
		{
			get => isToasting;
			set => SetProperty(ref isToasting, value);
		}

		public SnackBarViewModel()
		{
			ToasterTappedCommand = new AsyncCommand<VisualElement>(OnToasterTapped, allowsMultipleExecutions: false);
		}

		public ICommand ToasterTappedCommand { get; }

		async Task OnToasterTapped(VisualElement? element)
		{
			IsToasting = true;

			await Task.Delay(TimeSpan.FromSeconds(2));

			IsToasting = false;

			await element!.DisplayToastAsync("Your toast is ready");
		}
	}
}
