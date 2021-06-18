using System;
using Xamarin.CommunityToolkit.Extensions;

namespace Xamarin.CommunityToolkit.Sample.Pages.TestCases
{
	public partial class SnackBarActionExceptionPage
	{
		public SnackBarActionExceptionPage() => InitializeComponent();

		async void ShowSnackBar(object sender, EventArgs e)
		{
			try
			{
				await this.DisplaySnackBarAsync("Execute action to throw exception", "Throw exception", () => throw new Exception());
			}
			catch
			{
				await this.DisplayToastAsync("Exception caught");
			}
		}
	}
}