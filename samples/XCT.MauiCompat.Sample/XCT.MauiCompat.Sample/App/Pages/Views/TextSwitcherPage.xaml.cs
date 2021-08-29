using System.IO;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Xamarin.CommunityToolkit.UI.Views;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views
{
	public partial class TextSwitcherPage
	{
		public TextSwitcherPage()
			=> InitializeComponent();

		public ICommand UpdateTextCommand { get; }
			= new Command<TextSwitcher>(textSwitcher => textSwitcher.Text = Path.GetRandomFileName());
	}
}
