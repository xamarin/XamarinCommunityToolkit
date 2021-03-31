using System;
using System.Globalization;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class AspectToDisplayAspectModeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((Aspect)value).ToDisplayAspectMode();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
