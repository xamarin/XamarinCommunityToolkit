using System;
using System.Linq;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.AndroidSpecific
{
	public class FullScreenEffect : Effects.FullScreenEffect
	{
		[Flags]
		public enum FullScreenMode
		{
			Default = 0,
			Disabled = 1 << 0,
			Enabled = 1 << 1,
			ImmersiveDroid = 1 << 2,
			StickyImmersiveDroid = 1 << 3,
			LeanBackDroid = 1 << 4
		}

		/// <summary>
		/// Backing BindableProperty for the <see cref="Mode"/> property.
		/// </summary>
		public static readonly BindableProperty ModeProperty
			= BindableProperty.CreateAttached("Mode", typeof(FullScreenMode), typeof(FullScreenEffect), FullScreenMode.Default, propertyChanged: OnModeChanged);

		public static FullScreenMode GetMode(BindableObject view)
			=> (FullScreenMode)view.GetValue(ModeProperty);

		public static void SetMode(BindableObject view, FullScreenMode value)
			=> view.SetValue(ModeProperty, value);

		static void OnModeChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is not Page page)
			{
				return;
			}

			var oldEffect = page.Effects.FirstOrDefault(e => e is FullScreenEffectRouter);

			if (oldEffect != null)
			{
				page.Effects.Remove(oldEffect);
			}

			page.Effects.Add(new FullScreenEffectRouter());
		}
	}
}