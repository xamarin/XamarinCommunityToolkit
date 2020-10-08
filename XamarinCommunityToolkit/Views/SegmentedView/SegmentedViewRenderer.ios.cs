using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Xamarin.CommunityToolkit.iOS.UI.Views;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using UIKit;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.iOS;
using System.Threading.Tasks;
using System.Threading;

[assembly: ExportRenderer(typeof(SegmentedView), typeof(SegmentedViewRenderer))]

namespace Xamarin.CommunityToolkit.iOS.UI.Views
{
	[Preserve(AllMembers = true)]
	public class SegmentedViewRenderer : VisualElementRenderer<SegmentedView>
	{
		readonly UISegmentedControl control;

		public SegmentedViewRenderer()
		{
			InvalidateControl();
		}

		void InitializeControl()
		{
			PopulateSegments(Element.Items);
			control.ClipsToBounds = true;
			control.SelectedSegment = Element.SelectedIndex;
			control.BackgroundColor = Element.BackgroundColor.ToUIColor();
			control.Layer.MasksToBounds = true;
			UpdateSelectedSegment(Element.SelectedIndex);

			control.ValueChanged += OnSelectedIndexChanged;
			((INotifyCollectionChanged)Element.Items).CollectionChanged += SegmentsCollectionChanged;

			if (Element.IsColorSet)
			{
				control.SelectedSegmentTintColor = Element.Color.ToUIColor();
				control.TintColor = Element.Color.ToUIColor();
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
					//	control.InsertSegment(img, position, false);
					//else
					//	Console.WriteLine("ImageSource is null");
					//break;
				default:
				case SegmentMode.Text:
					control.InsertSegment(segment, position, false);
					break;
			}
		}

		void InvalidateControl()
		{
			control.ValueChanged -= OnSelectedIndexChanged;
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
						control.RemoveSegmentAtIndex(e.OldStartingIndex, false);
					}
					break;
				case NotifyCollectionChangedAction.Reset:
					control.RemoveAllSegments();
					break;
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (control == null || Element == null)
				return;

			if (e.PropertyName == SegmentedView.SelectedItemProperty.PropertyName || e.PropertyName == SegmentedView.SelectedIndexProperty.PropertyName)
				UpdateSelectedSegment(Element.SelectedIndex);

			if (e.PropertyName == SegmentedView.ColorProperty.PropertyName)
			{
				control.SelectedSegmentTintColor = Element.Color.ToUIColor();
			}
		}

		void OnSelectedIndexChanged(object sender, EventArgs e)
		{
			Element.SelectedIndex = (int)control.SelectedSegment;
		}

		void UpdateSelectedSegment(int index)
		{
			control.SelectedSegment = index;
		}

		protected override void Dispose(bool disposing)
		{
			control.ValueChanged -= OnSelectedIndexChanged;
			control.Dispose();
			base.Dispose(disposing);
		}
	}
}
