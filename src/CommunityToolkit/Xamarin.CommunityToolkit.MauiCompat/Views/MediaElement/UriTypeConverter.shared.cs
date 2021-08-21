using System;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Xaml;

namespace Xamarin.CommunityToolkit.UI.Views
{
	[TypeConversion(typeof(Uri))]
	public class UriTypeConverter : TypeConverter
	{
		public override object? ConvertFromInvariantString(string? value) =>
			string.IsNullOrWhiteSpace(value) ? null : new Uri(value, UriKind.RelativeOrAbsolute);
	}
}