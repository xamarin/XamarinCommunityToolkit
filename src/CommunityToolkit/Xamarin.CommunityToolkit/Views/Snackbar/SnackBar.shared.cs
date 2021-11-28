using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	partial class SnackBar
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