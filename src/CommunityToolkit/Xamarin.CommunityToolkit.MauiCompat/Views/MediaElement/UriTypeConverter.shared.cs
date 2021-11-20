using System;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Xaml;

namespace Xamarin.CommunityToolkit.UI.Views
{
	[System.ComponentModel.TypeConverter(typeof(Uri))]
	public class UriTypeConverter : System.ComponentModel.TypeConverter
	{
		public override object? ConvertFrom(System.ComponentModel.ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object valueObject)
		{
			if (valueObject is not string value)
			    throw new InvalidOperationException("Only typeof(string) allowed");

			return string.IsNullOrWhiteSpace(value) ? null : new Uri(value, UriKind.RelativeOrAbsolute);
		}
	}
}