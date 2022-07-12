﻿using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views.Options
{
	public class MessageOptions
	{
		/// <summary>
		/// Gets or sets the message for the SnackBar.
		/// </summary>
		public string Message { get; set; } = DefaultMessage;

		public static string DefaultMessage { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the font for the SnackBar message.
		/// </summary>
		public Font Font { get; set; } = DefaultFont;

		public static Font DefaultFont { get; set; } = Font.Default;

		/// <summary>
		/// Gets or sets the font color for the SnackBar message.
		/// </summary>
		public Color Foreground { get; set; } = DefaultForeground;

		public static Color DefaultForeground { get; set; } = Color.Default;

		/// <summary>
		/// Gets or sets the padding for the SnackBar message.
		/// </summary>
		public Thickness Padding { get; set; }

		public static Thickness DefaultPadding { get; set; } = new Thickness(0, 0, 0, 0);
	}
}