using Xamarin.Forms;
using static System.Math;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class AvatarView : TemplatedView
	{
		const string emptyText = "X";

		static readonly Color[] colors = {
			RGB(69, 43, 103),
			RGB(119, 78, 133),
			RGB(211, 153, 184),
			RGB(249, 218, 231),
			RGB(223, 196, 208),
			RGB(209, 158, 180),
			RGB(171, 116, 139),
			RGB(143, 52, 87)
		};

		static readonly Color[] textColors = {
			RGB(255, 255, 255),
			RGB(255, 255, 255),
			RGB(255, 255, 255),
			RGB(131, 81, 102),
			RGB(53, 21, 61),
			RGB(255, 255, 255),
			RGB(114, 50, 75),
			RGB(255, 255, 255)
		};

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

		AvatarFrame frame;

		public AvatarView()
		{
			IsClippedToBounds = true;
			ControlTemplate = new ControlTemplate(typeof(AvatarFrame));
		}

		AvatarFrame Frame
		{
			get => frame;
			set
			{
				frame = value;
				OnValuePropertyChanged();
			}
		}

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

		static Color RGB(int r, int g, int b)
			=> Color.FromRgb(r, g, b);

		void OnSizePropertyChanged()
		{
			if (Frame == null)
				return;

			BatchBegin();
			var size = Size;
			Frame.CornerRadius = CornerRadius < 0
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
			if (Frame == null)
				return;

			var image = Frame.Image;
			var label = Frame.Label;
			var layout = Frame.MainLayout;

			image.BatchBegin();
			var source = Source;
			image.IsVisible = source != null;
			image.Source = source;
			image.BatchCommit();

			label.BatchBegin();
			var text = Text?.Trim() ?? string.Empty;
			label.Text = string.IsNullOrWhiteSpace(text)
				? emptyText
				: text?.Trim();

			var textHash = Abs(text.GetHashCode());
			var textColor = TextColor;
			label.TextColor = textColor == Color.Default
				? textColors[textHash % textColors.Length]
				: textColor;
			var fontSize = FontSize;
			label.FontSize = fontSize < 0
				? CalculateFontSize()
				: fontSize;
			label.FontFamily = FontFamily;
			label.FontAttributes = FontAttributes;
			label.BatchCommit();

			var color = Color;
			layout.BackgroundColor = color == Color.Default
				? colors[textHash % colors.Length]
				: color;

			Frame.BorderColor = BorderColor;
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

		class AvatarFrame : Frame
		{
			internal Image Image { get; } = new Image
			{
				Aspect = Aspect.AspectFill,
				IsVisible = false
			};

			internal Label Label { get; } = new Label
			{
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				LineBreakMode = LineBreakMode.TailTruncation
			};

			internal AbsoluteLayout MainLayout { get; } = new AbsoluteLayout
			{
				IsClippedToBounds = true
			};

			public AvatarFrame()
			{
				IsClippedToBounds = true;
				HasShadow = false;
				Padding = 0;
				MainLayout.Children.Add(Label, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
				MainLayout.Children.Add(Image, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
				Content = MainLayout;
			}

			protected override void OnParentSet()
			{
				base.OnParentSet();
				((AvatarView)Parent).Frame = this;
			}
		}
	}
}