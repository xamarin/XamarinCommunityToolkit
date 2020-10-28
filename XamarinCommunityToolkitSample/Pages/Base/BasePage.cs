using System;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.Sample.Models;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages
{
	public class BasePage : ContentPage
	{
		ICommand navigateCommand;

		public Color DetailColor { get; set; }

		public ICommand NavigateCommand => navigateCommand ??= new AsyncCommand<SectionModel>(sectionModel
			=> Navigation.PushAsync(PreparePage(sectionModel)));

		Page PreparePage(SectionModel model)
		{
			var page = (BasePage)Activator.CreateInstance(model.Type);
			page.Title = model.Title;
			page.DetailColor = model.Color;
			return page;
		}
	}
}