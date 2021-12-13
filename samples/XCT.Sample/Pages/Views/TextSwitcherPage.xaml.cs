using System.IO;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

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