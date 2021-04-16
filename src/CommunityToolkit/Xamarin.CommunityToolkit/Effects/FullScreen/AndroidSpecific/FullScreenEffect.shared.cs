using System;
using System.Linq;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Effects.AndroidSpecific
{
	public class FullScreenEffect : Effects.FullScreenEffect
	{
		[Flags]
		public enum FullScreenMode
		{
			Disabled = 0,
			Enabled = 1 << 0,
			ImmersiveDroid = 1 << 1,
			StickyImmersiveDroid = 1 << 2,
			LeanBackDroid = 1 << 3
		}

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