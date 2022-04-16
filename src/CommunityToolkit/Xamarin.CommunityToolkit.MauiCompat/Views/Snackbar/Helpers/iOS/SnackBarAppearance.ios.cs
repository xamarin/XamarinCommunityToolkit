using Microsoft.Maui;using Microsoft.Maui.Controls.Compatibility.Platform.iOS;using Microsoft.Maui.Graphics;using Microsoft.Maui.Controls.Platform;using Microsoft.Extensions.DependencyInjection;﻿using UIKit;
using Xamarin.CommunityToolkit.Helpers;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.iOS
{
	class NativeSnackBarAppearance
	{
		public UIColor Background { get; set; } = XCT.IsiOS13OrNewer ? UIColor.SystemGray : UIColor.Gray;

		public UIColor Foreground { get; set; } = DefaultColor;

		public UIFont Font { get; set; } = DefaultFont;

		public UITextAlignment TextAlignment { get; set; } = UITextAlignment.Left;

		public static UIColor DefaultColor { get; } = XCT.IsiOS13OrNewer ? default(Microsoft.Maui.Graphics.Color).ToUIColor() : UIColor.White;

		public static UIFont DefaultFont { get; } = Microsoft.Maui.Font.Default.ToUIFont(Microsoft.Maui.Controls.Application.Current?.Handler.MauiContext?.Services.GetRequiredService<IFontManager>());
	}

	static class NativeSnackButtonAppearance
	{
		public static UILineBreakMode LineBreakMode { get; set; } = UILineBreakMode.MiddleTruncation;

		public static UIColor DefaultColor { get; } = default(Microsoft.Maui.Graphics.Color).ToUIColor();

		public static UIFont DefaultFont { get; } = Microsoft.Maui.Font.Default.ToUIFont(Microsoft.Maui.Controls.Application.Current?.Handler.MauiContext?.Services.GetRequiredService<IFontManager>());
	}
}