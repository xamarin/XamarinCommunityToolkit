﻿using System.Collections;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts the incoming value to a <see cref="bool"/> indicating whether or not the value is not null and not empty.
	/// </summary>
	public class ListIsNotNullOrEmptyConverter : NullableBaseConverterOneWay<IEnumerable, bool>
	{
		/// <summary>
		/// Converts the incoming value to a <see cref="bool"/> indicating whether or not the value is not null and not empty.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>A <see cref="bool"/> indicating if the incoming value is not null and not empty.</returns>
		public override bool ConvertFrom(IEnumerable? value) =>
			!ListIsNullOrEmptyConverter.ConvertInternal(value);
	}
}