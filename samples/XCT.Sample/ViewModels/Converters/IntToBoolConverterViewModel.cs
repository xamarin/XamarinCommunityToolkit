using System;
namespace Xamarin.CommunityToolkit.Sample.ViewModels.Converters
{
	public class IntToBoolConverterViewModel : BaseViewModel
	{
		int index;
		public int Number
		{
			get => index;
			set
			{
				index = value;
				OnPropertyChanged(nameof(Number));
			}
		}
	}
}
