using System.ComponentModel;
using System.Linq;
using UIKit;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.iOS.Effects
{
	/// <summary>
	/// Base implementation of the <see cref="SemanticEffectRouter" />
	/// </summary>
	/// <typeparam name="T">Effect</typeparam>
	public class SemanticEffectRouterBase<T> : Xamarin.Forms.Platform.iOS.PlatformEffect
		where T : Effect
	{
		public SemanticEffectRouterBase()
		{
		}

		protected override void OnAttached()
		{
			if (Control == null && Container == null)
				Element.PropertyChanged += SetupControl;
			else
				Update();
		}

		protected virtual void Update(UIView view, T effect)
		{
		}

		protected void Update()
		{
			var effect = (T)Element.Effects.FirstOrDefault(e => e is T);
			Update(Control ?? Container, effect);
		}

		void SetupControl(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Renderer")
			{
				Element.PropertyChanged -= SetupControl;
				Update();
			}
		}

		protected override void OnDetached() => Element.PropertyChanged -= SetupControl;
	}
}