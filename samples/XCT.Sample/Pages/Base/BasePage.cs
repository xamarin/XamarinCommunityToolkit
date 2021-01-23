using System;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.Sample.Models;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages
{
	public class BasePage : ContentPage
	{
		public BasePage() =>
			NavigateCommand = CommandFactory.Create<SectionModel>(sectionModel => Navigation.PushAsync(PreparePage(sectionModel)));

		public Color DetailColor { get; set; }

		public ICommand NavigateCommand { get; }

		Page PreparePage(SectionModel model)
		{
			var page = (BasePage)Activator.CreateInstance(model.Type);
			page.Title = model.Title;
			page.DetailColor = model.Color;
			return page;
		}
	}
}