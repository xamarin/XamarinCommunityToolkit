namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts true to false and false to true. Simple as that!
	/// </summary>
	public class InvertedBoolConverter : BaseConverter<bool, bool>
	{
		/// <summary>
		/// Converts a <see cref="bool"/> to its inverse value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>An inverted <see cref="bool"/> from the one coming in.</returns>
		public override bool ConvertBackTo(bool value) => ConvertFrom(value);

		/// <summary>
		/// Converts a <see cref="bool"/> to its inverse value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>An inverted <see cref="bool"/> from the one coming in.</returns>
		public override bool ConvertFrom(bool value) => !value;
	}
}