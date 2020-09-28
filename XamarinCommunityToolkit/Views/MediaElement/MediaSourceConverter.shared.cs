using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.CommunityToolkit.UI.Views
{
	[TypeConversion(typeof(MediaSource))]
	public sealed class MediaSourceConverter : TypeConverter
	{
		public override object ConvertFromInvariantString(string value)
		{
			if (value != null)
			{
				return Uri.TryCreate(value, UriKind.Absolute, out var uri) && uri.Scheme != "file" ? MediaSource.FromUri(uri) : MediaSource.FromFile(value);
			}

			throw new InvalidOperationException(string.Format("Cannot convert \"{0}\" into {1}", value, typeof(MediaSource)));
		}
	}
}