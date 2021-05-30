using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.ObjectModel;
using CommunityToolkit.Maui.Sample.Models;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace CommunityToolkit.Maui.Sample.Pages
{
	public class BasePage : ContentPage
	{
		public BasePage()
		{
			On<iOS>().SetPrefersHomeIndicatorAutoHidden(true);

			NavigateCommand = CommandFactory.Create<SectionModel>(sectionModel =>
			{
				if (sectionModel != null)
					return Navigation.PushAsync(PreparePage(sectionModel));

				return Task.CompletedTask;
			});
		}

		public Color DetailColor { get; set; }

		public ICommand NavigateCommand { get; }

		Forms.Page PreparePage(SectionModel model)
		{
			var page = (BasePage)Activator.CreateInstance(model.Type);
			page.Title = model.Title;
			page.DetailColor = model.Color;
			return page;
		}
	}
}