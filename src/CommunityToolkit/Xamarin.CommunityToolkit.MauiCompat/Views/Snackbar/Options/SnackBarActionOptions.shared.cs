using Font = Microsoft.Maui.Font;﻿using System;using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.UI.Views.Options
{
	public class SnackBarActionOptions
	{
		/// <summary>
		/// Gets or sets the action for the SnackBar action button.
		/// </summary>
		public Func<Task>? Action { get; set; } = DefaultAction;

		public static Func<Task>? DefaultAction { get; set; } = null;

		/// <summary>
		/// Gets or sets the text for the SnackBar action button.
		/// </summary>
		public string Text { get; set; } = DefaultText;

		public static string DefaultText { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the font for the SnackBar action button.
		/// </summary>
		public Font Font { get; set; } = DefaultFont;

		public static Font DefaultFont { get; set; } = Font.Default;

		/// <summary>
		/// Gets or sets the background color for the SnackBar action button.
		/// </summary>
		public Color BackgroundColor { get; set; } = DefaultBackgroundColor;

		public static Color DefaultBackgroundColor { get; set; } = default(Microsoft.Maui.Graphics.Color);

		/// <summary>
		/// Gets or sets the font color for the SnackBar action button.
		/// </summary>
		public Color ForegroundColor { get; set; } = DefaultForegroundColor;

		public static Color DefaultForegroundColor { get; set; } = default(Microsoft.Maui.Graphics.Color);

		/// <summary>
		/// Gets or sets the padding for the SnackBar message.
		/// </summary>
		public Thickness Padding { get; set; }

		public static Thickness DefaultPadding { get; set; } = new Thickness(0, 0, 0, 0);
	}
}