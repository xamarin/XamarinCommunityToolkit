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

		public IVisualElementRenderer? Control { get; private set; }

		public BasePopup? Element { get; private set; }

		VisualElement? IVisualElementRenderer.Element => Element;

		public UIView? NativeView => View;

		public UIViewController? ViewController { get; private set; }

		public event EventHandler<VisualElementChangedEventArgs>? ElementChanged;

		public event EventHandler<PropertyChangedEventArgs>? ElementPropertyChanged;

		[Preserve(Conditional = true)]
		public PopupRenderer()
		{
		}

		public void SetElementSize(Size size) =>
			Control?.SetElementSize(size);

		public override void ViewDidLayoutSubviews()
		{
			base.ViewDidLayoutSubviews();

			_ = View ?? throw new NullReferenceException();
			SetElementSize(new Size(View.Bounds.Width, View.Bounds.Height));
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			_ = Element ?? throw new NullReferenceException();
			ModalInPopover = !Element.IsLightDismissEnabled;
		}

		public SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint) =>
			NativeView.GetSizeRequest(widthConstraint, heightConstraint);

		void IVisualElementRenderer.SetElement(VisualElement element)
		{
			if (element == null)
				throw new ArgumentNullException(nameof(element));

			if (element is not BasePopup popup)
				throw new ArgumentNullException("Element is not of type " + typeof(BasePopup), nameof(element));

			var oldElement = Element;
			Element = popup;
			CreateControl();

			if (oldElement != null)
				oldElement.PropertyChanged -= OnElementPropertyChanged;

			element.PropertyChanged += OnElementPropertyChanged;

			OnElementChanged(new ElementChangedEventArgs<BasePopup?>(oldElement, Element));
		}

		protected virtual void OnElementChanged(ElementChangedEventArgs<BasePopup?> e)
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

		protected virtual void OnElementPropertyChanged(object? sender, PropertyChangedEventArgs args)
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
			_ = Element ?? throw new NullReferenceException();

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
			_ = Element ?? throw new NullReferenceException();
			Element.Dismissed += OnDismissed;
		}

		void SetSize()
		{
			_ = Element ?? throw new NullReferenceException();
			if (!Element.Size.IsZero)
			{
				PreferredContentSize = new CGSize(Element.Size.Width, Element.Size.Height);
			}
		}

		void SetLayout()
		{
			((UIPopoverPresentationController)PresentationController).SourceRect = new CGRect(0, 0, PreferredContentSize.Width, PreferredContentSize.Height);

			_ = Element ?? throw new NullReferenceException();
			if (Element.Anchor == null)
			{
				var originY = Element.VerticalOptions.Alignment switch
				{
					LayoutAlignment.End => UIScreen.MainScreen.Bounds.Height,
					LayoutAlignment.Center => UIScreen.MainScreen.Bounds.Height / 2,
					_ => 0f
				};

				var originX = Element.HorizontalOptions.Alignment switch
				{
					LayoutAlignment.End => UIScreen.MainScreen.Bounds.Width,
					LayoutAlignment.Center => UIScreen.MainScreen.Bounds.Width / 2,
					_ => 0f
				};

				PopoverPresentationController.SourceRect = new CGRect(originX, originY, 0, 0);
				PopoverPresentationController.PermittedArrowDirections = 0;
			}
			else
			{
				var view = Platform.GetRenderer(Element.Anchor).NativeView;
				PopoverPresentationController.SourceView = view;
				PopoverPresentationController.SourceRect = view.Bounds;
				var arrowDirection = Views.iOSSpecific.Popup.GetArrowDirection(Element);
				PopoverPresentationController.PermittedArrowDirections = arrowDirection switch
				{
					Views.iOSSpecific.PopoverArrowDirection.Up => UIPopoverArrowDirection.Up,
					Views.iOSSpecific.PopoverArrowDirection.Down => UIPopoverArrowDirection.Down,
					Views.iOSSpecific.PopoverArrowDirection.Left => UIPopoverArrowDirection.Left,
					Views.iOSSpecific.PopoverArrowDirection.Right => UIPopoverArrowDirection.Right,
					Views.iOSSpecific.PopoverArrowDirection.Any => UIPopoverArrowDirection.Any,
					Views.iOSSpecific.PopoverArrowDirection.Unknown => UIPopoverArrowDirection.Unknown,
					_ => 0
				};
			}
		}

		void SetBackgroundColor()
		{
			_ = Element ?? throw new NullReferenceException();
			_ = Control ?? throw new NullReferenceException();

			Control.NativeView.BackgroundColor = Element.Color.ToUIColor();
		}

		void SetView()
		{
			_ = View ?? throw new NullReferenceException();
			_ = Control ?? throw new NullReferenceException();

			View.AddSubview(Control.ViewController.View ?? throw new NullReferenceException());
			View.Bounds = new CGRect(0, 0, PreferredContentSize.Width, PreferredContentSize.Height);
			AddChildViewController(Control.ViewController);
		}

		void SetPresentationController()
		{
			var popOverDelegate = new PopoverDelegate();
			popOverDelegate.PopoverDismissed += HandlePopoverDelegateDismissed;

			((UIPopoverPresentationController)PresentationController).SourceView = ViewController?.View ?? throw new NullReferenceException();

			((UIPopoverPresentationController)PresentationController).Delegate = popOverDelegate;
		}

		void HandlePopoverDelegateDismissed(object? sender, UIPresentationController e)
		{
			_ = Element ?? throw new NullReferenceException();

			if (IsViewLoaded && Element.IsLightDismissEnabled)
				Element.LightDismiss();
		}

		void AddToCurrentPageViewController()
		{
			_ = ViewController ?? throw new NullReferenceException();
			_ = Element ?? throw new NullReferenceException();

			ViewController.PresentViewController(this, true, () => Element.OnOpened());
		}

		async void OnDismissed(object? sender, PopupDismissedEventArgs e)
		{
			if (ViewController != null)
				await ViewController.DismissViewControllerAsync(true);
		}

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
					Element = null;

					var presentationController = (UIPopoverPresentationController)PresentationController;
					if (presentationController != null)
						presentationController.Delegate = null;
				}
			}

			base.Dispose(disposing);
		}

		class PopoverDelegate : UIPopoverPresentationControllerDelegate
		{
			readonly WeakEventManager<UIPresentationController> popoverDismissedEventManager = new WeakEventManager<UIPresentationController>();

			public event EventHandler<UIPresentationController> PopoverDismissed
			{
				add => popoverDismissedEventManager.AddEventHandler(value);
				remove => popoverDismissedEventManager.RemoveEventHandler(value);
			}

			public override UIModalPresentationStyle GetAdaptivePresentationStyle(UIPresentationController forPresentationController) =>
				UIModalPresentationStyle.None;

			public override void DidDismiss(UIPresentationController presentationController) =>
				popoverDismissedEventManager.RaiseEvent(this, presentationController, nameof(PopoverDismissed));
		}
	}
}
