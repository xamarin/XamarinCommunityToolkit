using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views.Options
{
	public class MessageOptions
	{
		/// <summary>
		/// Gets or sets the message for the SnackBar.
		/// </summary>
		public string Message { get; set; } = DefaultMessage;

		public static string DefaultMessage { get; set; }

		/// <summary>
		/// Gets or sets the font size for the SnackBar message.
		/// </summary>
		public double FontSize { get; set; } = DefaultFontSize;

		public static double DefaultFontSize { get; set; } = 14;

		/// <summary>
		/// Gets or sets the font family for the SnackBar message.
		/// </summary>
		public string FontFamily { get; set; } = DefaultFontFamily;

		public static string DefaultFontFamily { get; set; } = "Arial";

		/// <summary>
		/// Gets or sets the font color for the SnackBar message.
		/// </summary>
		public Color Foreground { get; set; } = DefaultForeground;

		public static Color DefaultForeground { get; set; } = Color.Black;
	}
}