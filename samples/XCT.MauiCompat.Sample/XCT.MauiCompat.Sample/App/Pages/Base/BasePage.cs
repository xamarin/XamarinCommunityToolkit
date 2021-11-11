using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.Sample.Models;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Xamarin.CommunityToolkit.Sample.Pages.Behaviors;

namespace Xamarin.CommunityToolkit.Sample.Pages
{
	public class BasePage : ContentPage
	{
		public BasePage()
		{
			On<Microsoft.Maui.Controls.PlatformConfiguration.iOS>().SetPrefersHomeIndicatorAutoHidden(true);

			NavigateCommand = CommandFactory.Create<SectionModel>(sectionModel =>
			{
				if (sectionModel != null)
					return Navigation.PushAsync(PreparePage(sectionModel));

				return Task.CompletedTask;
			});
		}

		public Color DetailColor { get; set; }

		public ICommand NavigateCommand { get; }

		Microsoft.Maui.Controls.Page PreparePage(SectionModel model)
		{
			var page = (BasePage)Activator.CreateInstance(model.Type)!;
			page.Title = model.Title;
			page.DetailColor = model.Color;

			//page.SetAppThemeColor(BackgroundColorProperty, Colors.White, Colors.Black);
			return page;
		}
	}
}