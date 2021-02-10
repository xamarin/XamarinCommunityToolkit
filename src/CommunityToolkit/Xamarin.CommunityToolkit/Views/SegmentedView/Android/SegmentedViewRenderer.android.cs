using System.Collections.Specialized;
using System.ComponentModel;
using Android.Content;
using Xamarin.CommunityToolkit.Android.UI.Views;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using RadioButton = Android.Widget.RadioButton;
using System.Linq;
using System;
using Android.Util;
using System.Threading.Tasks;

[assembly: ExportRenderer(typeof(SegmentedView), typeof(SegmentedViewRenderer))]

namespace Xamarin.CommunityToolkit.Android.UI.Views
{
	public class SegmentedViewRenderer : ViewRenderer<SegmentedView, FormsSegments>
	{
		const string TAG = "SegmentedView";

		public SegmentedViewRenderer(Context context)
			: base(context)
		{
		}

		protected override async void OnElementChanged(ElementChangedEventArgs<SegmentedView> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
			{
				try
				{
					await InitializeControl();
				}
				catch (Exception ex)
				{
					Log.Error(TAG, ex.Message);
				}
			}

			if (e.OldElement != null && Control != null)
				InvalidateControl();
		}

		void InvalidateControl()
		{
			Control.SegmentSelected -= SegmentSelected;
			((INotifyCollectionChanged)Element.Items).CollectionChanged -= SegmentsCollectionChanged;
		}

		async Task InitializeControl()
		{
			((INotifyCollectionChanged)Element.Items).CollectionChanged += SegmentsCollectionChanged;

			SetNativeControl(new FormsSegments(Context));
			Control.DisplayMode = Element.DisplayMode;
			Control.CornerRadius = Element.CornerRadius;
			Control.SegmentSelected += SegmentSelected;

			PopulateSegments();

			Control.ShouldReactToCollectionChanges = true;
			await Control.Initialize();
		}

		void PopulateSegments()
		{
                        var items = Element.Items.Count();
			for (var i = 0; i < items; i++)
			{
				Control.Children.Add(Element.Items.ElementAt(i));
			}
		}

		void SegmentsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					for (var s = 0; s < e.NewItems.Count; s++)
					{
						Control.Children.Add(e.NewItems[s].ToString());
					}
					break;
				case NotifyCollectionChangedAction.Remove:
					for (var s = 0; s < e.OldItems.Count; s++)
					{
						Control.Children.RemoveAt(e.OldStartingIndex);
					}
					break;
				case NotifyCollectionChangedAction.Reset:
				default:
					Control.Children.Clear();
					break;
			}
		}

		void SegmentSelected(object sender, SelectedPositionChangedEventArgs e)
		{
			Element.SelectedIndex = (int)e.SelectedPosition;
		}

		/// <summary>
		/// When a property of an element changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Xamarin.Forms Elements</param>
		protected override async void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (Control == null || Element == null)
				return;

			try
			{
				if (e.PropertyName == SegmentedView.SelectedIndexProperty.PropertyName)
					Control.CurrentSegment = (RadioButton)Control.GetChildAt(Element.SelectedIndex);

				if (e.PropertyName == SegmentedView.BackgroundColorProperty.PropertyName)
				{
					if (Element.BackgroundColor == Color.Default)
						return;

					Control.BackgroundColor = Element.BackgroundColor.ToAndroid();
					await Control.Initialize();

					// Set parent view back to transparent so the colors don't bleed
					Element.BackgroundColor = Color.Default;
				}

				if (e.PropertyName == SegmentedView.ColorProperty.PropertyName)
				{
					Control.TintColor = Element.Color.ToAndroid();
					await Control.Initialize();
				}

				if (e.PropertyName == SegmentedView.NormalTextColorProperty.PropertyName)
				{
					Control.NormalTextColor = Element.NormalTextColor.ToAndroid();
					await Control.Initialize();
				}

				if (e.PropertyName == SegmentedView.SelectedTextColorProperty.PropertyName)
				{
					Control.SelectedTextColor = Element.SelectedTextColor.ToAndroid();
					await Control.Initialize();
				}

				if (e.PropertyName == SegmentedView.DisplayModeProperty.PropertyName)
				{
					Control.DisplayMode = Element.DisplayMode;
					await Control.Initialize();
				}

				if (e.PropertyName == SegmentedView.CornerRadiusProperty.PropertyName)
				{
					Control.CornerRadius = Element.CornerRadius;
					await Control.Initialize();
				}
			}
			catch (Exception ex)
			{
				Log.Error(TAG, ex.Message);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (!disposing)
				return;

			InvalidateControl();
		}
	}
}
