using System;
using System.Linq;
using CoreFoundation;
using Foundation;
using UIKit;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.iOS.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(PlatformOtpEffect), nameof(OtpEffect))]

namespace Xamarin.CommunityToolkit.iOS.Effects
{
	public class PlatformOtpEffect : PlatformEffect
	{
		OtpEffect? otpEffect;

		protected override void OnAttached()
		{
			otpEffect = (OtpEffect)Element.Effects
				.FirstOrDefault(e => e is OtpEffect);
			if (otpEffect != null
				&& UIDevice.CurrentDevice.CheckSystemVersion(12, 0)
				&& Control is UITextField textField)
			{
				textField.TextContentType = UITextContentType.OneTimeCode;
				textField.EditingChanged += OnTextChanged;
			}
		}

		/// <summary>
		/// To detect if an otp code is filled in, the text has to be observed.
		/// Since every letter of the filled text will trigger this event,
		/// many changes in a short time have to be observed to fire the <see cref="OtpEffect.OtpReceived"/> event.
		/// </summary>
		void OnTextChanged(object sender, EventArgs e)
		{
			var field = (UITextField)sender;
			var initialContent = field.Text;
			if (initialContent == null)
				return;

			// On the initial input, start a delayed task where we check if more text was entered quicker then someone would type.
			var isInitialInput = initialContent.Length == 1;
			if (!isInitialInput)
				return;

			DispatchQueue.MainQueue.DispatchAfter(new DispatchTime(DispatchTime.Now, TimeSpan.FromMilliseconds(10)), () =>
			{
				var finalContent = field.Text;

				// Removing all text is really fast. Detect and skip in this case.
				if (finalContent == null || finalContent.Length == 0)
					return;

				// Check if the text changed in the short time span, indicating that it was pasted/autofilled.
				var didChangeInTime = initialContent != finalContent;
				if(didChangeInTime)
				{
					OnOtpReceived(finalContent);
				}
			});
		}

		void OnOtpReceived(string otpCode) => otpEffect?.RaiseOtpReceivedEvent(Element, new OtpReceivedEventArgs(otpCode));

		protected override void OnDetached()
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0)
				&& Control is UITextField textField)
			{
				textField.TextContentType = NSString.Empty;
				textField.EditingChanged -= OnTextChanged;
			}
		}
	}
}
