using System;
using System.Text.RegularExpressions;
using Android.Content;
using Android.Gms.Auth.Api.Phone;
using Android.Gms.Common.Apis;
using Xamarin.CommunityToolkit.Android.Effects;
using Xamarin.CommunityToolkit.Effects;

[assembly: Xamarin.Forms.ExportEffect(typeof(PlatformOtpEffect), nameof(OtpEffect))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	public class PlatformOtpReceiver : BroadcastReceiver
	{
		readonly PlatformOtpEffect effect;

		public PlatformOtpReceiver(PlatformOtpEffect effect) => this.effect = effect;

		public override void OnReceive(Context? context, Intent? intent)
		{
			try
			{
				if (context == null || intent == null)
					return;
				if (intent.Action != SmsRetriever.SmsRetrievedAction)
					return;
				var bundle = intent.Extras;
				if (bundle == null)
					return;
				if (bundle.Get(SmsRetriever.ExtraStatus) is not Statuses status)
					return;
				switch (status.StatusCode)
				{
					case CommonStatusCodes.Success:
						if (bundle.Get(SmsRetriever.ExtraSmsMessage) is not Java.Lang.String message)
							return;
						var code = ExtractNumber(message.ToString());
						effect.OnOtpReceived(code);
						break;
					case CommonStatusCodes.Timeout:
						break;
				}
			}
			catch (Exception e)
			{
				throw new Exception("Receiving OTP failed", e);
			}
		}

		static string ExtractNumber(string text)
		{
			if (string.IsNullOrEmpty(text))
				return string.Empty;
			var number = Regex.Match(text, @"\d+").Value;
			return number;
		}
	}
}
