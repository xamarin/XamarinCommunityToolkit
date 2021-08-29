//using System.Threading.Tasks;
//using Microsoft.Maui;
//using Microsoft.Maui.Controls;
//using Microsoft.Maui.Graphics;
//using Xamarin.CommunityToolkit.Sample.Pages;
//using Xamarin.CommunityToolkit.Sample.ViewModels.Markup;



//namespace Xamarin.CommunityToolkit.Sample
//{
//	public partial class SearchPage : BasePage
//	{
//		readonly SearchViewModel vm;
//		View? header;

//		public SearchPage()
//		{
//			On<iOS>().SetUseSafeArea(true);
//			BackgroundColor = Colors.Black;

//			BindingContext = vm = new SearchViewModel();
//			Build();
//		}

//		async void Search_FocusChanged(object? sender, FocusEventArgs e)
//		{
//			Microsoft.Maui.Controls.ViewExtensions.CancelAnimations(header);
//			await (header?.TranslateTo(e.IsFocused ? -56 : 0, 0, 250, Easing.CubicOut) ?? Task.CompletedTask);
//		}
//	}
//}