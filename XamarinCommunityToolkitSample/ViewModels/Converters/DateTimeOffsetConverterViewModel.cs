using System;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Converters
{
	public class DateTimeOffsetConverterViewModel : BaseViewModel
	{
		DateTimeOffset theDate = DateTimeOffset.Now;

		public DateTimeOffset TheDate
		{
			get => theDate;
			set => Set(ref theDate, value);
		}
	}
}