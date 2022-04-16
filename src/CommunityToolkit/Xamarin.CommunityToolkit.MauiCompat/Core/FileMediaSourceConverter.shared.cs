using System;using Microsoft.Extensions.Logging;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Xaml;

namespace Xamarin.CommunityToolkit.Core
{
	[System.ComponentModel.TypeConverter(typeof(FileMediaSource))]
	public sealed class FileMediaSourceConverter : System.ComponentModel.TypeConverter
	{
		public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object valueObject)
		{
			if (valueObject is not string value){throw new InvalidOperationException("Only typeof(string) allowed");}return value != null
				? (FileMediaSource)MediaSource.FromFile(value)
				: throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(FileMediaSource)}");
		}
	}
}