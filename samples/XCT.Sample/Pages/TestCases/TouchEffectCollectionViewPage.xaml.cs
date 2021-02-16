
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.TestCases
{
	public partial class TouchEffectCollectionViewPage
	{
		ICommand longPressCommand;

		public TouchEffectCollectionViewPage()
			=> InitializeComponent();

		public ICommand LongPressCommand => longPressCommand ??= new Command(() =>
		{
			this.DisplayAlert("Long Press", null, "OK");
		});
	}
}
