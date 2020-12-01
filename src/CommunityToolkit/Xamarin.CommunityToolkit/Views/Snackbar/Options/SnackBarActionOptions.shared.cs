﻿using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views.Options
{
	public class SnackBarActionOptions
	{
		/// <summary>
		/// Gets or sets the action for the SnackBar action button.
		/// </summary>
		public Func<Task> Action { get; set; } = DefaultAction;

		public static Func<Task> DefaultAction { get; set; } = null;

		/// <summary>
		/// Gets or sets the text for the SnackBar action button.
		/// </summary>
		public string Text { get; set; } = DefaultText;

		public static string DefaultText { get; set; }

		/// <summary>
		/// Gets or sets the font for the SnackBar action button.
		/// </summary>
		public Font Font { get; set; } = DefaultFont;

		public static Font DefaultFont { get; set; } = Font.OfSize("Times New Roman", 14);

		/// <summary>
		/// Gets or sets the background color for the SnackBar action button.
		/// </summary>
		public Color BackgroundColor { get; set; } = DefaultBackgroundColor;

		public static Color DefaultBackgroundColor { get; set; } = Color.White;

		/// <summary>
		/// Gets or sets the font color for the SnackBar action button.
		/// </summary>
		public Color ForegroundColor { get; set; } = DefaultForegroundColor;

		public static Color DefaultForegroundColor { get; set; } = Color.Black;
	}
}