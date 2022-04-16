using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.UI.Views
{
	// TODO: Make it public once Android Renderer is implemeted
	class ImageSwitcher : Image, IViewSwitcher
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