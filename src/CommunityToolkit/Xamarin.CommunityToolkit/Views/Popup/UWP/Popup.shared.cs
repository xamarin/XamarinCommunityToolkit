using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views.WindowsSpecific
{
	public static class Popup
	{
		public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(
			"BorderColor", typeof(Color), typeof(Views.Popup), default(Color));

		public static void SetBorderColor(BindableObject? element, Color color) =>
			element?.SetValue(BorderColorProperty, color);

		public static Color GetBorderColor(BindableObject? element) =>
			(Color)(element?.GetValue(BorderColorProperty) ?? throw new NullReferenceException());
	}
}