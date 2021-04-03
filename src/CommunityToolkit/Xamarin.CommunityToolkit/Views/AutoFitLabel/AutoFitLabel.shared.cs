using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Views
{
	public class AutoFitLabel : Label
	{
		public static readonly BindableProperty AutoFitModeProperty = BindableProperty.Create(nameof(AutoFitMode), typeof(AutoFitTextMode), typeof(Label), AutoFitTextMode.None);

		public static readonly BindableProperty MinAutoFitFontSizeProperty = BindableProperty.Create(nameof(MinAutoFitFontSize), typeof(int), typeof(Label), 9);

		public static readonly BindableProperty MaxAutoFitFontSizeProperty = BindableProperty.Create(nameof(MaxAutoFitFontSize), typeof(int), typeof(Label), 100);

		public AutoFitTextMode AutoFitMode
		{
			get => (AutoFitTextMode)GetValue(AutoFitModeProperty);
			set => SetValue(AutoFitModeProperty, value);
		}

		public int MinAutoFitFontSize
		{
			get => (int)GetValue(MinAutoFitFontSizeProperty);
			set => SetValue(MinAutoFitFontSizeProperty, value);
		}

		public int MaxAutoFitFontSize
		{
			get => (int)GetValue(MaxAutoFitFontSizeProperty);
			set => SetValue(MaxAutoFitFontSizeProperty, value);
		}
	}
}
