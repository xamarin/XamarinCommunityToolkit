using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class LazyView<TView> : BaseLazyView where TView : View, new()
	{
		public override void LoadView()
		{
			View view = new TView { BindingContext = BindingContext };

			Content = view;

			SetIsLoaded(true);
		}
	}
}