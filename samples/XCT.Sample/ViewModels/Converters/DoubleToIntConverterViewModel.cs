namespace Xamarin.CommunityToolkit.Sample.ViewModels.Converters
{
	public class DoubleToIntConverterViewModel : BaseViewModel
	{
		double index;

		public double Input
		{
			get => index;
			set => SetProperty(ref index, value);
		}
	}
}