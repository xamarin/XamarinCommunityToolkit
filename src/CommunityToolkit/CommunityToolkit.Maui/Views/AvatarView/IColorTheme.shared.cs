using Xamarin.Forms;

namespace CommunityToolkit.Maui.UI.Views
{
	/// <summary>
	/// By implementing the <see cref="IColorTheme"/> you can compose your own theme that can be used with the <see cref="AvatarView"/> to create a consistent look. Assign the implementation of this interface to the <see cref="AvatarView.ColorTheme"/> property to use it.
	/// </summary>
	public interface IColorTheme
	{
		Color GetForegroundColor(string text);

		Color GetBackgroundColor(string text);
	}
}