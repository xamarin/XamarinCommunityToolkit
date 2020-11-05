using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views.TabView
{
	public partial class TabItemAnimationPage : BasePage
	{
		public TabItemAnimationPage() => InitializeComponent();
	}

	public class CustomTabViewItemAnimation : ITabViewItemAnimation
	{
		protected uint SelectedAnimationLength { get; } = 500;

		protected uint DeSelectedAnimationLength { get; } = 1000;

		public Task OnDeSelected(View tabViewItem)
		{
			var tcs = new TaskCompletionSource<bool>();

			if (!(tabViewItem.FindByName("TabIcon") is Image icon))
				return Task.FromResult(false);

			var deSelectedAnimation = new Animation();

			deSelectedAnimation.WithConcurrent((f) => icon.Opacity = f, 1, 0.75, null, 0, 0.01);
			deSelectedAnimation.WithConcurrent((f) => icon.Scale = f, 1.2, 1, null, 0, 0.5);

			deSelectedAnimation.Commit(tabViewItem, nameof(OnDeSelected), length: DeSelectedAnimationLength,
				finished: (v, t) => tcs.SetResult(true));

			return tcs.Task;
		}

		public Task OnSelected(View tabViewItem)
		{
			var tcs = new TaskCompletionSource<bool>();

			var icon = tabViewItem.FindByName<Image>("TabIcon");

			var selectedAnimation = new Animation();

			selectedAnimation.WithConcurrent((f) => icon.RotationY = f, 75, 0, Easing.CubicOut);
			selectedAnimation.WithConcurrent((f) => icon.Opacity = f, 0.75, 1, null, 0, 0.01);
			selectedAnimation.WithConcurrent((f) => icon.Scale = f, 1, 1.2, null, 0, 0.5);

			selectedAnimation.Commit(icon, nameof(OnSelected), length: SelectedAnimationLength,
			   finished: (v, t) => tcs.SetResult(true));

			return tcs.Task;
		}
	}
}