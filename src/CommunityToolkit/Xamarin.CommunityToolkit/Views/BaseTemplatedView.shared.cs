using System.ComponentModel;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract class BaseTemplatedView<TControl> : TemplatedView where TControl : View, new()
	{
		protected TControl Control { get; private set; }

		public BaseTemplatedView()
			=> ControlTemplate = new ControlTemplate(typeof(TControl));

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			Control.BindingContext = BindingContext;
		}

		protected override void OnChildAdded(Element child)
		{
			if (Control == null && child is TControl content)
			{
				Control = content;
				OnControlInitialized(Control);
			}

			base.OnChildAdded(child);
		}

		protected abstract void OnControlInitialized(TControl control);
	}
}