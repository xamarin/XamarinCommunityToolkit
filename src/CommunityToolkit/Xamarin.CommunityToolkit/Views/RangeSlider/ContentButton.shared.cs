using System;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.UI.Views.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class ContentButton : BaseTemplatedView<Frame>
	{
		readonly WeakEventManager clickedEventManager = new WeakEventManager();

		public event EventHandler Clicked
		{
			add => clickedEventManager.AddEventHandler(value);
			remove => clickedEventManager.RemoveEventHandler(value);
		}

		public static readonly BindableProperty ContentTemplateProperty
			= BindableProperty.Create(nameof(ContentTemplate), typeof(DataTemplate), typeof(ContentButton), propertyChanged: OnContentTemplatePropertyChanged);

		public static readonly BindableProperty ContentProperty
			= BindableProperty.Create(nameof(Content), typeof(View), typeof(ContentButton), propertyChanged: OnContentPropertyChanged);

		public static readonly BindableProperty CornerRadiusProperty 
			= BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(ContentButton), 0.0f, propertyChanged: OnCornerRadiusPropertyChanged);

		public static readonly BindableProperty BorderColorProperty 
			= BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(ContentButton), Color.Default, propertyChanged: OnBorderColorPropertyChanged);

		public static new readonly BindableProperty BackgroundColorProperty 
			= BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(ContentButton), Color.Default, propertyChanged: OnBackgroundColorPropertyChanged);

		public static readonly BindableProperty CommandParameterProperty
			= BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ContentButton));

		public static readonly BindableProperty CommandProperty
			= BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ContentButton));

		static void OnContentTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
			=> ((ContentButton)bindable).OnContentTemplatePropertyChanged();

		static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
			=> ((ContentButton)bindable).OnContentPropertyChanged();

		static void OnCornerRadiusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
			=> ((ContentButton)bindable).OnCornerRadiusPropertyChanged();

		static void OnBorderColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
			=> ((ContentButton)bindable).OnBorderColorPropertyChanged();

		static void OnBackgroundColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
			=> ((ContentButton)bindable).OnBackgroundColorPropertyChanged();

		void OnContentTemplatePropertyChanged()
			=> SetContent(true);

		void OnContentPropertyChanged()
			=> SetContent();

		void OnCornerRadiusPropertyChanged()
			=> Control.CornerRadius = CornerRadius;

		void OnBorderColorPropertyChanged()
			=> Control.BorderColor = BorderColor;

		void OnBackgroundColorPropertyChanged()
			=> Control.BackgroundColor = BackgroundColor;

		public DataTemplate ContentTemplate
		{
			get => (DataTemplate)GetValue(ContentTemplateProperty);
			set => SetValue(ContentTemplateProperty, value);
		}

		public View Content
		{
			get => (View)GetValue(ContentProperty);
			set => SetValue(ContentProperty, value);
		}

		public float CornerRadius
		{
			get => (float)GetValue(CornerRadiusProperty);
			set => SetValue(CornerRadiusProperty, value);
		}

		public Color BorderColor
		{
			get => (Color)GetValue(BorderColorProperty);
			set => SetValue(BorderColorProperty, value);
		}

		public new Color BackgroundColor
		{
			get => (Color)GetValue(BackgroundColorProperty);
			set => SetValue(BackgroundColorProperty, value);
		}

		public object CommandParameter
		{
			get => GetValue(CommandParameterProperty);
			set => SetValue(CommandParameterProperty, value);
		}

		public ICommand Command
		{
			get => (ICommand)GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}

		DataTemplate previousTemplate;

		readonly object contentSetLocker = new object();

		bool shouldIgnoreContentSetting;

		void SetContent(bool isForceUpdate)
		{
			if (isForceUpdate)
			{
				lock (contentSetLocker)
				{
					shouldIgnoreContentSetting = true;

					var contentFromTemplate = CreateContent();
					if (contentFromTemplate != null)
						Content = contentFromTemplate;

					shouldIgnoreContentSetting = false;
				}
			}
		}

		void SetContent()
		{
			if (Content != null)
				Control.Content = Content;
				
			if (!shouldIgnoreContentSetting)
				SetContent(true);
		}

		View CreateContent()
		{
			var template = ContentTemplate;
			while (template is DataTemplateSelector selector)
				template = selector.SelectTemplate(BindingContext, this);

			if (template == previousTemplate && Content != null)
				return null;

			previousTemplate = template;
			return (View)template?.CreateContent();
		}

		protected override void OnControlInitialized(Frame control)
		{
			control.Padding = 0;
			control.GestureRecognizers.Add(new TapGestureRecognizer
			{
				Command = new Command(async (parameter) =>
				{
					await Control.FadeTo(0.75, 100);
					await Control.FadeTo(1, 100);
					Command?.Execute(CommandParameter);
					OnTapped();
				})
			});
		}

		void OnTapped() => clickedEventManager.RaiseEvent(this, EventArgs.Empty, nameof(Clicked));
	}
}
