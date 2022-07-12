namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts an <see cref="int"/> to corresponding <see cref="bool"/> and vice versa.
	/// </summary>
	public class IntToBoolConverter : BaseConverter<int, bool>
	{
		/// <summary>
		/// Converts back <see cref="bool"/> to corresponding <see cref="int"/>.
		/// </summary>
		/// <param name="value"><see cref="bool"/> value.</param>
		/// <returns>0 if the value is False, otherwise 1 if the value is True.</returns>
		public override int ConvertBackTo(bool value) => value ? 1 : 0;

		/// <summary>
		/// Converts an <see cref="int"/> to corresponding <see cref="bool"/>.
		/// </summary>
		/// <param name="value"><see cref="int"/> value.</param>
		/// <returns>False if the value is 0, otherwise if the value is anything but 0 it returns True.</returns>
		public override bool ConvertFrom(int value) => value != 0;
	}
}