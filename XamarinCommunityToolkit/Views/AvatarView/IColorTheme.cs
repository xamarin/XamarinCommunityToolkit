using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public interface IColorTheme
	{
		Color[] BackgroundColors { get; set; }

		Color[] ForegroundColors { get; set; }

		Color GetForegroundColor(string text);

		Color GetBackgroundColor(string text);
	}
}
