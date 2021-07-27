using System;
namespace Xamarin.CommunityToolkit.Sample.ViewModels.Converters
{
	public class UtcDateTimeToLocalStringConverterViewModel : BaseViewModel
	{
		DateTime date = DateTime.UtcNow;

		public DateTime UtcDate
		{
			get => date;
			set => SetProperty(ref date, value);
		}

		DateTimeOffset utcDateTimeOffset = DateTimeOffset.UtcNow;

		public DateTimeOffset UtcDateTimeOffset
		{
			get => utcDateTimeOffset;
			set => SetProperty(ref utcDateTimeOffset, value);
		}
	}
}
