using System;
using Xamarin.CommunityToolkit.UI.Views.Internals;
using Xamarin.Forms;
using static System.Math;

namespace Xamarin.CommunityToolkit.UI.Views
{
	/// <summary>
	/// The <see cref="AvatarView"/> control allows the user to display an avatar or the user's initials if no avatar is available. By binding the <see cref="Source"/> property the user can assign an image to the <see cref="AvatarView"/>. Simultaneously binding the <see cref="Text"/> property will allow the user to also set the initials to be shown if no valid image is provided.
	/// </summary>
	public class AvatarView : BaseTemplatedView<Frame>
	{
		const string emptyText = "X";
		static readonly IImageSourceValidator imageSourceValidator = new ImageSourceValidator();

		/// <summary>
		/// Backing BindableProperty for the <see cref="Aspect"/> property.
		/// </summary>
		public static readonly BindableProperty AspectProperty = BindableProperty.Create(nameof(Aspect), typeof(Aspect), typeof(AvatarView), Aspect.AspectFill, propertyChanged: OnValuePropertyChanged);

		/// <summary>
		/// Backing BindableProperty for the <see cref="Size"/> property.
		/// </summary>
		public static readonly BindableProperty SizeProperty = BindableProperty.Create(nameof(Size), typeof(double), typeof(AvatarView), 40.0, propertyChanged: OnSizePropertyChanged);

		/// <summary>
		/// Backing BindableProperty for the <see cref="CornerRadius"/> property.
		/// </summary>
		public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(AvatarView), -1.0, propertyChanged: OnSizePropertyChanged);

