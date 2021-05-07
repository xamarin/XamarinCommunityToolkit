using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Xamarin.CommunityToolkit.Sample.Pages.TestCases
{
	public partial class TouchEffectButtonPage
	{
		public TouchEffectButtonPage()
		{
			Command = CommandFactory.Create(() => DisplayAlert("Command executed ", null, "OK"));
			InitializeComponent();
		}

		public ICommand Command { get; }
	}
}