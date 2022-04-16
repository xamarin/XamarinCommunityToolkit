using System;using Microsoft.Extensions.Logging;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Checks if the value is between minValue and maxValue, returning true if the value is within the range and false if the value is out of the range.
	/// </summary>
	public class IsInRangeConverter : BaseConverterOneWay<IComparable, bool>
	{
		/// <summary>
		/// Backing BindableProperty for the <see cref="MinValue"/> property.
		/// </summary>
		public static readonly BindableProperty MinValueProperty = BindableProperty.Create(nameof(MinValue), typeof(object), typeof(IsInRangeConverter));

		/// <summary>
		/// Backing BindableProperty for the <see cref="MaxValue"/> property.
		/// </summary>
		public static readonly BindableProperty MaxValueProperty = BindableProperty.Create(nameof(MaxValue), typeof(object), typeof(IsInRangeConverter));

		/// <summary>
		/// Gets or sets the minimum value of the range for the <see cref="IsInRangeConverter"/>. This is a bindable property.
		/// </summary>
		public object MinValue
		{
			get => GetValue(MinValueProperty);
			set => SetValue(MinValueProperty, value);
		}

		/// <summary>
		/// Gets or sets the maximum value of the range for the <see cref="IsInRangeConverter"/>. This is a bindable property.
		/// </summary>
		public object MaxValue
		{
			get => GetValue(MaxValueProperty);
			set => SetValue(MaxValueProperty, value);
		}

		/// <summary>
		/// Checks if the value is between minValue and maxValue, returning true if the value is within the range and false if the value is out of the range.
		/// </summary>
		/// <param name="value">The object to compare.</param>
		/// <returns>True if <paramref name="value"/> and <paramref name="parameter"/> are equal, False if they are not equal.</returns>
		public override bool ConvertFrom(IComparable value)
		{
			if (MinValue is not IComparable)
				throw new ArgumentException("is expected to implement IComparable interface.", nameof(MinValue));

			if (MaxValue is not IComparable)
				throw new ArgumentException("is expected to implement IComparable interface.", nameof(MaxValue));

			return value.CompareTo(MinValue) >= 0 && value.CompareTo(MaxValue) <= 0;
		}
	}
}