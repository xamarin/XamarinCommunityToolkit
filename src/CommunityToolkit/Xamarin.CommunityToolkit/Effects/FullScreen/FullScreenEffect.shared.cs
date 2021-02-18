using System.Linq;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Effects
{
	public class FullScreenEffect
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

		/// <summary>
		/// Backing BindableProperty for the <see cref="Mode"/> property.
		/// </summary>
		public static readonly BindableProperty ModeProperty
	   = BindableProperty.CreateAttached("Mode", typeof(FullScreenMode), typeof(FullScreenEffect), FullScreenMode.Disabled, propertyChanged: OnModeChanged);

		public static FullScreenMode GetMode(BindableObject view)
			=> (FullScreenMode)view.GetValue(ModeProperty);

		public static void SetMode(BindableObject view, FullScreenMode value)
			=> view.SetValue(ModeProperty, value);

		static void OnModeChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (!(bindable is Page page))
				return;

			var oldEffect = page.Effects.FirstOrDefault(e => e is FullScreenEffectRouter);

			if (oldEffect != null)
			{
				page.Effects.Remove(oldEffect);
			}

			page.Effects.Add(new FullScreenEffectRouter());
		}
	}
}