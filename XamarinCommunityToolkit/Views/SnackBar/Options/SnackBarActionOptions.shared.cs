using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views.Options
{
	public class SnackBarActionOptions
	{
		/// <summary>
		/// Gets or sets the action for the SnackBar action button.
		/// </summary>
		public Func<Task> Action { get; set; }

		/// <summary>
		/// Gets or sets the text for the SnackBar action button.
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets the font size for the SnackBar action button.
		/// </summary>
		public double FontSize { get; set; } = 14;

		/// <summary>
		/// Gets or sets the font family for the SnackBar action button.
		/// </summary>
		public string FontFamily { get; set; } = "Arial";

		/// <summary>
		/// Gets or sets the background color for the SnackBar action button.
		/// </summary>
		public Color BackgroundColor { get; set; } = Color.White;

		/// <summary>
		/// Gets or sets the font color for the SnackBar action button.
		/// </summary>
		public Color ForegroundColor { get; set; } = Color.Black;
	}
}