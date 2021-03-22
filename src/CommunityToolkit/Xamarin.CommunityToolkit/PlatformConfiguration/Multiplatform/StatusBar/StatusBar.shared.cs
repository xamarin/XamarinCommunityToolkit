using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	public static partial class StatusBar
	{
		public static readonly BindableProperty ColorProperty = BindableProperty.Create(
			"Color", typeof(Color), typeof(StatusBar), Color.Default, propertyChanged: (_, _, newValue) => SetColor((Color)newValue));

		public static readonly BindableProperty StyleProperty = BindableProperty.Create(
			"Style", typeof(StatusBarStyle), typeof(StatusBar), StatusBarStyle.Default, propertyChanged: (_, _, newValue) => SetStyle((StatusBarStyle)newValue));

		public static Color GetColor(BindableObject bindable) =>
			(Color)bindable.GetValue(ColorProperty);

		public static StatusBarStyle GetStyle(BindableObject bindable) =>
			(StatusBarStyle)bindable.GetValue(StyleProperty);

		public static void SetColor(Color color) => PlatformSetColor(color);

		public static void SetStyle(StatusBarStyle style) => PlatformSetStyle(style);

		static partial void PlatformSetColor(Color color);

		static partial void PlatformSetStyle(StatusBarStyle style);
	}

	public enum StatusBarStyle
	{
		Default = 0,
		LightContent = 1,
		DarkContent = 2
	}
}