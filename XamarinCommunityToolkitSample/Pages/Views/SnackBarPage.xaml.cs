using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views
{
	public partial class SnackBarPage : BasePage
	{
		public SnackBarPage() => InitializeComponent();

		async void DisplaySnackBarClicked(object sender, EventArgs args)
		{
			var result = await this.DisplaySnackBarAsync(GenerateLongText(5), "Run action", () =>
			{
				Debug.WriteLine("SnackBar action button clicked");
				return Task.CompletedTask;
			});
			StatusText.Text = result ? "SnackBar is closed by user" : "SnackBar is closed by timeout";
		}

		async void DisplayToastClicked(object sender, EventArgs args)
		{
			var result = await this.DisplayToastAsync(GenerateLongText(5));
			StatusText.Text = result ? "SnackBar is closed by user" : "SnackBar is closed by timeout";
		}

		async void DisplaySnackBarAdvancedClicked(object sender, EventArgs args)
		{
			var messageOptions = new MessageOptions
			{
				Foreground = Color.DeepSkyBlue,
				FontSize = 40,
				FontFamily = "Sans-serif",
				Message = GenerateLongText(5)
			};

			var actionOptions = new List<SnackBarActionOptions>
			{
				new SnackBarActionOptions
				{
					ForegroundColor = Color.Red,
					BackgroundColor = Color.Green,
					FontSize = 40,
					FontFamily = "Sans-serif",
					Text = "Action1",
					Action = () =>
					{
						Debug.WriteLine("1");
						return Task.CompletedTask;
					}
				},
				new SnackBarActionOptions
				{
					ForegroundColor = Color.Green,
					BackgroundColor = Color.Red,
					FontSize = 20,
					FontFamily = "Sans-serif",
					Text = "Action2",
					Action = () =>
					{
						Debug.WriteLine("2");
						return Task.CompletedTask;
					}
				}
			};
			var options = new SnackBarOptions(messageOptions, 5000, Color.Coral, true, actionOptions);
			var result = await this.DisplaySnackBarAsync(options);
			StatusText.Text = result ? "SnackBar is closed by user" : "SnackBar is closed by timeout";
		}

		string GenerateLongText(int stringDuplicationTimes)
		{
			const string message = "It is a very long message to test multiple strings. A B C D E F G H I I J K LO P Q R S T U V W X Y Z";
			var result = new StringBuilder();
			for (var i = 0; i < stringDuplicationTimes; i++)
			{
				result.AppendLine(message);
			}

			return result.ToString();
		}
	}
}