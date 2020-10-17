using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.Forms.Xaml;

namespace Xamarin.CommunityToolkit.Sample.Pages.Behaviors
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MaxLengthReachedBehaviorPage : BasePage
	{
		public MaxLengthReachedBehaviorPage()
			=> InitializeComponent();

		void MaxLengthReachedBehavior_MaxLengthReached(object sender, MaxLengthReachedEventArgs e)
			=> nextEntry.Focus();
	}
}