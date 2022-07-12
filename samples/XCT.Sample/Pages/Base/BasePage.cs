using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.Sample.Models;
using Xamarin.CommunityToolkit.Sample.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Xamarin.CommunityToolkit.Sample.Pages
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

		protected override void OnAppearing()
		{
			base.OnAppearing();
			(BindingContext as BaseGalleryViewModel)?.FilterCommand.Execute(string.Empty);
		}
	}
}