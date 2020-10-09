using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Xamarin.CommunityToolkit.iOS.UI.Views;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using UIKit;
using NativeImage = UIKit.UIImage;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SegmentedView), typeof(SegmentedViewRenderer))]

namespace Xamarin.CommunityToolkit.iOS.UI.Views
{
	[Preserve(AllMembers = true)]
	public class SegmentedViewRenderer : ViewRenderer<SegmentedView, UISegmentedControl>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<SegmentedView> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement == null)
				return;

			if (Control == null)
				SetNativeControl(new UISegmentedControl());

			if (e.OldElement != null && Control != null)
				InvalidateControl();

			if (e.NewElement != null)
				InitializeControl();
		}

		void InitializeControl()
		{
			PopulateSegments(Element.Items);
			Control.ClipsToBounds = true;
			Control.SelectedSegment = Element.SelectedIndex;
			Control.BackgroundColor = Element.BackgroundColor.ToUIColor();
			Control.Layer.MasksToBounds = true;
			UpdateSelectedSegment(Element.SelectedIndex);

			Control.ValueChanged += OnSelectedIndexChanged;
			((INotifyCollectionChanged)Element.Items).CollectionChanged += SegmentsCollectionChanged;

			if (Element.IsColorSet)
			{
				Control.SelectedSegmentTintColor = Element.Color.ToUIColor();
				Control.TintColor = Element.Color.ToUIColor();
			}
		}

		void PopulateSegments(IList<string> segments)
		{
			for (var i = 0; i < segments.Count; i++)
			{
				InsertSegment(segments.ElementAt(i), i);
			}
		}

		async void InsertSegment(string segment, int position)
		{
			switch (Element.DisplayMode)
			{
				case SegmentMode.Image:
					//var img = await ((ImageSource)segment).GetNativeImageAsync();
					//if (img != null)
					//	Control.InsertSegment(img, position, false);
					//else
					//	Console.WriteLine("ImageSource is null");
					//break;
				default:
				case SegmentMode.Text:
					Control.InsertSegment(segment, position, false);
					break;
			}
		}

		void InvalidateControl()
		{
			Control.ValueChanged -= OnSelectedIndexChanged;
			((INotifyCollectionChanged)Element.Items).CollectionChanged -= SegmentsCollectionChanged;
		}

		void SegmentsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					var startIndex = e.NewStartingIndex;
					foreach (var item in e.NewItems)
					{
						InsertSegment((string)item, startIndex++);
					}
					break;
				case NotifyCollectionChangedAction.Remove:
					for (var i = 0; i < e.OldItems.Count; i++)
					{
						Control.RemoveSegmentAtIndex(e.OldStartingIndex, false);
					}
					break;
				case NotifyCollectionChangedAction.Reset:
					Control.RemoveAllSegments();
					break;
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (Control == null || Element == null)
				return;

			if (e.PropertyName == SegmentedView.SelectedItemProperty.PropertyName || e.PropertyName == SegmentedView.SelectedIndexProperty.PropertyName)
				UpdateSelectedSegment(Element.SelectedIndex);

			if (e.PropertyName == SegmentedView.ColorProperty.PropertyName)
			{
				Control.SelectedSegmentTintColor = Element.Color.ToUIColor();
			}
		}

		void OnSelectedIndexChanged(object sender, EventArgs e)
		{
			Element.SelectedIndex = (int)Control.SelectedSegment;
		}

		void UpdateSelectedSegment(int index)
		{
			Control.SelectedSegment = index;
		}

		protected override void Dispose(bool disposing)
		{
			Control.ValueChanged -= OnSelectedIndexChanged;
			Control.Dispose();
			base.Dispose(disposing);
		}
	}
}
