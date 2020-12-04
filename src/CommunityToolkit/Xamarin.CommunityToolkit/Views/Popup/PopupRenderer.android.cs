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
using AView = Android.Views.View;
using FormsPlatform = Xamarin.Forms.Platform.Android.Platform;
using GravityFlags = Android.Views.GravityFlags;

[assembly: ExportRenderer(typeof(BasePopup), typeof(PopupRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class PopupRenderer : Dialog, IVisualElementRenderer, IDialogInterfaceOnCancelListener
	{
		int? defaultLabelFor;
		VisualElementTracker tracker;
		ContainerView container;
		bool isDisposed = false;

		public BasePopup Element { get; private set; }

		void IVisualElementRenderer.UpdateLayout() => tracker?.UpdateLayout();

		VisualElement IVisualElementRenderer.Element => Element;

		AView IVisualElementRenderer.View => container;

		ViewGroup IVisualElementRenderer.ViewGroup => null;

		VisualElementTracker IVisualElementRenderer.Tracker => tracker;

		public event EventHandler<VisualElementChangedEventArgs> ElementChanged;

		public event EventHandler<PropertyChangedEventArgs> ElementPropertyChanged;

		public PopupRenderer(Context context)
			: base(context)
		{
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

			if (tracker == null)
				tracker = new VisualElementTracker(this);

			OnElementChanged(new ElementChangedEventArgs<BasePopup>(oldElement, Element));
		}

		protected virtual void OnElementChanged(ElementChangedEventArgs<BasePopup> e)
		{
			if (e.NewElement != null && !isDisposed)
			{
				SetEvents();
				SetColor();
				SetSize();
				SetAnchor();
				SetLightDismiss();

				Show();
			}

			ElementChanged?.Invoke(this, new VisualElementChangedEventArgs(e.OldElement, e.NewElement));
		}

		public override void Show()
		{
			base.Show();
			Element?.OnOpened();
		}

		protected virtual void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == BasePopup.VerticalOptionsProperty.PropertyName ||
				args.PropertyName == BasePopup.HorizontalOptionsProperty.PropertyName ||
				args.PropertyName == BasePopup.SizeProperty.PropertyName)
			{
				SetSize();
				SetAnchor();
			}
			else if (args.PropertyName == BasePopup.ColorProperty.PropertyName)
			{
				SetColor();
			}

			ElementPropertyChanged?.Invoke(this, args);
		}

		void CreateControl()
		{
			if (container == null)
			{
				container = new ContainerView(Context, Element.View);
				SetContentView(container);
			}
		}

		void SetEvents()
		{
			SetOnCancelListener(this);
			Element.Dismissed += OnDismissed;
		}

		void SetColor()
		{
			Window.SetBackgroundDrawable(new ColorDrawable(Element.Color.ToAndroid()));
		}

		void SetSize()
		{
			if (Element.Size != default)
			{
				var decorView = (ViewGroup)Window.DecorView;
				var child = (FrameLayout)decorView.GetChildAt(0);

				var childLayoutParams = (FrameLayout.LayoutParams)child.LayoutParameters;
				childLayoutParams.Width = (int)Element.Size.Width;
				childLayoutParams.Height = (int)Element.Size.Height;
				child.LayoutParameters = childLayoutParams;

				var horizontalParams = -1;
				switch (Element.View.HorizontalOptions.Alignment)
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
				switch (Element.View.VerticalOptions.Alignment)
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

				if (Element.View.WidthRequest > -1)
				{
					var inputMeasuredWidth = Element.View.WidthRequest > Element.Size.Width ?
						(int)Element.Size.Width : (int)Element.View.WidthRequest;
					container.Measure(inputMeasuredWidth, (int)MeasureSpecMode.Unspecified);
					horizontalParams = container.MeasuredWidth;
				}
				else
				{
					container.Measure((int)Element.Size.Width, (int)MeasureSpecMode.Unspecified);
					horizontalParams = container.MeasuredWidth > Element.Size.Width ?
						(int)Element.Size.Width : container.MeasuredWidth;
				}

				if (Element.View.HeightRequest > -1)
				{
					verticalParams = (int)Element.View.HeightRequest;
				}
				else
				{
					var inputMeasuredWidth = Element.View.WidthRequest > -1 ? horizontalParams : (int)Element.Size.Width;
					container.Measure(inputMeasuredWidth, (int)MeasureSpecMode.Unspecified);
					verticalParams = container.MeasuredHeight > Element.Size.Height ?
						(int)Element.Size.Height : container.MeasuredHeight;
				}

				var containerLayoutParams = new FrameLayout.LayoutParams(horizontalParams, verticalParams);

				switch (Element.View.VerticalOptions.Alignment)
				{
					case LayoutAlignment.Start:
						containerLayoutParams.Gravity = GravityFlags.Top;
						break;
					case LayoutAlignment.Center:
					case LayoutAlignment.Fill:
						containerLayoutParams.Gravity = GravityFlags.FillVertical;
						containerLayoutParams.Height = (int)Element.Size.Height;
						container.MatchHeight = true;
						break;
					case LayoutAlignment.End:
						containerLayoutParams.Gravity = GravityFlags.Bottom;
						break;
				}

				switch (Element.View.HorizontalOptions.Alignment)
				{
					case LayoutAlignment.Start:
						containerLayoutParams.Gravity |= GravityFlags.Left;
						break;
					case LayoutAlignment.Center:
					case LayoutAlignment.Fill:
						containerLayoutParams.Gravity |= GravityFlags.FillHorizontal;
						containerLayoutParams.Width = (int)Element.Size.Width;
						container.MatchWidth = true;
						break;
					case LayoutAlignment.End:
						containerLayoutParams.Gravity |= GravityFlags.Right;
						break;
				}

				container.LayoutParameters = containerLayoutParams;
			}
		}

		void SetAnchor()
		{
			if (Element.Anchor != null)
			{
				var anchorView = FormsPlatform.GetRenderer(Element.Anchor).View;
				var locationOnScreen = new int[2];
				anchorView.GetLocationOnScreen(locationOnScreen);

				Window.SetGravity(GravityFlags.Top | GravityFlags.Left);
				Window.DecorView.Measure((int)MeasureSpecMode.Unspecified, (int)MeasureSpecMode.Unspecified);

				// This logic is tricky, please read these notes if you need to modify
				// Android window coordinate starts (0,0) at the top left and (max,max) at the bottom right. All of the positions
				// that are being handled in this operation assume the point is at the top left of the rectangle. This means the
				// calculation operates in this order:
				// 1. Calculate top-left position of Anchor
				// 2. Calculate the Actual Center of the Anchor by adding the width /2 and height / 2
				// 3. Calculate the top-left point of where the dialog should be positioned by subtracting the Width / 2 and height / 2
				//    of the dialog that is about to be drawn.
				Window.Attributes.X = locationOnScreen[0] + (anchorView.Width / 2) - (Window.DecorView.MeasuredWidth / 2);
				Window.Attributes.Y = locationOnScreen[1] + (anchorView.Height / 2) - (Window.DecorView.MeasuredHeight / 2);
			}
			else
			{
				SetDialogPosition();
			}
		}

		void SetLightDismiss()
		{
			if (Element.IsLightDismissEnabled)
				return;

			SetCancelable(false);
			SetCanceledOnTouchOutside(false);
		}

		void SetDialogPosition()
		{
			var gravityFlags = GravityFlags.Center;
			switch (Element.VerticalOptions.Alignment)
			{
				case LayoutAlignment.Start:
					gravityFlags = GravityFlags.Top;
					break;
				case LayoutAlignment.End:
					gravityFlags = GravityFlags.Bottom;
					break;
				default:
					gravityFlags = GravityFlags.CenterVertical;
					break;
			}

			switch (Element.HorizontalOptions.Alignment)
			{
				case LayoutAlignment.Start:
					gravityFlags |= GravityFlags.Left;
					break;
				case LayoutAlignment.End:
					gravityFlags |= GravityFlags.Right;
					break;
				default:
					gravityFlags |= GravityFlags.CenterHorizontal;
					break;
			}

			Window.SetGravity(gravityFlags);
		}

		void OnDismissed(object sender, PopupDismissedEventArgs e)
		{
			Dismiss();
		}

		public void OnCancel(IDialogInterface dialog)
		{
			if (IsShowing && Element.IsLightDismissEnabled)
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
			if (defaultLabelFor == null)
				defaultLabelFor = container.LabelFor;

			container.LabelFor = (int)(id ?? defaultLabelFor);
		}
	}
}
