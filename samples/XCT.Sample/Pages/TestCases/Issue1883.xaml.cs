using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views
{
	public partial class Issue1883Page : BasePage
	{
		public Issue1883Page() => InitializeComponent();

		async void Issue1883(object sender, EventArgs args)
		{
			var options = new SnackBarOptions
			{
				MessageOptions = new MessageOptions
				{
					Foreground = Color.DodgerBlue,
					Padding = new Thickness(10, 20, 30, 40),
					Message = "Pour supprimer un élément de votre liste hors connexion, faites glisser l'élément vers la droite"
				},
				BackgroundColor = Color.Aquamarine,
				Duration = TimeSpan.FromSeconds(5),
				CornerRadius = new Thickness(10, 20, 30, 40),
				Actions = new[]
				{
					new SnackBarActionOptions
					{
						Text = "I've completed this",
						BackgroundColor = (Color.Aqua),
						ForegroundColor = Color.Brown,
						Padding = new Thickness(40, 30, 20, 10),
						Action = () => Task.FromResult(true)
					},
				}
			};

			var result = await this.DisplaySnackBarAsync(options);
			StatusText.Text = result ? "SnackBar is closed by user" : "SnackBar is closed by timeout";
		}
	}
}