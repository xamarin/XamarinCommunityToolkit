using System;using Microsoft.Extensions.Logging;
using Xamarin.CommunityToolkit.Helpers;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Internals;

namespace Xamarin.CommunityToolkit.Core
{
	[System.ComponentModel.TypeConverter(typeof(MediaSourceConverter))]
	public abstract class MediaSource : Element
	{
		readonly WeakEventManager weakEventManager = new WeakEventManager();

		public static MediaSource FromFile(string? file) =>
			new FileMediaSource { File = file };

		public static MediaSource? FromUri(Uri? uri)
		{
			if (uri == null)
				return null;

			return !uri.IsAbsoluteUri ? throw new ArgumentException("Uri must be be absolute", nameof(uri)) : new UriMediaSource { Uri = uri };
		}

		public static MediaSource? FromUri(string uri) => FromUri(new Uri(uri));

		[Microsoft.Maui.Controls.Internals.Preserve(Conditional = true)]
		public static implicit operator MediaSource?(string? source) =>
			Uri.TryCreate(source, UriKind.Absolute, out var uri) && uri.Scheme != "file"
				? FromUri(uri)
				: FromFile(source);

		[Microsoft.Maui.Controls.Internals.Preserve(Conditional = true)]
		public static implicit operator MediaSource?(Uri? uri) => FromUri(uri);

		protected void OnSourceChanged() =>
			weakEventManager.RaiseEvent(this, EventArgs.Empty, nameof(SourceChanged));

		internal event EventHandler SourceChanged
		{
			add => weakEventManager.AddEventHandler(value);
			remove => weakEventManager.RemoveEventHandler(value);
		}
	}
}