using System.Collections;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts the incoming value to a <see cref="bool"/> indicating whether or not the value is null or empty.
	/// </summary>
	public class ListIsNullOrEmptyConverter : BaseNullableConverterOneWay<IEnumerable, bool>
	{
		/// <summary>
		/// Converts the incoming value to a <see cref="bool"/> indicating whether or not the value is null or empty.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>A <see cref="bool"/> indicating if the incoming value is null or empty.</returns>
		public override bool ConvertFrom(IEnumerable? value) => ConvertInternal(value);

		internal static bool ConvertInternal(IEnumerable? value)
		{
			if (value == null)
				return true;

			return !value.GetEnumerator().MoveNext();
		}
	}
}