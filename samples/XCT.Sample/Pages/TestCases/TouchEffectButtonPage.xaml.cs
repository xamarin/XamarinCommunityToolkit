using System.Windows.Input;
using CommunityToolkit.Maui.ObjectModel;

namespace CommunityToolkit.Maui.Sample.Pages.TestCases
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