using System;
using FormsCommunityToolkit.Effects.iOS.Effects;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(ItalicPlaceholderEffect), nameof(ItalicPlaceholderEffect))]
namespace FormsCommunityToolkit.Effects.iOS.Effects
{
	[Preserve(AllMembers = true)]
	public class ItalicPlaceholderEffect : PlatformEffect
	{
		private NSAttributedString _old;

		protected override void OnAttached()
		{
			var entry = Control as UITextField;
			if (entry != null && !string.IsNullOrWhiteSpace(entry.Placeholder))
			{
				_old = entry.AttributedPlaceholder;
				var entryFontSize = entry.Font.PointSize;
				entry.AttributedPlaceholder = new NSAttributedString(entry.Placeholder, font: UIFont.ItalicSystemFontOfSize(entryFontSize));
			}
		}

		protected override void OnDetached()
		{
			var entry = Control as UITextField;
			if (entry != null)
			{
				entry.AttributedPlaceholder = _old;
			}
		}
	}
}
