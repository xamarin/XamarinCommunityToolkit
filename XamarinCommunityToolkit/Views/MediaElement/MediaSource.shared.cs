using System;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	[TypeConverter(typeof(MediaSourceConverter))]
	public abstract class MediaSource : Element
	{
		readonly WeakEventManager weakEventManager = new WeakEventManager();

		public static MediaSource FromFile(string file) => new FileMediaSource { File = file };

		public static MediaSource FromUri(Uri uri)
		{
			if (!uri.IsAbsoluteUri)
				throw new ArgumentException("uri is relative");
			return new UriMediaSource { Uri = uri };
		}

		public static implicit operator MediaSource(string source) => Uri.TryCreate(source, UriKind.Absolute, out var uri) && uri.Scheme != "file" ? FromUri(uri) : FromFile(source);

		public static implicit operator MediaSource(Uri uri)
		{
			if (uri is null)
				return null;

			if (!uri.IsAbsoluteUri)
				throw new ArgumentException("uri is relative");

			return FromUri(uri);
		}

		protected void OnSourceChanged() => weakEventManager.RaiseEvent(this, EventArgs.Empty, nameof(SourceChanged));

		internal event EventHandler SourceChanged
		{
			add { weakEventManager.AddEventHandler(value); }
			remove { weakEventManager.RemoveEventHandler(value); }
		}
	}
}
