namespace Xamarin.CommunityToolkit.Sample.ViewModels.TestCases
{
	public class TabViewBindingViewModel : BaseViewModel
	{
		string? message;

		public string? Message
		{
			get => message;
			set => SetProperty(ref message, value);
		}

		public TabViewBindingViewModel() => Message = "MainPage - Binding Message OK";
	}
}