using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Xamarin.CommunityToolkit.Sample.Pages.TestCases
{
	public partial class TouchEffectCollectionViewPage
	{
		public TouchEffectCollectionViewPage()
		{
			InitializeComponent();
			LongPressCommand = CommandFactory.Create(() => DisplayAlert("Long Press", null, "OK"));
		}

		public ICommand LongPressCommand { get; }
	}
}