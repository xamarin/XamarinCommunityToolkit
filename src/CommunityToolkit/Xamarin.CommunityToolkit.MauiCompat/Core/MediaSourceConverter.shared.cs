using System;using Microsoft.Extensions.Logging;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Core
{
	public sealed class MediaSourceConverter : System.ComponentModel.TypeConverter
	{
		public override object? ConvertFrom(System.ComponentModel.ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object valueObject)
		{
			if (valueObject is not string value){throw new InvalidOperationException("Only typeof(string) allowed");}if (value == null)
				throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(MediaSource)}");

			return Uri.TryCreate(value, UriKind.Absolute, out var uri) && uri.Scheme != "file"
				? MediaSource.FromUri(uri)
				: MediaSource.FromFile(value);
		}
	}
}