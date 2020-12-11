using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Content;
using Android.Media;
using Android.Widget;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class FormsVideoView : VideoView
	{
		public event EventHandler MetadataRetrieved;

		public FormsVideoView(Context context)
			: base(context)
		{
			SetBackgroundColor(global::Android.Graphics.Color.Transparent);
		}

		public override void SetVideoPath(string path)
		{
			base.SetVideoPath(path);

			if (System.IO.File.Exists(path))
			{
				var retriever = new MediaMetadataRetriever();

				Task.Run(() =>
				{
					retriever.SetDataSource(path);
					ExtractMetadata(retriever);
					MetadataRetrieved?.Invoke(this, EventArgs.Empty);
				});
			}
		}

		protected void ExtractMetadata(MediaMetadataRetriever retriever)
		{
			if (int.TryParse(retriever.ExtractMetadata(MetadataKey.VideoWidth), out var videoWidth))
				VideoWidth = videoWidth;

			if (int.TryParse(retriever.ExtractMetadata(MetadataKey.VideoHeight), out var videoHeight))
				VideoHeight = videoHeight;

			var durationString = retriever.ExtractMetadata(MetadataKey.Duration);

			if (!string.IsNullOrEmpty(durationString) && long.TryParse(durationString, out var durationMS))
				DurationTimeSpan = TimeSpan.FromMilliseconds(durationMS);
			else
				DurationTimeSpan = null;
		}

		public override void SetVideoURI(global::Android.Net.Uri uri, IDictionary<string, string> headers)
		{
			GetMetaData(uri, headers);
			base.SetVideoURI(uri, headers);
		}

		protected void GetMetaData(global::Android.Net.Uri uri, IDictionary<string, string> headers)
		{
			Task.Run(() =>
			{
				var retriever = new MediaMetadataRetriever();

				if (uri.Scheme != null && uri.Scheme.StartsWith("http"))
				{
					retriever.SetDataSource(uri.ToString(), headers ?? new Dictionary<string, string>());
				}
				else
				{
					retriever.SetDataSource(Context, uri);
				}

				ExtractMetadata(retriever);

				MetadataRetrieved?.Invoke(this, EventArgs.Empty);
			});
		}

		public int VideoHeight { get; private set; }

		public int VideoWidth { get; private set; }

		public TimeSpan? DurationTimeSpan { get; private set; }

		public TimeSpan Position => TimeSpan.FromMilliseconds(CurrentPosition);
	}
}
