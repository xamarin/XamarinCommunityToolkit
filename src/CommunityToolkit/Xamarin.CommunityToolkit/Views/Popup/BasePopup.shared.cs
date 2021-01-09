using System;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	/// <summary>
	/// The popup controls base implementation.
	/// </summary>
	[ContentProperty(nameof(Content))]
	public abstract class BasePopup : VisualElement
	{
		readonly WeakEventManager<PopupDismissedEventArgs> dismissWeakEventManager = new WeakEventManager<PopupDismissedEventArgs>();
		readonly WeakEventManager<PopupOpenedEventArgs> openedWeakEventManager = new WeakEventManager<PopupOpenedEventArgs>();

		/// <summary>
		/// Instantiates a new instance of <see cref="BasePopup"/>.
		/// </summary>
		public BasePopup()
		{
			Color = Color.White;
			VerticalOptions = LayoutOptions.CenterAndExpand;
			HorizontalOptions = LayoutOptions.CenterAndExpand;
			IsLightDismissEnabled = true;
		}

		public static BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(BasePopup));
		public static BindableProperty SizeProperty = BindableProperty.Create(nameof(Size), typeof(Size), typeof(BasePopup));

		public static BindableProperty VerticalOptionsProperty = BindableProperty.Create(nameof(VerticalOptions), typeof(LayoutOptions), typeof(BasePopup), LayoutOptions.CenterAndExpand);
		public static BindableProperty HorizontalOptionsProperty = BindableProperty.Create(nameof(HorizontalOptions), typeof(LayoutOptions), typeof(BasePopup), LayoutOptions.CenterAndExpand);

		/// <summary>
		/// Gets or sets the <see cref="View"/> content to render in the Popup.
		/// </summary>
		/// <remarks>
		/// The View can be or type: <see cref="View"/>, <see cref="ContentPage"/> or <see cref="NavigationPage"/>
		/// </remarks>
		public virtual View Content { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="Color"/> of the Popup.
		/// </summary>
		/// <remarks>
		/// This color sets the native background color of the <see cref="Popup"/>, which is
		/// independent of any background color configured in the actual View.
		/// </remarks>
		public Color Color
		{
			get => (Color)GetValue(ColorProperty);
			set => SetValue(ColorProperty, value);
		}

		/// <summary>
		/// Gets or sets the <see cref="LayoutOptions"/> for positioning the <see cref="Popup"/> vertically on the screen.
		/// </summary>
		public LayoutOptions VerticalOptions
		{
			get => (LayoutOptions)GetValue(VerticalOptionsProperty);
			set => SetValue(VerticalOptionsProperty, value);
		}

		/// <summary>
		/// Gets or sets the <see cref="LayoutOptions"/> for positioning the <see cref="Popup"/> horizontally on the screen.
		/// </summary>
		public LayoutOptions HorizontalOptions
		{
			get => (LayoutOptions)GetValue(HorizontalOptionsProperty);
			set => SetValue(HorizontalOptionsProperty, value);
		}

		/// <summary>
		/// Gets or sets the <see cref="View"/> anchor.
		/// </summary>
		/// <remarks>
		/// The Anchor is where the Popup will render closest to. When an Anchor is configured
		/// the popup will appear centered over that control or as close as possible.
		/// </remarks>
		public View Anchor { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="Size"/> of the Popup Display. 
		/// </summary>
		/// <remarks>
		/// The Popup will always try to constrain the actual size of the <see cref="Popup" />
		/// to the <see cref="Popup" /> of the View unless a <see cref="Size"/> is specified.
		/// If the <see cref="Popup" /> contiains <see cref="LayoutOptions"/> a <see cref="Size"/>
		/// will be required. This will allow the View to have a concept of <see cref="Size"/>
		/// that varies from the actual <see cref="Size"/> of the <see cref="Popup" />
		/// </remarks>
		public Size Size
		{
			get => (Size)GetValue(SizeProperty);
			set => SetValue(SizeProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether the popup can be light dismissed.
		/// </summary>
		/// <remarks>
		/// When true and the user taps outside of the popup it will dismiss.
		/// </remarks>
		public bool IsLightDismissEnabled { get; set; }

		/// <summary>
		/// Dismissed event is invoked when the popup is closed.
		/// </summary>
		public event EventHandler<PopupDismissedEventArgs> Dismissed
		{
			add => dismissWeakEventManager.AddEventHandler(value);
			remove => dismissWeakEventManager.RemoveEventHandler(value);
		}

		/// <summary>
		/// Opened event is invoked when the popup is opened.
		/// </summary>
		public event EventHandler<PopupOpenedEventArgs> Opened
		{
			add => openedWeakEventManager.AddEventHandler(value);
			remove => openedWeakEventManager.RemoveEventHandler(value);
		}

		/// <summary>
		/// Invokes the <see cref="Dismissed"/> event.
		/// </summary>
		/// <param name="result">
		/// The results to add to the <see cref="PopupDismissedEventArgs"/>.
		/// </param>
		protected void OnDismissed(object result) =>
			dismissWeakEventManager.RaiseEvent(this, new PopupDismissedEventArgs(result), nameof(Dismissed));

		/// <summary>
		/// Invokes the <see cref="Opened"/> event.
		/// </summary>
		internal virtual void OnOpened() =>
			openedWeakEventManager.RaiseEvent(this, new PopupOpenedEventArgs(), nameof(Opened));

		/// <summary>
		/// Invoked when the popup is light dismissed. In other words when the
		/// user taps outside of the popup and it closes.
		/// </summary>
		protected internal virtual void LightDismiss()
		{
			// TODO - AH 12/3/2020
			// This is not being tested correctly. It is not
			// implemented in iOS or UWP.

			// empty default implementation
		}

		/// <inheritdoc />
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			if (Content != null)
			{
				SetInheritedBindingContext(Content, BindingContext);
			}
		}
	}
}
