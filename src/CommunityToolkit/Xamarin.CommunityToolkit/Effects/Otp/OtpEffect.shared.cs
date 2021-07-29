using System;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Effects
{
	public class OtpEffect : RoutingEffect
	{
		readonly WeakEventManager eventManager = new WeakEventManager();

		public event EventHandler<OtpReceivedEventArgs> OtpReceived
		{
			add => eventManager.AddEventHandler<OtpReceivedEventArgs>(value);
			remove => eventManager.RemoveEventHandler<OtpReceivedEventArgs>(value);
		}

		public OtpEffect()
			: base(EffectIds.Otp)
		{
#if __ANDROID__
			if (System.DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.Android.Effects.PlatformOtpEffect();
#elif __IOS__
			if (System.DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.iOS.Effects.PlatformOtpEffect();
#endif
		}

		internal void RaiseOtpReceivedEvent(Element element, OtpReceivedEventArgs eventArgs) => eventManager.RaiseEvent(element, eventArgs, nameof(OtpReceived));
	}

	public class OtpReceivedEventArgs : EventArgs
	{
		public string Code { get; }

		public OtpReceivedEventArgs(string code)
		{
			Code = code;
		}
	}
}
