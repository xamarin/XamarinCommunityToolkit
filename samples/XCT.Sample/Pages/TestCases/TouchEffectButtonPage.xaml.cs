using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;

namespace Xamarin.CommunityToolkit.Sample.Pages.TestCases
{
	public partial class TouchEffectButtonPage
	{
		public TouchEffectButtonPage()
		{
			Command = CommandHelper.Create(() => DisplayAlert("Command executed ", null, "OK"));
			InitializeComponent();
		}

		public ICommand Command { get; }
	}
}