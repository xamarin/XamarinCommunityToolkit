using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public interface IColorTheme
	{
		Color GetForegroundColor(string text);

		Color GetBackgroundColor(string text);
	}
}