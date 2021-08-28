using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.Sample.Models;
using Xamarin.CommunityToolkit.Sample.Pages.Views.Popups;
using Xamarin.CommunityToolkit.UI.Views;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views
{
	public class PopupGalleryViewModel
	{
		public PopupGalleryViewModel()
		{
			DisplayPopup = CommandFactory.Create<Type>(OnDisplayPopup);
		}

		INavigation Navigation => Application.Current.MainPage.Navigation;

		public IEnumerable<SectionModel> Examples { get; } = new[]
		{
			new SectionModel(typeof(SimplePopup), "Simple Popup", Colors.Red, "Displays a basic popup centered on the screen"),
			new SectionModel(typeof(PopupPositionPage), "Custom Positioning Popup", Colors.Red, "Displays a basic popup anywhere on the screen using VerticalOptions and HorizontalOptions"),
			new SectionModel(typeof(ButtonPopup), "Popup With 1 Button", Colors.Red, "Displays a basic popup with a confirm button"),
			new SectionModel(typeof(MultipleButtonPopup), "Popup With Multiple Buttons", Colors.Red, "Displays a basic popup with a cancel and confirm button"),
			new SectionModel(typeof(NoLightDismissPopup), "Simple Popup Without Light Dismiss", Colors.Red, "Displays a basic popup but does not allow the user to close it if they tap outside of the popup. In other words the LightDismiss is set to false."),
			new SectionModel(typeof(ToggleSizePopup), "Toggle Size Popup", Colors.Red, "Displays a popup that can have it's size updated by pressing a button"),
			new SectionModel(typeof(TransparentPopup), "Transparent Popup", Colors.Red, "Displays a popup with a transparent background"),
			new SectionModel(typeof(PopupAnchorPage), "Anchor Popup", Colors.Red, "Popups can be anchored to other view's on the screen"),
			new SectionModel(typeof(OpenedEventSimplePopup), "Opened Event Popup", Colors.Red, "Popup with opened event"),
			new SectionModel(typeof(ReturnResultPopup), "Return Result Popup", Colors.Red, "A popup that returns a string message when dismissed"),
			new SectionModel(typeof(XamlBindingPopup), "Xaml Binding Popup", Colors.Red, "A simple popup that uses XAML BindingContext"),
			new SectionModel(typeof(CsharpBindingPopup), "C# Binding Popup", Colors.Red, "A simple popup that uses C# BindingContext")
		}.OrderBy(x => x.Title);

		public ICommand DisplayPopup { get; }

		async Task OnDisplayPopup(Type? popupType)
		{
			var view = (VisualElement)Activator.CreateInstance(popupType);

			if (view is Popup<string?> popup)
			{
				var result = await Navigation.ShowPopupAsync(popup);
				await Application.Current.MainPage.DisplayAlert("Popup Result", result, "OKAY");
			}
			else if (view is BasePopup basePopup)
			{
				Navigation.ShowPopup(basePopup);
			}
			else if (view is Page page)
			{
				await Navigation.PushAsync(page);
			}
		}
	}
}
