using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views.Options
{
	public class MessageOptions
	{
		/// <summary>
		/// Gets or sets the message for the SnackBar.
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// Gets or sets the font size for the SnackBar message.
		/// </summary>
		public double FontSize { get; set; } = 14;

		/// <summary>
		/// Gets or sets the font family for the SnackBar message.
		/// </summary>
		public string FontFamily { get; set; } = "Arial";

		/// <summary>
		/// Gets or sets the font color for the SnackBar message.
		/// </summary>
		public Color Foreground { get; set; } = Color.Black;
	}
}