		/// <summary>
		/// Backing BindableProperty for the <see cref="BorderColor"/> property.
		/// </summary>
		public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(AvatarView), Color.Default, propertyChanged: OnValuePropertyChanged);

		/// <summary>
		/// Backing BindableProperty for the <see cref="Color"/> property.
		/// </summary>
		public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(AvatarView), Color.Default, propertyChanged: OnValuePropertyChanged);

		/// <summary>
		/// Backing BindableProperty for the <see cref="Source"/> property.
		/// </summary>
		public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(ImageSource), typeof(AvatarView), propertyChanged: OnSourcePropertyChanged);

		/// <summary>
		/// Backing BindableProperty for the <see cref="Text"/> property.
		/// </summary>
		public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(AvatarView), propertyChanged: OnValuePropertyChanged);

		/// <summary>
		/// Backing BindableProperty for the <see cref="TextColor"/> property.
		/// </summary>
		public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(AvatarView), Color.Default, propertyChanged: OnValuePropertyChanged);

		/// <summary>
		/// Backing BindableProperty for the <see cref="FontFamily"/> property.
		/// </summary>
		public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(AvatarView), propertyChanged: OnValuePropertyChanged);

		/// <summary>
		/// Backing BindableProperty for the <see cref="FontSize"/> property.
		/// </summary>
		public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(AvatarView), -1.0, propertyChanged: OnValuePropertyChanged);

		/// <summary>
		/// Backing BindableProperty for the <see cref="FontAttributes"/> property.
		/// </summary>
		public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(AvatarView), FontAttributes.None, propertyChanged: OnValuePropertyChanged);

		/// <summary>
		/// Backing BindableProperty for the <see cref="ColorTheme"/> property.
		/// </summary>
		public static readonly BindableProperty ColorThemeProperty = BindableProperty.Create(nameof(ColorTheme), typeof(IColorTheme), typeof(AvatarView), propertyChanged: OnValuePropertyChanged);

		/// <summary>
		/// Setting the <see cref="Aspect"/> property determines how the avatar image is shown. Depending on the <see cref="Forms.Aspect"/> value it might crop the assigned image. This only applies when <see cref="Source"/> is set and results in showing an image. This is a bindable property.
		/// </summary>
		public Aspect Aspect
		{
			get => (Aspect)GetValue(AspectProperty);
			set => SetValue(AspectProperty, value);
		}

		/// <summary>
		/// Gets or sets the size of the image for the <see cref="AvatarView"/>. This is a bindable property.
		/// </summary>
		public double Size
		{
			get => (double)GetValue(SizeProperty);
			set => SetValue(SizeProperty, value);
		}

		/// <summary>
		/// Gets or sets the corner radius of the image for the <see cref="AvatarView"/>. This is a bindable property.
		/// </summary>
		public double CornerRadius
		{
			get => (double)GetValue(CornerRadiusProperty);
			set => SetValue(CornerRadiusProperty, value);
		}

		/// <summary>
		/// Gets or sets the border <see cref="Forms.Color"/> of the image for the <see cref="AvatarView"/>. This is a bindable property.
		/// </summary>
		public Color BorderColor
		{
			get => (Color)GetValue(BorderColorProperty);
			set => SetValue(BorderColorProperty, value);
		}

		/// <summary>
		/// Gets or sets the background <see cref="Forms.Color"/> of the  for the <see cref="AvatarView"/>. This only applies when <see cref="Source"/> is not set or doesn't result in a showing image. This is a bindable property.
		/// </summary>
		public Color Color
		{
			get => (Color)GetValue(ColorProperty);
			set => SetValue(ColorProperty, value);
		}

		/// <summary>
		/// Gets or sets the <see cref="ImageSource"/> that is used to try and show an avatar image. If the image could not be loaded, the <see cref="Text"/> will be shown. This is a bindable property.
		/// </summary>
		public ImageSource Source
		{
			get => (ImageSource)GetValue(SourceProperty);
			set => SetValue(SourceProperty, value);
		}

		/// <summary>
		/// Gets or sets the text for the <see cref="AvatarView"/>. Which is shown instead of the image when the <see cref="Source"/> is either not set or doesn't result in showing an image. This is a bindable property.
		/// </summary>
		public string Text
		{
			get => (string)GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}

		/// <summary>
		/// Gets or sets the <see cref="Forms.Color"/> of the <see cref="Text"/> for the <see cref="AvatarView"/>. This is a bindable property.
		/// </summary>
		public Color TextColor
		{
			get => (Color)GetValue(TextColorProperty);
			set => SetValue(TextColorProperty, value);
		}

		/// <summary>
		/// Font of the <see cref="Text"/> on the <see cref="AvatarView" />. This is a bindable property.
		/// </summary>
		public string FontFamily
		{
			get => (string)GetValue(FontFamilyProperty);
			set => SetValue(FontFamilyProperty, value);
		}

		/// <summary>
		/// Font size of the text on the <see cref="AvatarView" />. <see cref="NamedSize" /> values can be used. This is a bindable property.
		/// </summary>
		[TypeConverter(typeof(FontSizeConverter))]
		public double FontSize
		{
			get => (double)GetValue(FontSizeProperty);
			set => SetValue(FontSizeProperty, value);
		}

		/// <summary>
		/// Font attributes of the text on the <see cref="AvatarView" />. This is a bindable property.
		/// </summary>
		public FontAttributes FontAttributes
		{
			get => (FontAttributes)GetValue(FontAttributesProperty);
			set => SetValue(FontAttributesProperty, value);
		}

		/// <summary>
		/// Gets or sets the <see cref="ColorTheme"/> to be used on the <see cref="AvatarView"/>. This only applies when the <see cref="Source"/> is not set or doesn't result in showing an image. This is a bindable property.
		/// </summary>
		public IColorTheme ColorTheme
		{
			get => (IColorTheme)GetValue(ColorThemeProperty);
			set => SetValue(ColorThemeProperty, value);
		}

		Image Image { get; } = new Image
		{
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
			MainLayout.Children.Add(Label, new Rectangle(0.5, 0.5, -1, -1), AbsoluteLayoutFlags.PositionProportional);
			MainLayout.Children.Add(Image, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
			control.IsClippedToBounds = true;
			control.HasShadow = false;
			control.Padding = 0;
			control.Content = MainLayout;

			Image.BindingContextChanged += (s, e) => OnValuePropertyChanged(true);
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
			=> ((AvatarView)bindable).OnValuePropertyChanged(false);

		static void OnSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
			=> ((AvatarView)bindable).OnValuePropertyChanged(true);

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

		async void OnValuePropertyChanged(bool shouldUpdateSource)
		{
			if (Control == null)
				return;

			Image.BatchBegin();
			if (shouldUpdateSource)
			{
				if (Image.Source == Source)
					Image.Source = null;

				try
				{
					Image.IsVisible = await imageSourceValidator.IsImageSourceValidAsync(Source);
				}
				catch (OperationCanceledException)
				{
					Forms.Internals.Log.Warning("CancellationException", "IsImageSourceValidAsync was cancelled.");
				}
				catch (Exception ex)
				{
					Forms.Internals.Log.Warning("Error", ex.Message);
					throw;
				}
				Image.Source = Source;
			}

			Image.Aspect = Aspect;
			if (Aspect == Aspect.AspectFit)
			{
				AbsoluteLayout.SetLayoutBounds(Image, new Rectangle(.5, .5, -1, -1));
				AbsoluteLayout.SetLayoutFlags(Image, AbsoluteLayoutFlags.PositionProportional);
			}
			else
			{
				AbsoluteLayout.SetLayoutBounds(Image, new Rectangle(0, 0, 1, 1));
				AbsoluteLayout.SetLayoutFlags(Image, AbsoluteLayoutFlags.All);
			}

			Image.BatchCommit();

			Label.BatchBegin();
			Label.IsVisible = !Image.IsVisible;
			var text = Text?.Trim() ?? string.Empty;
			Label.Text = string.IsNullOrEmpty(text)
				? emptyText
				: text;

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