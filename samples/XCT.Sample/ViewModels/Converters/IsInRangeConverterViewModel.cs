using System;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Converters
{
	public class IsInRangeConverterViewModel : BaseViewModel
	{
		DateTime date = DateTime.Now.Date;
		DateTime startDate = DateTime.Now.Date.AddDays(-7);
		DateTime endDate = DateTime.Now.Date.AddDays(7);

		public DateTime StartDate
		{
			get => startDate;
			set => SetProperty(ref startDate, value, onChanged: RaisePropertiesChanged);
		}

		public DateTime EndDate
		{
			get => endDate;
			set => SetProperty(ref endDate, value, onChanged: RaisePropertiesChanged);
		}

		public DateTime Date
		{
			get => date;
			set => SetProperty(ref date, value);
		}

		void RaisePropertiesChanged()
		{
			OnPropertyChanged(nameof(StartDate));
			OnPropertyChanged(nameof(EndDate));
			OnPropertyChanged(nameof(Date));
		}
	}
}