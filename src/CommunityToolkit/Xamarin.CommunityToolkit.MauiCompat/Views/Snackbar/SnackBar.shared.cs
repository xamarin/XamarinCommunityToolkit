using System;using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.UI.Views
{
	internal partial class SnackBar
	{
		internal partial ValueTask Show(VisualElement sender, SnackBarOptions arguments);

		async Task OnActionClick(SnackBarActionOptions action, SnackBarOptions arguments)
		{
			try
			{
				if (action.Action != null)
					await action.Action();

				arguments.SetResult(true);
			}
			catch (Exception ex)
			{
				arguments.SetException(ex);
			}
		}
	}
}