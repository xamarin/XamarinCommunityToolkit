using System;
using System.ComponentModel;
using CoreGraphics;
using UIKit;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BasePopup), typeof(PopupRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class PopupRenderer : UIViewController, IVisualElementRenderer
	{
		bool isDisposed;

		public IVisualElementRenderer Control { get; private set; }

		public BasePopup Element { get; private set; }

		VisualElement IVisualElementRenderer.Element => Element;

		public UIView NativeView => base.View;

		public UIViewController ViewController { get; private set; }

		public event EventHandler<VisualElementChangedEventArgs> ElementChanged;

		public event EventHandler<PropertyChangedEventArgs> ElementPropertyChanged;

		[Preserve(Conditional = true)]
		public PopupRenderer()
		{
		}

		public void SetElementSize(Size size)
		{
			Control?.SetElementSize(size);
		}

		public override void ViewDidLayoutSubviews()
		{
			base.ViewDidLayoutSubviews();
			SetElementSize(new Size(base.View.Bounds.Width, base.View.Bounds.Height));
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			ModalInPopover = !Element.IsLightDismissEnabled;
		}

		public SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			return NativeView.GetSizeRequest(widthConstraint, heightConstraint);
		}

		void IVisualElementRenderer.SetElement(VisualElement element)
		{
			if (element == null)
				throw new ArgumentNullException(nameof(element));

			if (!(element is BasePopup popup))
				throw new ArgumentNullException("Element is not of type " + typeof(BasePopup), nameof(element));

			var oldElement = Element;
			Element = popup;
			CreateControl();

			if (oldElement != null)
				oldElement.PropertyChanged -= OnElementPropertyChanged;

			element.PropertyChanged += OnElementPropertyChanged;

			OnElementChanged(new ElementChangedEventArgs<BasePopup>(oldElement, Element));
		}

		protected virtual void OnElementChanged(ElementChangedEventArgs<BasePopup> e)
		{
			if (e.NewElement != null && !isDisposed)
			{
				ModalInPopover = true;
				ModalPresentationStyle = UIModalPresentationStyle.Popover;

				SetViewController();
				SetPresentationController();
				SetEvents();
				SetSize();
				SetLayout();
				SetBackgroundColor();
				SetView();
				AddToCurrentPageViewController();
			}

			ElementChanged?.Invoke(this, new VisualElementChangedEventArgs(e.OldElement, e.NewElement));
		}

		protected virtual void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == BasePopup.VerticalOptionsProperty.PropertyName ||
				args.PropertyName == BasePopup.HorizontalOptionsProperty.PropertyName)
			{
				SetLayout();
			}
			else if (args.PropertyName == BasePopup.SizeProperty.PropertyName)
			{
				SetSize();
			}
			else if (args.PropertyName == BasePopup.ColorProperty.PropertyName)
			{
				SetBackgroundColor();
			}

			ElementPropertyChanged?.Invoke(this, args);
		}

		void CreateControl()
		{
			var view = Element.Content;
			var contentPage = new ContentPage { Content = view, Padding = new Thickness(25) };

			Control = Platform.CreateRenderer(contentPage);
			Platform.SetRenderer(contentPage, Control);
			contentPage.Parent = Application.Current.MainPage;
		}

		void SetViewController()
		{
			var currentPageRenderer = Platform.GetRenderer(Application.Current.MainPage);
			ViewController = currentPageRenderer.ViewController;
		}

		void SetEvents()
		{
			Element.Dismissed += OnDismissed;
		}

		void SetSize()
		{
			if (!Element.Size.IsZero)
			{
				PreferredContentSize = new CGSize(Element.Size.Width, Element.Size.Height);
			}
		}

		void SetLayout()
		{
			((UIPopoverPresentationController)PresentationController).SourceRect = new CGRect(0, 0, PreferredContentSize.Width, PreferredContentSize.Height);

			if (Element.Anchor == null)
			{
				nfloat originX = 0;
				nfloat originY = 0;

				switch (Element.VerticalOptions.Alignment)
				{
					case LayoutAlignment.End:
						originY = UIScreen.MainScreen.Bounds.Height - PreferredContentSize.Height;
						break;
					case LayoutAlignment.Center:
						originY = (UIScreen.MainScreen.Bounds.Height / 2) - (PreferredContentSize.Height / 2);
						break;
				}

				switch (Element.HorizontalOptions.Alignment)
				{
					case LayoutAlignment.End:
						originX = UIScreen.MainScreen.Bounds.Width;
						break;
					case LayoutAlignment.Center:
						originX = UIScreen.MainScreen.Bounds.Width / 2;
						break;
				}

				PopoverPresentationController.SourceRect = new CGRect(originX, originY, 0, 0);
			}
			else
			{
				var view = Platform.GetRenderer(Element.Anchor).NativeView;
				PopoverPresentationController.SourceView = view;
				PopoverPresentationController.SourceRect = view.Bounds;
			}
		}

		void SetBackgroundColor()
		{
			Control.NativeView.BackgroundColor = Element.Color.ToUIColor();
		}

		void SetView()
		{
			base.View.AddSubview(Control.ViewController.View);
			base.View.Bounds = new CGRect(0, 0, PreferredContentSize.Width, PreferredContentSize.Height);
			AddChildViewController(Control.ViewController);
		}

		void SetPresentationController()
		{
			((UIPopoverPresentationController)PresentationController).SourceView = ViewController.View;

			// Setting PermittedArrowDirector to 0 breaks the Popover layout. It would be nice if there is no anchor to remove the arrow.
			((UIPopoverPresentationController)PresentationController).PermittedArrowDirections = UIPopoverArrowDirection.Up;
			((UIPopoverPresentationController)PresentationController).Delegate = new PopoverDelegate(this);
		}

		void AddToCurrentPageViewController()
		{
			ViewController.PresentViewController(this, true, () => Element.OnOpened());
		}

		void OnDismissed(object sender, PopupDismissedEventArgs e) =>
			ViewController.DismissViewControllerAsync(true);

		protected override void Dispose(bool disposing)
		{
			if (isDisposed)
				return;

			isDisposed = true;
			if (disposing)
			{
				if (Element != null)
				{
					Element.PropertyChanged -= OnElementPropertyChanged;

					if (Platform.GetRenderer(Element) == this)
					{
						// NOTE - AH 9/12/2020
						// This used to use the internal property
						// 'Xamarin.Forms.Platform.Android.Platform.RendererProperty'
						// That property is marked internal so the closest thing we can do
						// is duplicate the implementation here.
						//
						// I don't think this is really needed for the control, but I am
						// leaving it in so it can be reviewed.
						var rendererProperty = BindableProperty.CreateAttached("Renderer", typeof(IVisualElementRenderer), typeof(Platform), default(IVisualElementRenderer), propertyChanged: (bindable, oldvalue, newvalue) =>
						{
							var view = bindable as VisualElement;
							if (view != null)
								view.IsPlatformEnabled = newvalue != null;
						});

						Element.ClearValue(rendererProperty);
					}

					Element = null;
				}
			}

			base.Dispose(disposing);
		}

		class PopoverDelegate : UIPopoverPresentationControllerDelegate
		{
			PopupRenderer popupRenderer;

			public PopoverDelegate(PopupRenderer renderer) =>
				popupRenderer = renderer;

			public override UIModalPresentationStyle GetAdaptivePresentationStyle(UIPresentationController forPresentationController)
			{
				return UIModalPresentationStyle.None;
			}

			public override void DidDismiss(UIPresentationController presentationController)
			{
				if (popupRenderer.IsViewLoaded && popupRenderer.Element.IsLightDismissEnabled)
					popupRenderer.Element.LightDismiss();
			}
		}
	}
}
