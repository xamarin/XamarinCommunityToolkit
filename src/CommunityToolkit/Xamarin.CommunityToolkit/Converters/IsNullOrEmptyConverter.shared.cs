using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts the incoming value to a <see cref="bool"/> indicating whether or not the value is null or empty.
	/// </summary>
	public class IsNullOrEmptyConverter : BaseConverterOneWay<object, bool>
	{
		protected override bool AllowsNull => true;

		/// <summary>
		/// Converts the incoming value to a <see cref="bool"/> indicating whether or not the value is null or empty.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>A <see cref="bool"/> indicating if the incoming value is null or empty.</returns>
		public override bool ConvertFrom(object? value) => ConvertInternal(value);

		internal static bool ConvertInternal(object? value) =>
			value == null || (value is string str && string.IsNullOrWhiteSpace(str));
	}
}