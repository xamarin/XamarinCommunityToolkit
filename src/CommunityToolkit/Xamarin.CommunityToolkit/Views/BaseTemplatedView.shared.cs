using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views.Internals
{
	/// <summary>
	/// Abstract class that templated views should inherit
	/// </summary>
	/// <typeparam name="TControl">The type of the control that this template will be used for</typeparam>
	public abstract class BaseTemplatedView<TControl> : TemplatedView where TControl : View, new()
	{
		protected TControl Control { get; private set; }

		/// <summary>
		/// Constructor of <see cref="BaseTemplatedView" />
		/// </summary>
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