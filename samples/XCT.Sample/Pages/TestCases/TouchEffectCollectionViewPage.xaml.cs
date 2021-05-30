using System.Windows.Input;
using CommunityToolkit.Maui.ObjectModel;

namespace CommunityToolkit.Maui.Sample.Pages.TestCases
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
