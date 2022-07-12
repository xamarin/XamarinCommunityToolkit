﻿using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Sample.Pages;
using Xamarin.CommunityToolkit.Sample.ViewModels.Markup;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Xamarin.CommunityToolkit.Sample
{
	public partial class SearchPage : BasePage
	{
		readonly SearchViewModel vm;
		View? header;

		public SearchPage()
		{
			On<iOS>().SetUseSafeArea(true);
			BackgroundColor = Color.Black;

			BindingContext = vm = new SearchViewModel();
			Build();
		}

		async void Search_FocusChanged(object? sender, FocusEventArgs e)
		{
			ViewExtensions.CancelAnimations(header);
			await (header?.TranslateTo(e.IsFocused ? -56 : 0, 0, 250, Easing.CubicOut) ?? Task.CompletedTask);
		}
	}
}