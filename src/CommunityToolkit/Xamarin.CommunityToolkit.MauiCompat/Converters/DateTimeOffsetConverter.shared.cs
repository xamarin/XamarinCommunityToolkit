using System;using Microsoft.Extensions.Logging;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts <see cref="DateTimeOffset"/> to <see cref="DateTime"/> and back.
	/// </summary>
	public class DateTimeOffsetConverter : BaseConverter<DateTimeOffset, DateTime>
	{
		/// <summary>
		/// Converts <see cref="DateTime"/> back to <see cref="DateTimeOffset"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The <see cref="DateTimeOffset"/> value.</returns>
		public override DateTimeOffset ConvertBackTo(DateTime value) => value.Kind switch
		{
			DateTimeKind.Local => new DateTimeOffset(DateTime.SpecifyKind(value, DateTimeKind.Unspecified), DateTimeOffset.Now.Offset),
			DateTimeKind.Utc => new DateTimeOffset(value, DateTimeOffset.UtcNow.Offset),
			_ => new DateTimeOffset(value, TimeSpan.Zero)
		};

		/// <summary>
		/// Converts <see cref="DateTimeOffset"/> to <see cref="DateTime"/>
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The <see cref="DateTime"/> value.</returns>
		public override DateTime ConvertFrom(DateTimeOffset value) => value.DateTime;
	}
}