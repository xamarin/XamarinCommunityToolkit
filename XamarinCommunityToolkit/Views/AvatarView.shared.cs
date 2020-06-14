using System;
using System.ComponentModel;
using Xamarin.Forms;
using static System.Math;

namespace XamarinCommunityToolkit.Views
{
    public class AvatarView : Frame
    {
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
            RGB(53, 21, 61),
            RGB(255, 255, 255),
            RGB(131, 81, 102),
            RGB(114, 50, 75)
        };

        public static readonly BindableProperty SizeProperty = BindableProperty.Create(nameof(Size), typeof(double), typeof(AvatarView), 40.0, propertyChanged: OnSizePropertyChanged);

        public static new readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(AvatarView), -1.0, propertyChanged: OnSizePropertyChanged);

        public static new readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(AvatarView), Color.Default, propertyChanged: OnValuePropertyChanged);

        public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(ImageSource), typeof(AvatarView), propertyChanged: OnValuePropertyChanged);

        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(AvatarView), propertyChanged: OnValuePropertyChanged);

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(AvatarView), Color.Default, propertyChanged: OnValuePropertyChanged);

        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(AvatarView), propertyChanged: OnValuePropertyChanged);

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(AvatarView), -1.0, propertyChanged: OnValuePropertyChanged);

        public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(AvatarView), FontAttributes.None, propertyChanged: OnValuePropertyChanged);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is disabled.", true)]
        public static new readonly BindableProperty ContentProperty = ContentView.ContentProperty;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is disabled.", true)]
        public static new readonly BindableProperty HasShadowProperty = Frame.HasShadowProperty;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is disabled.", true)]
        public static new readonly BindableProperty PaddingProperty = Frame.HasShadowProperty;

        readonly Image image = new Image
        {
            Aspect = Aspect.AspectFill,
            IsVisible = false
        };

        readonly Label label = new Label
        {
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            LineBreakMode = LineBreakMode.TailTruncation
        };

        readonly AbsoluteLayout layout = new AbsoluteLayout
        {
            IsClippedToBounds = true
        };

        public AvatarView()
        {
            layout.Children.Add(label, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            layout.Children.Add(image, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);

            IsClippedToBounds = true;
            base.HasShadow = false;
            base.Padding = 0;
            base.Content = layout;
        }

        public double Size
        {
            get => (double)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public new double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
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

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is disabled.", true)]
        public new View Content
        {
            get => base.Content;
            set => base.Content = value;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is disabled.", true)]
        public new bool HasShadow
        {
            get => base.HasShadow;
            set => base.HasShadow = value;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is disabled.", true)]
        public new Thickness Padding
        {
            get => base.Padding;
            set => base.Padding = value;
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
            try
            {
                BatchBegin();
                var size = Size;
                base.CornerRadius = CornerRadius < 0
                    ? (float)size / 2
                    : (float)CornerRadius;

                if (Abs(Width - size) <= double.Epsilon &&
                    Abs(Height - size) <= double.Epsilon)
                    return;

                HeightRequest = size;
                WidthRequest = size;
                Layout(new Rectangle(X, Y, size, size));
            }
            finally
            {
                BatchCommit();
            }
        }

        void OnValuePropertyChanged()
        {
            image.BatchBegin();
            var source = Source;
            image.IsVisible = source != null;
            image.Source = source;
            image.BatchCommit();

            label.BatchBegin();
            var text = Text?.Trim() ?? string.Empty;
            label.Text = string.IsNullOrWhiteSpace(text)
                ? "X"
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

            var color = BackgroundColor;
            layout.BackgroundColor = color == Color.Default
                ? colors[textHash % colors.Length]
                : color;
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