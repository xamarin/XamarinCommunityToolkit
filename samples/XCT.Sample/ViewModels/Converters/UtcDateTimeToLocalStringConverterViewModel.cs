using System;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Converters
{
	public class UtcDateTimeToLocalStringConverterViewModel : BaseViewModel
	{
		DateTime utcDateTime = DateTime.Now.ToUniversalTime();

		public DateTime UtcDateTime
		{
			get => utcDateTime;
			set => SetProperty(ref utcDateTime, value);
		}

		DateTimeOffset utcDateTimeOffset = DateTimeOffset.Now.ToUniversalTime();

		public DateTimeOffset UtcDateTimeOffset
		{
			get => utcDateTimeOffset;
			set => SetProperty(ref utcDateTimeOffset, value);
		}
	}
}
