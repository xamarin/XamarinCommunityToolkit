using System;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Xaml;

namespace Xamarin.CommunityToolkit.Core
{
	[TypeConversion(typeof(FileMediaSource))]
	public sealed class FileMediaSourceConverter : TypeConverter
	{
		public override object ConvertFromInvariantString(string value) =>
			value != null
				? (FileMediaSource)MediaSource.FromFile(value)
				: throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(FileMediaSource)}");
	}
}