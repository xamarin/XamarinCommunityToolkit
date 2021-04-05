using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Xamarin.CommunityToolkit.Extensions.iOS;
using Xamarin.CommunityToolkit.iOS.UI.Views;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SegmentedView), typeof(SegmentedViewRenderer))]

namespace Xamarin.CommunityToolkit.iOS.UI.Views
{
	public class SegmentedViewRenderer : ViewRenderer<SegmentedView, UISegmentedControl>
	{
		const string TAG = "SegmentedView";
		bool isDisposed;

		protected override async void OnElementChanged(ElementChangedEventArgs<SegmentedView> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null && Control != null)
				InvalidateControl();

			if (e.NewElement == null)
				return;

			if (Control == null)
				SetNativeControl(new UISegmentedControl());

			if (e.NewElement != null)
			{
				try
				{
					await InitializeControl();
				}
				catch (Exception ex)
				{
					Log.Warning(TAG, ex.Message);
				}
			}
		}

		async Task InitializeControl()
		{
			var items = Element.Items;
			await PopulateSegments(items);
			Control.ClipsToBounds = true;
			Control.SelectedSegment = Element.SelectedIndex;
			Control.Layer.MasksToBounds = true;
			Control.BackgroundColor = Element.BackgroundColor.ToUIColor();
			UpdateSelectedSegment(Element.SelectedIndex);

			Control.ValueChanged += OnSelectedIndexChanged;
			((INotifyCollectionChanged)Element.Items).CollectionChanged += SegmentsCollectionChanged;

			if (Element.IsColorSet)
			{
				Control.SelectedSegmentTintColor = Element.Color.ToUIColor();
				Control.TintColor = Element.Color.ToUIColor();
			}

			if (Element.IsNormalTextColorSet)
			{
				Control.SetTitleTextAttributes(new UITextAttributes() { TextColor = Element.NormalTextColor.ToUIColor() }, UIControlState.Normal);
			}

			if (Element.IsSelectedTextColorSet)
			{
				Control.SetTitleTextAttributes(new UITextAttributes() { TextColor = Element.SelectedTextColor.ToUIColor() }, UIControlState.Selected);
			}
		}

		async ValueTask PopulateSegments(IEnumerable<string> segments)
		{
			var segmentsCount = segments.Count();

			for (var i = 0; i < segmentsCount; i++)
			{
				await InsertSegment(segments.ElementAt(i), i);
			}
		}

		async ValueTask InsertSegment(string segment, int position)
		{
			switch (Element.DisplayMode)
			{
				case SegmentMode.Image:
					var img = await ((ImageSource)segment).GetNativeImageAsync();
					if (img != null)
						Control.InsertSegment(img, position, false);
					else
						Log.Warning(TAG, "ImageSource is null.");
					break;
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

		async void SegmentsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					var startIndex = e.NewStartingIndex;
					foreach (var item in e.NewItems)
					{
						try
						{
							await InsertSegment((string)item, startIndex++);
						}
						catch (Exception ex)
						{
							Log.Warning(TAG, ex.Message);
						}
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

			switch (e.PropertyName)
			{
				case "SelectedItem":
				case "SelectedIndex":
					UpdateSelectedSegment(Element.SelectedIndex);
					break;
				case "Color":
					Control.SelectedSegmentTintColor = Element.Color.ToUIColor();
					break;
				case "NormalTextColor":
					Control.SetTitleTextAttributes(new UITextAttributes() { TextColor = Element.NormalTextColor.ToUIColor() }, UIControlState.Normal);
					break;
				case "SelectedTextColor":
					Control.SetTitleTextAttributes(new UITextAttributes() { TextColor = Element.SelectedTextColor.ToUIColor() }, UIControlState.Selected);
					break;
				default:
					break;
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
			if (disposing)
				return;

			if (isDisposed)
				return;

			Control.ValueChanged -= OnSelectedIndexChanged;
			Control.Dispose();

			isDisposed = true;
			base.Dispose(disposing);
		}

		async ValueTask<UIImage?> LoadImageAsync(ImageSource imagesource, CancellationToken cancelationToken = default(CancellationToken), float scale = 1f)
		{
			UIImage? image = null;
			var streamsource = imagesource as StreamImageSource;
			if (streamsource?.Stream != null)
			{
				using (var streamImage = await ((IStreamImageSource)streamsource).GetStreamAsync(cancelationToken).ConfigureAwait(false))
				{
					if (streamImage != null)
						image = UIImage.LoadFromData(NSData.FromStream(streamImage), scale);
				}
			}

			if (image == null)
			{
				Log.Warning(TAG, "Could not load image: {0}", streamsource);
			}

			return image;
		}
	}
}
