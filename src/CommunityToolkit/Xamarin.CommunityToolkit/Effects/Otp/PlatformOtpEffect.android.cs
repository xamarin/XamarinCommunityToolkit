using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Gms.Auth.Api.Phone;
using Android.Gms.Tasks;
using Android.OS;
using Android.Views;
using Android.Widget;
using Xamarin.CommunityToolkit.Android.Effects;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.ExportEffect(typeof(PlatformOtpEffect), nameof(OtpEffect))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	public class PlatformOtpEffect : PlatformEffect
	{
		PlatformOtpReceiver? receiver;

		EditText? control;
		OtpEffect? otpEffect;

		protected override void OnAttached()
		{
			otpEffect = (OtpEffect)Element.Effects
				.FirstOrDefault(e => e is OtpEffect) ??
				throw new ArgumentNullException($"The effect {nameof(OtpEffect)} can't be null.");

			control = Control as EditText ??
				throw new Exception($"The effect {nameof(OtpEffect)} must be attached to an {nameof(Xamarin.Forms.Entry)}");

			if (otpEffect != null
				&& Build.VERSION.SdkInt >= BuildVersionCodes.O
				&& control != null
				&& control.Context != null)
			{
				SetupOtp(control.Context);
			}
		}

		protected override void OnDetached()
		{
			RemoveOtpListener();
		}

		void SetupOtp(Context context)
		{
			var listener = new OtpListener();
			receiver = new PlatformOtpReceiver(this);
			context.RegisterReceiver(receiver, new IntentFilter(SmsRetriever.SmsRetrievedAction));

			var client = SmsRetriever.GetClient(context);
			var task = client.StartSmsRetriever();
			task.AddOnFailureListener(listener);
		}

		void RemoveOtpListener()
		{
			if (receiver == null || control?.Context == null)
				return;

			control.Context.UnregisterReceiver(receiver);
			receiver = null;
		}

		internal void OnOtpReceived(string otpCode)
		{
			if (control == null)
				return;

			control.Text = otpCode;
			otpEffect?.RaiseOtpReceivedEvent(Element, new OtpReceivedEventArgs(otpCode));
		}

		class OtpListener : Java.Lang.Object, IOnFailureListener
		{
			public void OnFailure(Java.Lang.Exception e) => throw new Exception("Failed to set-up otp listener", e);
		}
	}
}
