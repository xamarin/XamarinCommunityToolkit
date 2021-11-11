using System;
using System.Linq;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.iOS.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(LifeCycleEffectRouter), nameof(LifecycleEffect))]

namespace Xamarin.CommunityToolkit.iOS.Effects
{
	/// <summary>
	/// iOS implementation of the <see cref="LifecycleEffect" />
	/// </summary>
	public class LifeCycleEffectRouter : PlatformEffect
	{
		LifecycleEffect? lifeCycleEffect;

		protected override void OnAttached()
		{
			lifeCycleEffect = Element.Effects.OfType<LifecycleEffect>().FirstOrDefault() ??
				throw new ArgumentNullException($"The effect {nameof(LifecycleEffect)} can't be null.");

			Element.PropertyChanged += OnPropertyChanged;
		}

		void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Renderer" && lifeCycleEffect != null)
			{
				var result = Platform.GetRenderer((VisualElement)Element);

				if (result != null)
					lifeCycleEffect.RaiseLoadedEvent(Element);
				else
				{
					lifeCycleEffect.RaiseUnloadedEvent(Element);
					lifeCycleEffect = null;
					Element.PropertyChanged -= OnPropertyChanged;
				}
			}
		}

		protected override void OnDetached()
		{
		}
	}
}