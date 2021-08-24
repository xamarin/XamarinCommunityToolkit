using System;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Xaml;

namespace Xamarin.CommunityToolkit.Core
{
	[TypeConversion(typeof(FileMediaSource))]
	public sealed class FileMediaSourceConverter : System.ComponentModel.TypeConverter
	{
		public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
		{
			if (value is not string text)
				throw new InvalidOperationException("Only typeof(string) allowed");

			return text != null
				? (FileMediaSource)MediaSource.FromFile(text)
				: throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(FileMediaSource)}");
		}
	}
}