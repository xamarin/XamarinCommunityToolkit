using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.TestCases
{
	public partial class TouchEffectButtonPage
	{
		ICommand command;

		public TouchEffectButtonPage()
			=> InitializeComponent();

		public ICommand Command => command ??= new Command(() => this.DisplayAlert("Command executed ", null, "OK"));
	}
}
