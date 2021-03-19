using Xamarin.Forms;
using TargetElement = Xamarin.Forms.Application;
using XFPC = Xamarin.Forms.PlatformConfiguration;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.AndroidSpecific
{
	public static partial class NavigationBar
	{
		public static readonly BindableProperty ColorProperty = BindableProperty.Create(
			"Color", typeof(Color), typeof(TargetElement), Color.Default, propertyChanged: (_, _, newValue) => SetColor((Color)newValue));

		public static readonly BindableProperty StyleProperty = BindableProperty.Create(
			"Style", typeof(NavigationBarStyle), typeof(TargetElement), NavigationBarStyle.Default, propertyChanged: (_, _, newValue) => SetStyle((NavigationBarStyle)newValue));

		public static Color GetColor(BindableObject element) =>
			(Color)element.GetValue(ColorProperty);

		public static NavigationBarStyle GetStyle(BindableObject element) =>
			(NavigationBarStyle)element.GetValue(StyleProperty);

		public static IPlatformElementConfiguration<XFPC.Android, TargetElement> SetColor(this IPlatformElementConfiguration<XFPC.Android, TargetElement> config, Color color)
		{
			SetColor(color);
			return config;
		}

		public static IPlatformElementConfiguration<XFPC.Android, TargetElement> SetStyle(this IPlatformElementConfiguration<XFPC.Android, TargetElement> config, NavigationBarStyle style)
		{
			SetStyle(style);
			return config;
		}

		static partial void SetColor(Color color);

		static partial void SetStyle(NavigationBarStyle style);
	}

	public enum NavigationBarStyle
	{
		Default = 0,
		LightContent = 1,
		DarkContent = 2
	}
}
