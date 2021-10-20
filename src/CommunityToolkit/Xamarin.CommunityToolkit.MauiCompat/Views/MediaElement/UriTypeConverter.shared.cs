using System;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Xaml;

namespace Xamarin.CommunityToolkit.UI.Views
{
	[System.ComponentModel.TypeConverter(typeof(Uri))]
	public class UriTypeConverter : System.ComponentModel.TypeConverter
	{
		public override object? ConvertFrom(System.ComponentModel.ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
		{
			if (value is not string text)
				throw new InvalidOperationException("Only typeof(string) allowed");

			return string.IsNullOrWhiteSpace(text) ? null : new Uri(text, UriKind.RelativeOrAbsolute);
		}
	}
}