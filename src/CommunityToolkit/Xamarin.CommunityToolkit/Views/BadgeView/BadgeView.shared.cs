using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views.Internals;
using Xamarin.Forms;
using PropertyChangedEventArgs = System.ComponentModel.PropertyChangedEventArgs;

namespace Xamarin.CommunityToolkit.UI.Views
{
    [ContentProperty(nameof(Content))]
    public class BadgeView : BaseTemplatedView<Grid>
    {
        bool isVisible;
        bool placementDone;

        public static readonly BindableProperty ContentProperty =
            BindableProperty.Create(nameof(Content), typeof(View), typeof(BadgeView),
                propertyChanged: OnLayoutPropertyChanged);

        static void OnLayoutPropertyChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as BadgeView)?.UpdateLayout();

        public View Content
        {
            get => (View)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static readonly BindableProperty BadgePositionProperty =
            BindableProperty.Create(nameof(BadgePosition), typeof(BadgePosition), typeof(BadgeView), BadgePosition.TopRight,
                propertyChanged: OnBadgePositionChanged);

        public BadgePosition BadgePosition
        {
            get => (BadgePosition)GetValue(BadgePositionProperty);
            set => SetValue(BadgePositionProperty, value);
        }

        static void OnBadgePositionChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as BadgeView)?.UpdateBadgeViewPlacement(true);

        public static BindableProperty AutoHideProperty =
            BindableProperty.Create(nameof(AutoHide), typeof(bool), typeof(BadgeView), defaultValue: true,
                propertyChanged: OnAutoHideChanged);

        public bool AutoHide
        {
            get => (bool)GetValue(AutoHideProperty);
            set => SetValue(AutoHideProperty, value);
        }

        static async void OnAutoHideChanged(BindableObject bindable, object oldValue, object newValue) => await (bindable as BadgeView)?.UpdateVisibilityAsync();

        public static BindableProperty IsAnimatedProperty =
           BindableProperty.Create(nameof(IsAnimated), typeof(bool), typeof(BadgeView), defaultValue: true);

        public bool IsAnimated
        {
            get => (bool)GetValue(IsAnimatedProperty);
            set => SetValue(IsAnimatedProperty, value);
        }

        public static BindableProperty BadgeAnimationProperty =
            BindableProperty.Create(nameof(BadgeAnimation), typeof(IBadgeAnimation), typeof(BadgeView), new BadgeAnimation());

        public IBadgeAnimation BadgeAnimation
        {
            get => (IBadgeAnimation)GetValue(BadgeAnimationProperty);
            set => SetValue(BadgeAnimationProperty, value);
        }

