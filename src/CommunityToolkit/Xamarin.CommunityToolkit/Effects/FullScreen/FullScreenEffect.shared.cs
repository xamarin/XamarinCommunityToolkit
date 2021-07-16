using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Effects
{
	public abstract class FullScreenEffect
	{
		// TODO: Remove if not required
		public static bool InitialHasNavigationBar;

		/// <summary>
		/// Backing BindableProperty for the <see cref="IsPersistent"/> property.
		/// </summary>
		public static readonly BindableProperty IsPersistentProperty
	   = BindableProperty.CreateAttached("IsPersistent", typeof(bool), typeof(FullScreenEffect), null, defaultBindingMode: BindingMode.TwoWay);

		public static bool GetIsPersistent(BindableObject view)
			=> (bool)view.GetValue(IsPersistentProperty);

		public static void SetIsPersistent(BindableObject view, bool value)
			=> view.SetValue(IsPersistentProperty, value);
	}
}