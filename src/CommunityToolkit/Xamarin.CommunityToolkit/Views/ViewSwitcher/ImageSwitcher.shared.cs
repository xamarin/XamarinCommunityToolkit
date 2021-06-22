using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class ImageSwitcher : Image
	{
		public static readonly BindableProperty TransitionDurationProperty
			= ViewSwitcher.TransitionDurationProperty;

		public static readonly BindableProperty TransitionTypeProperty
			= ViewSwitcher.TransitionTypeProperty;

		public uint TransitionDuration
		{
			get => (uint)GetValue(TransitionDurationProperty);
			set => SetValue(TransitionDurationProperty, value);
		}

		public TransitionType TransitionType
		{
			get => (TransitionType)GetValue(TransitionTypeProperty);
			set => SetValue(TransitionTypeProperty, value);
		}
	}
}