        public static new BindableProperty BackgroundColorProperty =
            BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(BadgeView), defaultValue: Color.Default);

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(BadgeView), Color.Default,
                propertyChanged: OnLayoutPropertyChanged);

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty HasShadowProperty =
          BindableProperty.Create(nameof(HasShadow), typeof(bool), typeof(BadgeView), false,
              propertyChanged: OnLayoutPropertyChanged);

        public bool HasShadow
        {
            get => (bool)GetValue(HasShadowProperty);
            set => SetValue(HasShadowProperty, value);
        }

        public static BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(BadgeView), defaultValue: Color.Default,
                propertyChanged: OnLayoutPropertyChanged);

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public static BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(BadgeView), defaultValue: "0",
                propertyChanged: OnTextChanged);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        static async void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is BadgeView badgeView)
            {
                badgeView.UpdateLayout();
                await badgeView.UpdateVisibilityAsync();
            }
        }

        public static BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(double), typeof(BadgeView), 10.0d,
                propertyChanged: OnFontChanged);

        static void OnFontChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as BadgeView)?.UpdateFont();

        [TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static BindableProperty FontFamilyProperty =
            BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(BadgeView), string.Empty,
                propertyChanged: OnFontChanged);

        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        public static BindableProperty FontAttributesProperty =
            BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(BadgeView), FontAttributes.None,
                propertyChanged: OnFontChanged);

        public FontAttributes FontAttributes
        {
            get => (FontAttributes)GetValue(FontAttributesProperty);
            set => SetValue(FontAttributesProperty, value);
        }

        ContentPresenter BadgeContent { get; } = CreateContentElement();

        Grid BadgeIndicatorContainer { get; } = CreateIndicatorContainerElement();

        Frame BadgeIndicatorBackground { get; } = CreateIndicatorBackgroundElement();

        Label BadgeText { get; } = CreateTextElement();

        protected override void OnControlInitialized(Grid control)
        {
            BadgeIndicatorBackground.Content = BadgeText;

            BadgeIndicatorContainer.Children.Add(BadgeIndicatorBackground);
            BadgeIndicatorContainer.PropertyChanged += OnBadgeIndicatorContainerPropertyChanged;
            BadgeText.SizeChanged += OnBadgeTextSizeChanged;

            control.Children.Add(BadgeContent);
            control.Children.Add(BadgeIndicatorContainer);
        }

        static ContentPresenter CreateContentElement()
            => new ContentPresenter
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start
            };

        static Grid CreateIndicatorContainerElement()
           => new Grid
           {
               HorizontalOptions = LayoutOptions.Start,
               VerticalOptions = LayoutOptions.Start,
               IsVisible = false
           };

        static Frame CreateIndicatorBackgroundElement()
           => new Frame
           {
               CornerRadius = Device.RuntimePlatform == Device.Android ? 12 : 8,
               Padding = 2
           };

        static Label CreateTextElement()
           => new Label
           {
               HorizontalOptions = LayoutOptions.Center,
               VerticalOptions = LayoutOptions.Center,
               Margin = new Thickness(4, 0)
           };

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            SetInheritedBindingContext(Content, BindingContext);
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            UpdateBadgeViewPlacement();
        }

        void UpdateLayout()
        {
            BatchBegin();
            BadgeContent.BatchBegin();
            BadgeIndicatorContainer.BatchBegin();
            BadgeIndicatorBackground.BatchBegin();
            BadgeText.BatchBegin();

            BadgeContent.Content = Content;

            BadgeIndicatorBackground.BackgroundColor = BackgroundColor;
            BadgeIndicatorBackground.BorderColor = BorderColor;
            BadgeIndicatorBackground.HasShadow = HasShadow;

            BadgeText.Text = Text;
            BadgeText.TextColor = TextColor;

            BadgeContent.BatchCommit();
            BadgeIndicatorContainer.BatchCommit();
            BadgeIndicatorBackground.BatchCommit();
            BadgeText.BatchCommit();
            BatchCommit();
        }

        void UpdateFont()
        {
            if (BadgeText == null)
                return;

            BadgeText.FontSize = FontSize;
            BadgeText.FontFamily = FontFamily;
            BadgeText.FontAttributes = FontAttributes;
        }

        void UpdateBadgeViewPlacement(bool force = false)
        {
            if (BadgeContent.Height <= 0 && BadgeContent.Width <= 0)
                return;

            if (force)
                placementDone = false;

            if (placementDone)
                return;

            var containerMargin = new Thickness(0);
            var contentMargin = new Thickness(0);

            if (BadgeIndicatorContainer.IsVisible)
            {
                const double Padding = 6;
                var size = Math.Max(BadgeText.Height, BadgeText.Width) + Padding;
                BadgeIndicatorBackground.HeightRequest = size;
                var margins = GetMargins(size);
                containerMargin = margins.Item1;
                contentMargin = margins.Item2;
            }

            BadgeIndicatorContainer.Margin = containerMargin;
            BadgeContent.Margin = contentMargin;
            placementDone = true;
        }

        Tuple<Thickness, Thickness> GetMargins(double size)
        {
            double verticalMargin;
            double horizontalMargin;
            var containerMargin = new Thickness(0);
            var contentMargin = new Thickness(0);
            switch (BadgePosition)
            {
                case BadgePosition.TopRight:
                    verticalMargin = size / 2;
                    horizontalMargin = BadgeContent.Width - verticalMargin;
                    containerMargin = new Thickness(horizontalMargin, 0, 0, 0);
                    contentMargin = new Thickness(0, verticalMargin, verticalMargin, 0);
                    break;
                case BadgePosition.TopLeft:
                    verticalMargin = size / 2;
                    containerMargin = new Thickness(0, 0, 0, 0);
                    contentMargin = new Thickness(verticalMargin, verticalMargin, 0, 0);
                    break;
                case BadgePosition.BottomLeft:
                    verticalMargin = size / 2;
                    var bottomLeftverticalMargin = BadgeContent.Height - verticalMargin;
                    containerMargin = new Thickness(0, bottomLeftverticalMargin, 0, 0);
                    contentMargin = new Thickness(verticalMargin, 0, 0, 0);
                    break;
                case BadgePosition.BottomRight:
                    verticalMargin = size / 2;
                    var bottomRightverticalMargin = BadgeContent.Height - verticalMargin;
                    horizontalMargin = BadgeContent.Width - verticalMargin;
                    containerMargin = new Thickness(horizontalMargin, bottomRightverticalMargin, 0, 0);
                    contentMargin = new Thickness(0, 0, verticalMargin, 0);
                    break;
            }
            return new Tuple<Thickness, Thickness>(containerMargin, contentMargin);
        }

        async Task UpdateVisibilityAsync()
        {
            if (BadgeIndicatorBackground == null)
                return;

            var badgeText = BadgeText.Text;

            if (string.IsNullOrEmpty(badgeText))
            {
                BadgeIndicatorBackground.IsVisible = false;
                return;
            }

            var badgeIsVisible = !AutoHide || !badgeText.Trim().Equals("0");
            BadgeIndicatorBackground.IsVisible = badgeIsVisible;

            if (IsAnimated)
            {
                if (badgeIsVisible == isVisible)
                    return;

                if (badgeIsVisible)
                {
                    BadgeIndicatorContainer.IsVisible = true;
                    await BadgeAnimation.OnAppearing(BadgeIndicatorContainer);
                }
                else
                {
                    await BadgeAnimation.OnDisappering(BadgeIndicatorContainer);
                    BadgeIndicatorContainer.IsVisible = false;
                }

                isVisible = badgeIsVisible;
            }
            else
                BadgeIndicatorContainer.IsVisible = badgeIsVisible;
        }

        void OnBadgeTextSizeChanged(object sender, EventArgs e)
            => UpdateBadgeViewPlacement(true);

        void OnBadgeIndicatorContainerPropertyChanged(object sender, PropertyChangedEventArgs e)
            => UpdateBadgeViewPlacement(true);
    }
}