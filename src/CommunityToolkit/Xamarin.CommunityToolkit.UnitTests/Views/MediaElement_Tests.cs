using System;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Core;
using Xamarin.CommunityToolkit.UI.Views;

namespace Xamarin.CommunityToolkit.UnitTests.Views
{
	public class MediaElement_Tests
	{
		[Test]
		public void TestSource()
		{
			var mediaElement = new MediaElement();

			Assert.Null(mediaElement.Source);

			var signaled = false;
			mediaElement.PropertyChanged += (sender, e) =>
			{
				if (e.PropertyName == "Source")
					signaled = true;
			};

			var source = MediaSource.FromFile("Video.mp4");
			mediaElement.Source = source;

			Assert.AreEqual(source, mediaElement.Source);
			Assert.IsTrue(signaled);
		}

		[Test]
		public void TestSourceDoubleSet()
		{
			var mediaElement = new MediaElement { Source = MediaSource.FromFile("Video.mp4") };

			var signaled = false;
			mediaElement.PropertyChanged += (sender, e) =>
			{
				if (e.PropertyName == "Source")
					signaled = true;
			};

			mediaElement.Source = mediaElement.Source;

			Assert.False(signaled);
		}

		[Test]
		public void TestFileMediaSourceChanged()
		{
			var source = (FileMediaSource)MediaSource.FromFile("Video.mp4");

			var signaled = false;
			source.SourceChanged += (sender, e) =>
			{
				signaled = true;
			};

			source.File = "Other.mp4";
			Assert.AreEqual("Other.mp4", source.File);

			Assert.IsTrue(signaled);
		}

		[Test]
		public void TestSourceRoundTrip()
		{
			var uri = new Uri("https://sec.ch9.ms/ch9/5d93/a1eab4bf-3288-4faf-81c4-294402a85d93/XamarinShow_mid.mp4");
			var media = new MediaElement();

			Assert.Null(media.Source);

			media.Source = uri;

			Assert.IsNotNull(media.Source);
			Assert.IsInstanceOf<UriMediaSource>(media.Source);
			Assert.AreEqual(uri, ((UriMediaSource?)media.Source)?.Uri);
		}

		[Test]
		public void TestDefaultValueForShowsPlaybackControls()
		{
			var media = new MediaElement();

			Assert.IsTrue(media.ShowsPlaybackControls);
		}

		[Test]
		public void TestShowsPlaybackControlsSet()
		{
			var media = new MediaElement { ShowsPlaybackControls = false };

			Assert.False(media.ShowsPlaybackControls);
		}
	}
}