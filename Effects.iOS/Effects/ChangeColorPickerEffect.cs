using FormsCommunityToolkit.Effects.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Color = Xamarin.Forms.Color;


[assembly: ExportEffect(typeof(ChangeColorPickerEffect), nameof(ChangeColorPickerEffect))]
namespace FormsCommunityToolkit.Effects.iOS
{

	public class ChangeColorPickerEffect : PlatformEffect
	{
		private Color _color;

		protected override void OnAttached()
		{
			/*
			 * Text Color change when I select a value
			 */ 
			_color = (Color)Element.GetValue(ChangePickerColorEffect.ColorProperty);
            (Control as UITextField).AttributedPlaceholder = new Foundation.NSAttributedString((Control as UITextField).AttributedPlaceholder.Value, foregroundColor: _color.ToUIColor());


		}

		protected override void OnDetached()
		{
		}
	}
}