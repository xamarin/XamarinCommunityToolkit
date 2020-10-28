using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.CommunityToolkit.UI.Views
{
	[TypeConversion(typeof(Uri))]
	public class UriTypeConverter : TypeConverter
	{
		public override object ConvertFromInvariantString(string value) =>
			string.IsNullOrWhiteSpace(value) ? null : (object)new Uri(value, UriKind.RelativeOrAbsolute);
	}
}