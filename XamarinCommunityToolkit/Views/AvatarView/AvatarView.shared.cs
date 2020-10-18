using Xamarin.Forms;
using static System.Math;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class AvatarView : BaseTemplatedView<Frame>
	{
		const string emptyText = "X";

		public static readonly BindableProperty SizeProperty = BindableProperty.Create(nameof(Size), typeof(double), typeof(AvatarView), 40.0, propertyChanged: OnSizePropertyChanged);

		public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(AvatarView), -1.0, propertyChanged: OnSizePropertyChanged);

		public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(AvatarView), Color.Default, propertyChanged: OnValuePropertyChanged);

		public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(AvatarView), Color.Default, propertyChanged: OnValuePropertyChanged);

		public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(ImageSource), typeof(AvatarView), propertyChanged: OnValuePropertyChanged);

		public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(AvatarView), propertyChanged: OnValuePropertyChanged);

		public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(AvatarView), Color.Default, propertyChanged: OnValuePropertyChanged);

		public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(AvatarView), propertyChanged: OnValuePropertyChanged);

		public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(AvatarView), -1.0, propertyChanged: OnValuePropertyChanged);

		public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(AvatarView), FontAttributes.None, propertyChanged: OnValuePropertyChanged);

		public static readonly BindableProperty ColorThemeProperty = BindableProperty.Create(nameof(ColorTheme), typeof(IColorTheme), typeof(AvatarView), propertyChanged: OnValuePropertyChanged);

		public double Size
		{
			get => (double)GetValue(SizeProperty);
			set => SetValue(SizeProperty, value);
		}

		public double CornerRadius
		{
			get => (double)GetValue(CornerRadiusProperty);
			set => SetValue(CornerRadiusProperty, value);
		}

		public Color BorderColor
		{
			get => (Color)GetValue(BorderColorProperty);
			set => SetValue(BorderColorProperty, value);
		}

		public Color Color
		{
			get => (Color)GetValue(ColorProperty);
			set => SetValue(ColorProperty, value);
		}

		public ImageSource Source
		{
			get => (ImageSource)GetValue(SourceProperty);
			set => SetValue(SourceProperty, value);
		}

		public string Text
		{
			get => (string)GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}

		public Color TextColor
		{
			get => (Color)GetValue(TextColorProperty);
			set => SetValue(TextColorProperty, value);
		}

		public string FontFamily
		{
			get => (string)GetValue(FontFamilyProperty);
			set => SetValue(FontFamilyProperty, value);
		}

		public double FontSize
		{
			get => (double)GetValue(FontSizeProperty);
			set => SetValue(FontSizeProperty, value);
		}

		public FontAttributes FontAttributes
		{
			get => (FontAttributes)GetValue(FontAttributesProperty);
			set => SetValue(FontAttributesProperty, value);
		}

		public IColorTheme ColorTheme
		{
			get => (IColorTheme)GetValue(ColorThemeProperty);
			set => SetValue(ColorThemeProperty, value);
		}

		Image Image { get; } = new Image
		{
			Aspect = Aspect.AspectFill,
			IsVisible = false
		};

		Label Label { get; } = new Label
		{
			HorizontalTextAlignment = TextAlignment.Center,
			VerticalTextAlignment = TextAlignment.Center,
			LineBreakMode = LineBreakMode.TailTruncation
		};

		AbsoluteLayout MainLayout { get; } = new AbsoluteLayout
		{
			IsClippedToBounds = true
		};

		protected override void OnControlInitialized(Frame control)
		{
			IsClippedToBounds = true;
			MainLayout.Children.Add(Label, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
			MainLayout.Children.Add(Image, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
			control.IsClippedToBounds = true;
			control.HasShadow = false;
			control.Padding = 0;
			control.Content = MainLayout;
		}

		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);
			OnSizePropertyChanged();
		}

		static void OnSizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((AvatarView)bindable).OnSizePropertyChanged();
			OnValuePropertyChanged(bindable, oldValue, newValue);
		}

		static void OnValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
			=> ((AvatarView)bindable).OnValuePropertyChanged();

		void OnSizePropertyChanged()
		{
			if (Control == null)
				return;

			BatchBegin();
			var size = Size;
			Control.CornerRadius = CornerRadius < 0
				? (float)size / 2
				: (float)CornerRadius;

			if (Abs(Width - size) > double.Epsilon &&
				Abs(Height - size) > double.Epsilon)
			{
				HeightRequest = size;
				WidthRequest = size;
				Layout(new Rectangle(X, Y, size, size));
			}
			BatchCommit();
		}

		void OnValuePropertyChanged()
		{
			if (Control == null)
				return;

			Image.BatchBegin();
			var source = Source;
			Image.IsVisible = source != null;
			Image.Source = source;
			Image.BatchCommit();

			Label.BatchBegin();
			var text = Text?.Trim() ?? string.Empty;
			Label.Text = string.IsNullOrWhiteSpace(text)
				? emptyText
				: text?.Trim();

			var colorTheme = ColorTheme ?? Views.ColorTheme.Default;
			var textColor = TextColor;
			Label.TextColor = textColor == Color.Default
				? colorTheme.GetForegroundColor(text)
				: textColor;

			var fontSize = FontSize;
			Label.FontSize = fontSize < 0
				? CalculateFontSize()
				: fontSize;
			Label.FontFamily = FontFamily;
			Label.FontAttributes = FontAttributes;
			Label.BatchCommit();

			var color = Color;
			MainLayout.BackgroundColor = color == Color.Default
				? colorTheme.GetBackgroundColor(text)
				: color;

			Control.BorderColor = BorderColor;
		}

		double CalculateFontSize()
		{
			var size = Size;
			if (size <= 24)
				return size * .5;

			if (size <= 30)
				return 12;

			return size * .4;
		}
	}
}