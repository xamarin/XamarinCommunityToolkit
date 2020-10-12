using System;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Converters
{
	public class DoubleToIntConverterViewModel : BaseViewModel
	{
		double input;

		public double Input
		{
			get => input;
			set
				{
					input = Convert.ToDouble(value);
					OnPropertyChanged(nameof(Input));
				}
		}
	}
}
