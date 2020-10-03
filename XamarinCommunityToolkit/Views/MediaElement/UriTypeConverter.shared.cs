using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.CommunityToolkit.UI.Views
{
	[TypeConversion(typeof(Uri))]
	public class UriTypeConverter : TypeConverter
	{
		public override object ConvertFromInvariantString(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				return null;
			return new Uri(value, UriKind.RelativeOrAbsolute);
		}
	}
}
