using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	public static partial class StatusBar
	{
		/// <summary>
		/// Sets the color of application's status bar
		/// </summary>
		public static readonly BindableProperty ColorProperty = BindableProperty.Create(
			"Color", typeof(Color), typeof(StatusBar), Color.Default, propertyChanged: ColorChanged);

		static void ColorChanged(BindableObject bindable, object oldValue, object newValue) => SetColor((Color)newValue);

		/// <summary>
		/// Sets style of application's status bar
		/// </summary>
		public static readonly BindableProperty StyleProperty = BindableProperty.Create(
			"Style", typeof(StatusBarStyle), typeof(StatusBar), StatusBarStyle.Default, propertyChanged: StyleChanged);

		static void StyleChanged(BindableObject bindable, object oldValue, object newValue) => SetStyle((StatusBarStyle)newValue);

		/// <summary>
		/// Needed for BindableProperty to work. Don't call this method.
		/// </summary>
		public static Color GetColor(BindableObject element) => throw new NotImplementedException();

		/// <summary>
		/// Needed for BindableProperty to work. Don't call this method.
		/// </summary>
		public static StatusBarStyle GetStyle(BindableObject element) => throw new NotImplementedException();

		/// <summary>
		/// Sets the color of application's status bar
		/// </summary>
		/// <param name="color">Color to set</param>
		public static void SetColor(Color color) => PlatformSetColor(color);

		/// <summary>
		/// Sets style of application's status bar
		/// </summary>
		/// <param name="style">Style to set</param>
		public static void SetStyle(StatusBarStyle style) => PlatformSetStyle(style);

		static partial void PlatformSetColor(Color color);

		static partial void PlatformSetStyle(StatusBarStyle style);
	}
}