using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views.Options;
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
			if (element is null)
			{
				return;
			}

			IsToasting = true;

			await Task.Delay(TimeSpan.FromSeconds(2));

			IsToasting = false;

			var messageOptions = new MessageOptions
			{
				Foreground = Color.LightGray,
				Font = Font.SystemFontOfSize(16),
				Message = "Your toast is ready",
				Padding = new Thickness(10, 10, 10, 10),
			};
			var options = new ToastOptions
			{
				MessageOptions = messageOptions,
				Duration = TimeSpan.FromMilliseconds(3000),
				BackgroundColor = Color.Black,
				CornerRadius = new Thickness(10, 10, 10, 10),
				IsRtl = false
			};

			await element.DisplayToastAsync(options);
		}
	}
}
