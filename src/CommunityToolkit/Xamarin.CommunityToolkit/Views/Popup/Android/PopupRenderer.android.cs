using System;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.App.ActionBar;
using AColorRes = Android.Resource.Color;
using AView = Android.Views.View;
using FormsPlatform = Xamarin.Forms.Platform.Android.Platform;
using GravityFlags = Android.Views.GravityFlags;

[assembly: ExportRenderer(typeof(BasePopup), typeof(PopupRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class PopupRenderer : Dialog, IVisualElementRenderer, IDialogInterfaceOnCancelListener
	{
		int? defaultLabelFor;
		VisualElementTracker? tracker;
		ContainerView? container;
		bool isDisposed;

		public BasePopup? Element { get; private set; }

		void IVisualElementRenderer.UpdateLayout() => tracker?.UpdateLayout();

		VisualElement? IVisualElementRenderer.Element => Element;

		AView? IVisualElementRenderer.View => container;

		ViewGroup? IVisualElementRenderer.ViewGroup => null;

		VisualElementTracker? IVisualElementRenderer.Tracker => tracker;

		public event EventHandler<VisualElementChangedEventArgs>? ElementChanged;

		public event EventHandler<PropertyChangedEventArgs>? ElementPropertyChanged;

		public PopupRenderer(Context context)
			: base(context)
		{
		}

		void IVisualElementRenderer.SetElement(VisualElement element)
		{
			if (element == null)
				throw new ArgumentNullException(nameof(element));

			if (element is not BasePopup popup)
				throw new ArgumentNullException("Element is not of type " + typeof(BasePopup), nameof(element));

			var oldElement = Element;
			Element = popup;
			CreateControl(Element);

			if (oldElement != null)
				oldElement.PropertyChanged -= OnElementPropertyChanged;

			element.PropertyChanged += OnElementPropertyChanged;

			if (tracker == null)
				tracker = new VisualElementTracker(this);

			OnElementChanged(new ElementChangedEventArgs<BasePopup?>(oldElement, Element));
		}

		protected virtual void OnElementChanged(ElementChangedEventArgs<BasePopup?> e)
		{
			if (e.NewElement != null && !isDisposed && Element is BasePopup basePopup)
			{
				SetEvents(basePopup);
				SetColor(basePopup);
				SetSize(basePopup);
				SetAnchor(basePopup);
				SetLightDismiss(basePopup);

				Show();
			}

			ElementChanged?.Invoke(this, new VisualElementChangedEventArgs(e.OldElement, e.NewElement));
		}

		public override void Show()
		{
			base.Show();
			Element?.OnOpened();
		}

		protected virtual void OnElementPropertyChanged(object? sender, PropertyChangedEventArgs args)
		{
			if (Element is BasePopup basePopup && !isDisposed)
			{
				if (args.PropertyName == BasePopup.VerticalOptionsProperty.PropertyName
					|| args.PropertyName == BasePopup.HorizontalOptionsProperty.PropertyName
					|| args.PropertyName == BasePopup.SizeProperty.PropertyName)
				{
					SetSize(basePopup);
					SetAnchor(basePopup);
				}
				else if (args.PropertyName == BasePopup.ColorProperty.PropertyName)
				{
					SetColor(basePopup);
				}
			}

			ElementPropertyChanged?.Invoke(this, args);
		}

		void CreateControl(in BasePopup basePopup)
		{
			if (container == null)
			{
				container = new ContainerView(Context, basePopup.Content);
				SetContentView(container);
			}
		}

		void SetEvents(in BasePopup basePopup)
		{
			SetOnCancelListener(this);
			basePopup.Dismissed += OnDismissed;
		}

		void SetColor(in BasePopup basePopup) => Window?.SetBackgroundDrawable(new ColorDrawable(basePopup.Color.ToAndroid(AColorRes.BackgroundLight, Context)));

		void SetSize(in BasePopup basePopup)
		{
			if (basePopup.Content != null && basePopup.Size != default)
			{
				var decorView = (ViewGroup)(Window?.DecorView ?? throw new NullReferenceException());
				var child = decorView?.GetChildAt(0) ?? throw new NullReferenceException();

				var realWidth = (int)Context.ToPixels(basePopup.Size.Width);
				var realHeight = (int)Context.ToPixels(basePopup.Size.Height);

				var realContentWidth = (int)Context.ToPixels(basePopup.Content.WidthRequest);
				var realContentHeight = (int)Context.ToPixels(basePopup.Content.HeightRequest);

				var childLayoutParams = (FrameLayout.LayoutParams)(child?.LayoutParameters ?? throw new NullReferenceException());
				childLayoutParams.Width = realWidth;
				childLayoutParams.Height = realHeight;
				child.LayoutParameters = childLayoutParams;

				var horizontalParams = -1;
				switch (basePopup.Content.HorizontalOptions.Alignment)
				{
					case LayoutAlignment.Center:
					case LayoutAlignment.End:
					case LayoutAlignment.Start:
						horizontalParams = LayoutParams.WrapContent;
						break;
					case LayoutAlignment.Fill:
						horizontalParams = LayoutParams.MatchParent;
						break;
				}

				var verticalParams = -1;
				switch (basePopup.Content.VerticalOptions.Alignment)
				{
					case LayoutAlignment.Center:
					case LayoutAlignment.End:
					case LayoutAlignment.Start:
						verticalParams = LayoutParams.WrapContent;
						break;
					case LayoutAlignment.Fill:
						verticalParams = LayoutParams.MatchParent;
						break;
				}

				_ = container ?? throw new NullReferenceException();
				if (realContentWidth > -1)
				{
					var inputMeasuredWidth = realContentWidth > realWidth ?
						realWidth : realContentWidth;
					container.Measure(inputMeasuredWidth, (int)MeasureSpecMode.Unspecified);
					horizontalParams = container.MeasuredWidth;
				}
				else
				{
					container.Measure(realWidth, (int)MeasureSpecMode.Unspecified);
					horizontalParams = container.MeasuredWidth > realWidth ?
						realWidth : container.MeasuredWidth;
				}

				if (realContentHeight > -1)
				{
					verticalParams = realContentHeight;
				}
				else
				{
					var inputMeasuredWidth = realContentWidth > -1 ? horizontalParams : realWidth;
					container.Measure(inputMeasuredWidth, (int)MeasureSpecMode.Unspecified);
					verticalParams = container.MeasuredHeight > realHeight ?
						realHeight : container.MeasuredHeight;
				}

				var containerLayoutParams = new FrameLayout.LayoutParams(horizontalParams, verticalParams);

				switch (basePopup.Content.VerticalOptions.Alignment)
				{
					case LayoutAlignment.Start:
						containerLayoutParams.Gravity = GravityFlags.Top;
						break;
					case LayoutAlignment.Center:
					case LayoutAlignment.Fill:
						containerLayoutParams.Gravity = GravityFlags.FillVertical;
						containerLayoutParams.Height = realHeight;
						container.MatchHeight = true;
						break;
					case LayoutAlignment.End:
						containerLayoutParams.Gravity = GravityFlags.Bottom;
						break;
				}

				switch (basePopup.Content.HorizontalOptions.Alignment)
				{
					case LayoutAlignment.Start:
						containerLayoutParams.Gravity |= GravityFlags.Left;
						break;
					case LayoutAlignment.Center:
					case LayoutAlignment.Fill:
						containerLayoutParams.Gravity |= GravityFlags.FillHorizontal;
						containerLayoutParams.Width = realWidth;
						container.MatchWidth = true;
						break;
					case LayoutAlignment.End:
						containerLayoutParams.Gravity |= GravityFlags.Right;
						break;
				}

				container.LayoutParameters = containerLayoutParams;
			}
		}

		void SetAnchor(in BasePopup basePopup)
		{
			if (basePopup.Anchor != null)
			{
				var anchorView = FormsPlatform.GetRenderer(Element?.Anchor).View;
				var locationOnScreen = new int[2];
				anchorView.GetLocationOnScreen(locationOnScreen);

				Window?.SetGravity(GravityFlags.Top | GravityFlags.Left);
				Window?.DecorView.Measure((int)MeasureSpecMode.Unspecified, (int)MeasureSpecMode.Unspecified);

				// This logic is tricky, please read these notes if you need to modify
				// Android window coordinate starts (0,0) at the top left and (max,max) at the bottom right. All of the positions
				// that are being handled in this operation assume the point is at the top left of the rectangle. This means the
				// calculation operates in this order:
				// 1. Calculate top-left position of Anchor
				// 2. Calculate the Actual Center of the Anchor by adding the width /2 and height / 2
				// 3. Calculate the top-left point of where the dialog should be positioned by subtracting the Width / 2 and height / 2
				//    of the dialog that is about to be drawn.
				_ = Window?.Attributes ?? throw new NullReferenceException();

				Window.Attributes.X = locationOnScreen[0] + (anchorView.Width / 2) - (Window.DecorView.MeasuredWidth / 2);
				Window.Attributes.Y = locationOnScreen[1] + (anchorView.Height / 2) - (Window.DecorView.MeasuredHeight / 2);
			}
			else
			{
				SetDialogPosition(basePopup);
			}
		}

		void SetLightDismiss(in BasePopup basePopup)
		{
			if (basePopup.IsLightDismissEnabled)
				return;

			SetCancelable(false);
			SetCanceledOnTouchOutside(false);
		}

		void SetDialogPosition(in BasePopup basePopup)
		{
			var gravityFlags = basePopup.VerticalOptions.Alignment switch
			{
				LayoutAlignment.Start => GravityFlags.Top,
				LayoutAlignment.End => GravityFlags.Bottom,
				_ => GravityFlags.CenterVertical,
			};
			gravityFlags |= basePopup.HorizontalOptions.Alignment switch
			{
				LayoutAlignment.Start => GravityFlags.Left,
				LayoutAlignment.End => GravityFlags.Right,
				_ => GravityFlags.CenterHorizontal,
			};
			Window?.SetGravity(gravityFlags);
		}

		void OnDismissed(object? sender, PopupDismissedEventArgs e)
		{
			if (IsShowing)
				Dismiss();
		}

		public void OnCancel(IDialogInterface? dialog)
		{
			if (Element?.IsLightDismissEnabled is true)
				Element.LightDismiss();
		}

		protected override void Dispose(bool disposing)
		{
			if (isDisposed)
				return;

			isDisposed = true;
			if (disposing)
			{
				tracker?.Dispose();
				tracker = null;

				if (Element != null)
				{
					Element.PropertyChanged -= OnElementPropertyChanged;
					Element = null;
				}
			}

			base.Dispose(disposing);
		}

		SizeRequest IVisualElementRenderer.GetDesiredSize(int widthConstraint, int heightConstraint)
		{
			if (isDisposed || container == null)
				return default(SizeRequest);

			container.Measure(widthConstraint, heightConstraint);
			return new SizeRequest(new Size(container.MeasuredWidth, container.MeasuredHeight), default(Size));
		}

		void IVisualElementRenderer.SetLabelFor(int? id)
		{
			_ = container ?? throw new NullReferenceException();

			if (defaultLabelFor == null)
				defaultLabelFor = container.LabelFor;

			container.LabelFor = (int)(id ?? defaultLabelFor);
		}
	}
}