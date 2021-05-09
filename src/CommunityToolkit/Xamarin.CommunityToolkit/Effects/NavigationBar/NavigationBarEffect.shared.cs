using System;
using Xamarin.Forms;
using TargetElement = Xamarin.Forms.Application;
using XFPC = Xamarin.Forms.PlatformConfiguration;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.AndroidSpecific
{
	public static partial class NavigationBarEffect
	{
		/// <summary>
		/// Sets the color of application's navigation bar
		/// </summary>
		public static readonly BindableProperty ColorProperty = BindableProperty.Create(
			"Color", typeof(Color), typeof(TargetElement), Color.Default, propertyChanged: ColorChanged);

		static void ColorChanged(BindableObject bindable, object oldValue, object newValue) => SetColor((Color)newValue);

		/// <summary>
		/// Sets the style of application's navigation bar
		/// </summary>
		public static readonly BindableProperty StyleProperty = BindableProperty.Create(
			"Style", typeof(NavigationBarStyle), typeof(TargetElement), NavigationBarStyle.Default, propertyChanged: StyleChanged);

		static void StyleChanged(BindableObject bindable, object oldValue, object newValue) => SetStyle((NavigationBarStyle)newValue);

		/// <summary>
		/// Needed for BindableProperty to work. Don't call this method.
		/// </summary>
		public static Color GetColor(BindableObject element) => throw new NotImplementedException();

		/// <summary>
		/// Needed for BindableProperty to work. Don't call this method.
		/// </summary>
		public static NavigationBarStyle GetStyle(BindableObject element) => throw new NotImplementedException();

		/// <summary>
		/// Sets the color of application's navigation bar
		/// </summary>
		/// <param name="color">Color to set</param>
		public static IPlatformElementConfiguration<XFPC.Android, TargetElement> SetNavigationBarColor(this IPlatformElementConfiguration<XFPC.Android, TargetElement> config, Color color)
		{
			SetColor(color);
			return config;
		}

		/// <summary>
		/// Sets the style of application's navigation bar
		/// </summary>
		/// <param name="style">Style to set</param>
		public static IPlatformElementConfiguration<XFPC.Android, TargetElement> SetNavigationBarStyle(this IPlatformElementConfiguration<XFPC.Android, TargetElement> config, NavigationBarStyle style)
		{
			SetStyle(style);
			return config;
		}

		static partial void SetColor(Color color);

		static partial void SetStyle(NavigationBarStyle style);
	}
}
