using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class Shield : BaseTemplatedView<Frame>
    {
        public static readonly BindableProperty SubjectProperty =
              BindableProperty.Create(nameof(Subject), typeof(string), typeof(Shield), string.Empty,
                  propertyChanged: OnSubjectChanged);

        static void OnSubjectChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as Shield)?.UpdateSubject();

        public string Subject
        {
            get => (string)GetValue(SubjectProperty);
            set => SetValue(SubjectProperty, value);
        }

        public static readonly BindableProperty StatusProperty =
            BindableProperty.Create(nameof(Status), typeof(string), typeof(Shield), string.Empty,
                propertyChanged: OnStatusChanged);

        static void OnStatusChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as Shield)?.UpdateStatus();

        public string Status
        {
            get => (string)GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }

        public static readonly BindableProperty ColorProperty =
            BindableProperty.Create(nameof(Color), typeof(Color), typeof(Shield), Color.Default,
                propertyChanged: OnColorChanged);

        static void OnColorChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as Shield)?.UpdateColor();

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(Shield), Color.Default,
                propertyChanged: OnTextColorChanged);

        static void OnTextColorChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as Shield)?.UpdateTextColor();

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(Shield), null);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(Shield), null);

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public event EventHandler Tapped;

        Grid ShieldSubjectContainer { get; } = CreateSubjectContainerElement();

        Label ShieldSubject { get; } = CreateSubjectElement();

        Grid ShieldStatusContainer { get; } = CreateStatusContainerElement();

        Label ShieldStatus { get; } = CreateStatusElement();

        protected override void OnControlInitialized(Frame control)
        {
            control.CornerRadius = 4;
            control.HorizontalOptions = LayoutOptions.Center;
            control.VerticalOptions = LayoutOptions.Start;
            control.Padding = 0;
            control.HeightRequest = 20;
            control.HasShadow = false;
            control.IsClippedToBounds = true;

            var content = new Grid
            {
                ColumnSpacing = 0
            };

            content.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            content.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            control.Content = content;

            ShieldSubjectContainer.Children.Add(ShieldSubject);
            content.Children.Add(ShieldSubjectContainer);
            Grid.SetColumn(ShieldSubjectContainer, 0);

            ShieldStatusContainer.Children.Add(ShieldStatus);
            content.Children.Add(ShieldStatusContainer);
            Grid.SetColumn(ShieldStatusContainer, 1);

            UpdateIsEnabled();
        }

        static Grid CreateSubjectContainerElement()
            => new Grid
            {
                BackgroundColor = Color.FromHex("#555555")
            };

        static Label CreateSubjectElement()
          => new Label
          {
              TextColor = Color.White,
              VerticalOptions = LayoutOptions.Center,
              Margin = new Thickness(4, 0)
          };

        static Grid CreateStatusContainerElement()
         => new Grid();

        static Label CreateStatusElement()
           => new Label
           {
               VerticalOptions = LayoutOptions.Center,
               Margin = new Thickness(4, 0)
           };

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsEnabledProperty.PropertyName)
                UpdateIsEnabled();
        }

        void UpdateSubject() => ShieldSubject.Text = Subject;

        void UpdateStatus() => ShieldStatus.Text = Status;

        void UpdateColor() => ShieldStatusContainer.BackgroundColor = Color;

        void UpdateTextColor() => ShieldStatus.TextColor = TextColor;

        void UpdateIsEnabled()
        {
            if (IsEnabled)
            {
                Opacity = 1.0d;

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += OnCloseButtonTapped;
                GestureRecognizers.Add(tapGestureRecognizer);
            }
            else
            {
                Opacity = 0.4d;

                GestureRecognizers.Clear();
            }

            void OnCloseButtonTapped(object sender, EventArgs e)
            {
                Tapped?.Invoke(this, EventArgs.Empty);
                Command?.Execute(CommandParameter);
            }
        }
	}
}