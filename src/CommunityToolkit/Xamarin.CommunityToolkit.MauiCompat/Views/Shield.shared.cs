using System;using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views.Internals;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.UI.Views
{
	/// <summary>
	/// The <see cref="Shield" /> is a type of badge that has two colored sections that contain text
	/// </summary>
	public class Shield : BaseTemplatedView<Frame>
	{
		/// <summary>
		/// Backing BindableProperty for the <see cref="Subject"/> property.
		/// </summary>
		public static readonly BindableProperty SubjectProperty =
			  BindableProperty.Create(nameof(Subject), typeof(string), typeof(Shield), null,
				  propertyChanged: OnSubjectChanged);

		static void OnSubjectChanged(BindableObject bindable, object oldValue, object newValue) => ((Shield)bindable).UpdateSubject();

		/// <summary>
		/// Text that is shown on the left side of the <see cref="Shield" />. This is a bindable property.
		/// </summary>
		public string? Subject
		{
			get => (string?)GetValue(SubjectProperty);
			set => SetValue(SubjectProperty, value);
		}

		/// <summary>
		/// Backing BindableProperty for the <see cref="SubjectBackgroundColor"/> property.
		/// </summary>
		public static readonly BindableProperty SubjectBackgroundColorProperty =
			BindableProperty.Create(nameof(SubjectBackgroundColor), typeof(Color), typeof(Shield), new Microsoft.Maui.Graphics.Color(),
				propertyChanged: OnSubjectBackgroundColorChanged);

		static void OnSubjectBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue) => ((Shield)bindable).UpdateSubjectColor();

		/// <summary>
		/// Background <see cref="Forms.Color" /> of the left side of the <see cref="Shield" />. This is a bindable property.
		/// </summary>
		public Color SubjectBackgroundColor
		{
			get => (Color)GetValue(SubjectBackgroundColorProperty);
			set => SetValue(SubjectBackgroundColorProperty, value);
		}

		[Obsolete("TextColor is obsolete. Please use StatusTextColor instead")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static readonly BindableProperty TextColorProperty =
			BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(Shield), new Microsoft.Maui.Graphics.Color(),
				propertyChanged: OnTextColorChanged);

		[Obsolete("TextColor is obsolete. Please use StatusTextColor instead")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public Color TextColor
		{
			get => (Color)GetValue(TextColorProperty);
			set => SetValue(TextColorProperty, value);
		}

		static void OnTextColorChanged(BindableObject bindable, object oldValue, object newValue) => ((Shield)bindable).StatusTextColor = (Color)newValue;

		/// <summary>
		/// Backing BindableProperty for the <see cref="SubjectTextColor"/> property.
		/// </summary>
		public static readonly BindableProperty SubjectTextColorProperty =
			BindableProperty.Create(nameof(SubjectTextColor), typeof(Color), typeof(Shield), new Microsoft.Maui.Graphics.Color(),
				propertyChanged: OnSubjectTextColorChanged);

		static void OnSubjectTextColorChanged(BindableObject bindable, object oldValue, object newValue) => ((Shield)bindable).UpdateSubjectTextColor();

		/// <summary>
		/// Text <see cref="Forms.Color" /> of the text on the right side of the Shield
		/// </summary>
		public Color SubjectTextColor
		{
			get => (Color)GetValue(SubjectTextColorProperty);
			set => SetValue(SubjectTextColorProperty, value);
		}

		/// <summary>
		/// Backing BindableProperty for the <see cref="Status"/> property.
		/// </summary>
		public static readonly BindableProperty StatusProperty =
			BindableProperty.Create(nameof(Status), typeof(string), typeof(Shield), null,
				propertyChanged: OnStatusChanged);

		static void OnStatusChanged(BindableObject bindable, object oldValue, object newValue) => ((Shield)bindable).UpdateStatus();

		/// <summary>
		/// Text that is shown on the right side of the <see cref="Shield" />. This is a bindable property.
		/// </summary>
		public string? Status
		{
			get => (string?)GetValue(StatusProperty);
			set => SetValue(StatusProperty, value);
		}

		[Obsolete("Color is obsolete. Please use StatusBackgroundColor instead")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static readonly BindableProperty ColorProperty =
			BindableProperty.Create(nameof(Color), typeof(Color), typeof(Shield), new Microsoft.Maui.Graphics.Color(),
		propertyChanged: OnColorChanged);

		static void OnColorChanged(BindableObject bindable, object oldValue, object newValue) => ((Shield)bindable).UpdateColor();

		[Obsolete("Color is obsolete. Please use StatusBackgroundColor instead")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public Color Color
		{
			get => (Color)GetValue(ColorProperty);
			set => SetValue(ColorProperty, value);
		}

		/// <summary>
		/// Backing BindableProperty for the <see cref="StatusBackgroundColor"/> property.
		/// </summary>
		public static readonly BindableProperty StatusBackgroundColorProperty =
			BindableProperty.Create(nameof(StatusBackgroundColor), typeof(Color), typeof(Shield), new Microsoft.Maui.Graphics.Color(),
				propertyChanged: OnStatusBackgroundColorChanged);

		static void OnStatusBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue) => ((Shield)bindable).UpdateStatusBackgroundColor();

		/// <summary>
		/// Background <see cref="Forms.Color" /> of the right side of the <see cref="Shield" />. This is a bindable property.
		/// </summary>
		public Color StatusBackgroundColor
		{
			get => (Color)GetValue(StatusBackgroundColorProperty);
			set => SetValue(StatusBackgroundColorProperty, value);
		}

		/// <summary>
		/// Backing BindableProperty for the <see cref="StatusTextColor"/> property.
		/// </summary>
		public static readonly BindableProperty StatusTextColorProperty =
			BindableProperty.Create(nameof(StatusTextColor), typeof(Color), typeof(Shield), new Microsoft.Maui.Graphics.Color(),
				propertyChanged: OnStatusTextColorChanged);

		static void OnStatusTextColorChanged(BindableObject bindable, object oldValue, object newValue) => ((Shield)bindable).UpdateStatusTextColor();

		/// <summary>
		/// Text <see cref="Forms.Color" /> of the text on the right side of the Shield
		/// </summary>
		public Color StatusTextColor
		{
			get => (Color)GetValue(StatusTextColorProperty);
			set => SetValue(StatusTextColorProperty, value);
		}

		/// <summary>
		/// Backing BindableProperty for the <see cref="FontSize"/> property.
		/// </summary>
		public static BindableProperty FontSizeProperty =
			BindableProperty.Create(nameof(FontSize), typeof(double), typeof(Shield), Label.FontSizeProperty.DefaultValue,
				propertyChanged: OnFontChanged);

		static void OnFontChanged(BindableObject bindable, object oldValue, object newValue) => ((Shield)bindable).UpdateFont();

		/// <summary>
		/// Font size of all the text on the <see cref="Shield" />. <see cref="NamedSize" /> values can be used. This is a bindable preoprty.
		/// </summary>
		[System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
		public double FontSize
		{
			get => (double)GetValue(FontSizeProperty);
			set => SetValue(FontSizeProperty, value);
		}

		/// <summary>
		/// Backing BindableProperty for the <see cref="FontFamily"/> property.
		/// </summary>
		public static BindableProperty FontFamilyProperty =
			BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(Shield), Label.FontFamilyProperty.DefaultValue,
				propertyChanged: OnFontChanged);

		/// <summary>
		/// Font of all the text on the <see cref="Shield" />. This is a bindable property.
		/// </summary>
		public string? FontFamily
		{
			get => (string?)GetValue(FontFamilyProperty);
			set => SetValue(FontFamilyProperty, value);
		}

		/// <summary>
		/// Backing BindableProperty for the <see cref="FontAttributes"/> property.
		/// </summary>
		public static BindableProperty FontAttributesProperty =
			BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(Shield), Label.FontAttributesProperty.DefaultValue,
				propertyChanged: OnFontChanged);

		/// <summary>
		/// Font attributes of all the text on the <see cref="Shield" />. This is a bindable property.
		/// </summary>
		public FontAttributes FontAttributes
		{
			get => (FontAttributes)GetValue(FontAttributesProperty);
			set => SetValue(FontAttributesProperty, value);
		}

		/// <summary>
		/// Backing BindableProperty for the <see cref="Command"/> property.
		/// </summary>
		public static readonly BindableProperty CommandProperty =
			BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(Shield), null);

		/// <summary>
		/// Command that is triggered when the <see cref="Shield" /> is tapped. This is a bindable property.
		/// </summary>
		public ICommand? Command
		{
			get => (ICommand?)GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}

		/// <summary>
		/// Backing BindableProperty for the <see cref="CommandParameter"/> property.
		/// </summary>
		public static readonly BindableProperty CommandParameterProperty =
			BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(Shield), null);

		/// <summary>
		/// Parameter that is provided to the <see cref="Command"/> when the <see cref="Shield" /> is tapped. This is a bindable property.
		/// </summary>
		public object? CommandParameter
		{
			get => GetValue(CommandParameterProperty);
			set => SetValue(CommandParameterProperty, value);
		}

		/// <summary>
		/// Event that is triggered when the <see cref="Shield" /> is tapped. This is a bindable property.
		/// </summary>
		public event EventHandler? Tapped;

		Microsoft.Maui.Controls.Compatibility.Grid ShieldSubjectContainer { get; } = CreateSubjectContainerElement();

		Label ShieldSubject { get; } = CreateSubjectElement();

		Microsoft.Maui.Controls.Compatibility.Grid ShieldStatusContainer { get; } = CreateStatusContainerElement();

		Label ShieldStatus { get; } = CreateStatusElement();

		static Microsoft.Maui.Controls.Compatibility.Grid CreateSubjectContainerElement()
			=> new Microsoft.Maui.Controls.Compatibility.Grid()
			{
				BackgroundColor = Color.FromHex("#555555")
			};

		static Label CreateSubjectElement()
		  => new Label
		  {
			  TextColor = Colors.White,
			  VerticalOptions = LayoutOptions.Center,
			  Margin = new Thickness(4, 0)
		  };

		static Microsoft.Maui.Controls.Compatibility.Grid CreateStatusContainerElement()
		 => new Microsoft.Maui.Controls.Compatibility.Grid();

		static Label CreateStatusElement()
		   => new Label
		   {
			   VerticalOptions = LayoutOptions.Center,
			   Margin = new Thickness(4, 0)
		   };

		protected override void OnControlInitialized(Frame control)
		{
			control.CornerRadius = 4;
			control.HorizontalOptions = LayoutOptions.Center;
			control.VerticalOptions = LayoutOptions.Start;
			control.Padding = 0;
			control.HasShadow = false;
			control.IsClippedToBounds = true;

			var content = new Microsoft.Maui.Controls.Compatibility.Grid
			{
				ColumnSpacing = 0
			};

			content.ColumnDefinitions.Add(new ColumnDefinition { Width = Microsoft.Maui.GridLength.Auto });
			content.ColumnDefinitions.Add(new ColumnDefinition { Width = Microsoft.Maui.GridLength.Auto });

			control.Content = content;

			ShieldSubjectContainer.Children.Add(ShieldSubject);
			content.Children.Add(ShieldSubjectContainer);
			Microsoft.Maui.Controls.Compatibility.Grid.SetColumn(ShieldSubjectContainer, 0);

			ShieldStatusContainer.Children.Add(ShieldStatus);
			content.Children.Add(ShieldStatusContainer);
			Microsoft.Maui.Controls.Compatibility.Grid.SetColumn(ShieldStatusContainer, 1);

			UpdateIsEnabled();
		}

		protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			base.OnPropertyChanged(propertyName);

			if (propertyName == IsEnabledProperty.PropertyName)
				UpdateIsEnabled();
		}

		void UpdateSubject() => ShieldSubject.Text = Subject;

		void UpdateStatus() => ShieldStatus.Text = Status;

		void UpdateSubjectColor() => ShieldSubjectContainer.BackgroundColor = SubjectBackgroundColor;

