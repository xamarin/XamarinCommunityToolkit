using Xamarin.CommunityToolkit.Behaviors;

namespace Xamarin.CommunityToolkit.Sample.Pages.Behaviors
{
	public partial class MaxLengthReachedBehaviorPage : BasePage
	{
		public MaxLengthReachedBehaviorPage()
			=> InitializeComponent();

		void MaxLengthReachedBehavior_MaxLengthReached(object? sender, MaxLengthReachedEventArgs e)
			=> nextEntry.Focus();
	}
}