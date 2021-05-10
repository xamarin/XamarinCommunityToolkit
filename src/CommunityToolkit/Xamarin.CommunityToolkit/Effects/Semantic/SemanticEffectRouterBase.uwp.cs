using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UWP.Effects
{
	/// <summary>
	/// Base implementation of the <see cref="SemanticEffectRouter" />
	/// </summary>
	/// <typeparam name="T">Effect</typeparam>
	public class SemanticEffectRouterBase<T> : Xamarin.Forms.Platform.UWP.PlatformEffect
		where T : Effect
	{
		public SemanticEffectRouterBase()
		{
		}

		protected override void OnAttached()
		{
			if (Control == null)
				Element.PropertyChanged += SetupControl;
			else
				Update();
		}

		protected virtual void Update(FrameworkElement view, T effect)
		{
		}

		protected void Update()
		{
			var effect = (T)Element.Effects.FirstOrDefault(e => e is T);
			Update(Control, effect);
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