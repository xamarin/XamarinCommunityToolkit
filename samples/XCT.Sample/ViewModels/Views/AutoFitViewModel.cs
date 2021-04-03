namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views
{
	public class AutoFitViewModel : BaseViewModel
	{
		string textToFit = "Change the text to see the autofit in action!";

		public string TextToFit
		{
			get => textToFit;
			set => SetProperty(ref textToFit, value);
		}
	}
}
