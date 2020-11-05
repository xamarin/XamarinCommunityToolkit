using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Android.Content;
using Android.Widget;
using Xamarin.CommunityToolkit.Android.UI.Views;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SegmentedView), typeof(SegmentedViewRenderer))]

namespace Xamarin.CommunityToolkit.Android.UI.Views
{
	public class SegmentedViewRenderer : ViewRenderer<SegmentedView, FormsSegments>
	{
		readonly Context context;
		FormsSegments control;

		public SegmentedViewRenderer(Context context)
			: base(context)
		{
			this.context = context;
		}

		protected override void OnElementChanged(ElementChangedEventArgs<SegmentedView> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
			{
				InitializeControl(Element.Items);
			}

			if (e.OldElement != null && Control != null)
				InvalidateControl();
		}

		void InvalidateControl()
		{
			Control.SegmentSelected -= SegmentSelected;
			((INotifyCollectionChanged)Element.Items).CollectionChanged -= SegmentsCollectionChanged;
		}

		void InitializeControl(IList<string> segments)
		{
			control = new FormsSegments(context);

			((INotifyCollectionChanged)Element.Items).CollectionChanged += SegmentsCollectionChanged;

			SetNativeControl(control);
			PopulateSegments(segments);

			Control.DisplayMode = Element.DisplayMode;
			Control.CornerRadius = Element.CornerRadius;
			Control.SegmentSelected += SegmentSelected;
		}

		void PopulateSegments(IList<string> segments)
		{
			for (var i = 0; i < segments.Count; i++)
			{
				Control.Children.Add(segments[i]);
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
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (Control == null || Element == null)
				return;

			if (e.PropertyName == SegmentedView.SelectedIndexProperty.PropertyName)
			{
				Control.CurrentSegment = (RadioButton)Control.GetChildAt(Element.SelectedIndex);
			}

			if (e.PropertyName == SegmentedView.ColorProperty.PropertyName)
			{
				Control.TintColor = Element.Color.ToAndroid();
			}

			if (e.PropertyName == SegmentedView.DisplayModeProperty.PropertyName)
				Control.DisplayMode = Element.DisplayMode;

			if (e.PropertyName == SegmentedView.CornerRadiusProperty.PropertyName)
			{
				Control.CornerRadius = Element.CornerRadius;
			}
		}

		RadioButton GetSegment(int index)
		{
			return (RadioButton)Control?.GetChildAt(index);
		}

		protected override void Dispose(bool disposing)
		{
			if (!disposing)
				return;

			InvalidateControl();
		}
	}
}
