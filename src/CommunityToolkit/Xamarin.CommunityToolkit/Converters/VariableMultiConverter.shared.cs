using System;
using System.Globalization;
using System.Linq;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Xamarin.CommunityToolkit.Converters
{
	public class VariableMultiConverter : IMultiValueConverter
	{
		public MultiBindingCondition ConditionType { get; set; }

		public int Count { get; set; }

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values == null || !targetType.IsAssignableFrom(typeof(bool)))
				return false;

			var amount = 0;

			foreach (var value in values)
			{
				if (!(value is bool b))
					return false;

				if (ConditionType == MultiBindingCondition.All && !b)
					return false;

				if (ConditionType == MultiBindingCondition.Any && b)
					return true;

				if (ConditionType == MultiBindingCondition.Count && b)
					amount++;
			}

			return ConditionType switch
			{
				MultiBindingCondition.Any => false,
				MultiBindingCondition.Count => Count == amount,
				MultiBindingCondition.All => true,
				_ => true,
			};
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			if (!(value is bool b) || targetTypes.Any(t => !t.IsAssignableFrom(typeof(bool))))
				return null;

			if (b)
				return targetTypes.Select(t => ConditionType == MultiBindingCondition.All ? (object)true : (object)false).ToArray();
			else
				return null;
		}
	}
}
