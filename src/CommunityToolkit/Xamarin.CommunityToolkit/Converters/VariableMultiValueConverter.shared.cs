using System;
using System.Globalization;
using System.Linq;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Xamarin.CommunityToolkit.Converters
{
	public class VariableMultiValueConverter : MultiValueConverterExtension, IMultiValueConverter
	{
		public MultiBindingCondition ConditionType { get; set; }

		public int Count { get; set; }

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values == null || values.Length == 0)
				return false;

			var boolValues = values.OfType<bool>().ToArray();

			if (boolValues.Length != values.Length)
				return false;

			var count = boolValues.Count(v => v);

			return ConditionType switch
			{
				MultiBindingCondition.Any => count >= 1,
				MultiBindingCondition.None => count == 0,
				MultiBindingCondition.Exact => count == Count,
				MultiBindingCondition.GreaterThan => count > Count,
				MultiBindingCondition.LessThan => count < Count,
				_ => count == boolValues.Count(),
			};
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			if (!(value is bool boolValue) || targetTypes.Any(t => !t.IsAssignableFrom(typeof(bool))))
				return null;

			return boolValue ? targetTypes.Select(t => ConditionType == MultiBindingCondition.All).OfType<object>().ToArray() : null;
		}
	}
}
