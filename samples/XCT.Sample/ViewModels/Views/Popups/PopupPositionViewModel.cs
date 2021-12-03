using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.Sample.Pages.Views.Popups;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views.Popups
{
	public class PopupPositionViewModel
	{
		public PopupPositionViewModel()
		{
			DisplayPopup = new Command<PopupPosition>(OnDisplayPopup);
		}

		INavigation Navigation => Application.Current.MainPage.Navigation;

		public ICommand DisplayPopup { get; }

		void OnDisplayPopup(PopupPosition position)
		{
			var popup = new SimplePopup();

			switch (position)
			{
				case PopupPosition.TopLeft:
					popup.VerticalOptions = new LayoutOptions(LayoutAlignment.Start, true);
					popup.HorizontalOptions = new LayoutOptions(LayoutAlignment.Start, true);
					break;
				case PopupPosition.Top:
					popup.VerticalOptions = new LayoutOptions(LayoutAlignment.Start, true);
					popup.HorizontalOptions = new LayoutOptions(LayoutAlignment.Center, true);
					break;
				case PopupPosition.TopRight:
					popup.VerticalOptions = new LayoutOptions(LayoutAlignment.Start, true);
					popup.HorizontalOptions = new LayoutOptions(LayoutAlignment.End, true);
					break;
				case PopupPosition.Left:
					popup.VerticalOptions = new LayoutOptions(LayoutAlignment.Center, true);
					popup.HorizontalOptions = new LayoutOptions(LayoutAlignment.Start, true);
					break;
				case PopupPosition.Center:
					popup.VerticalOptions = new LayoutOptions(LayoutAlignment.Center, true);
					popup.HorizontalOptions = new LayoutOptions(LayoutAlignment.Center, true);
					break;
				case PopupPosition.Right:
					popup.VerticalOptions = new LayoutOptions(LayoutAlignment.Center, true);
					popup.HorizontalOptions = new LayoutOptions(LayoutAlignment.End, true);
					break;
				case PopupPosition.BottomLeft:
					popup.VerticalOptions = new LayoutOptions(LayoutAlignment.End, true);
					popup.HorizontalOptions = new LayoutOptions(LayoutAlignment.Start, true);
					break;
				case PopupPosition.Bottom:
					popup.VerticalOptions = new LayoutOptions(LayoutAlignment.End, true);
					popup.HorizontalOptions = new LayoutOptions(LayoutAlignment.Center, true);
					break;
				case PopupPosition.BottomRight:
					popup.VerticalOptions = new LayoutOptions(LayoutAlignment.End, true);
					popup.HorizontalOptions = new LayoutOptions(LayoutAlignment.End, true);
					break;
			}

			Navigation.ShowPopup(popup);
		}

		public enum PopupPosition
		{
			TopLeft = 0,
			Top = 1,
			TopRight = 2,
			Left = 3,
			Center = 4,
			Right = 5,
			BottomLeft = 6,
			Bottom = 7,
			BottomRight = 8
		}
	}
}