#pragma warning disable CS0618 // Type or member is obsolete
		void UpdateColor() => ShieldStatusContainer.BackgroundColor = Color;
#pragma warning restore CS0618 // Type or member is obsolete

		void UpdateStatusBackgroundColor() => ShieldStatusContainer.BackgroundColor = StatusBackgroundColor;

		void UpdateSubjectTextColor() => ShieldSubject.TextColor = SubjectTextColor;

		void UpdateStatusTextColor() => ShieldStatus.TextColor = StatusTextColor;

		void UpdateFont()
		{
			ShieldSubject.FontSize = FontSize;
			ShieldSubject.FontFamily = FontFamily;
			ShieldSubject.FontAttributes = FontAttributes;

			ShieldStatus.FontSize = FontSize;
			ShieldStatus.FontFamily = FontFamily;
			ShieldStatus.FontAttributes = FontAttributes;
		}

		void UpdateIsEnabled()
		{
			if (IsEnabled)
			{
				var tapGestureRecognizer = new TapGestureRecognizer();
				tapGestureRecognizer.Tapped += OnCloseButtonTapped;
				GestureRecognizers.Add(tapGestureRecognizer);
			}
			else
			{
				GestureRecognizers.Clear();
			}

			void OnCloseButtonTapped(object? sender, EventArgs e)
			{
				Tapped?.Invoke(this, EventArgs.Empty);
				Command?.Execute(CommandParameter);
			}
		}
	}
}