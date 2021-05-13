using System;
using NUnit.Framework;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UnitTests.Views
{
	public class MediaSource_Tests
	{
		[Test]
		public void TestConstructors()
		{
			var filesource = new Core.FileMediaSource { File = "File.mp4" };
			Assert.AreEqual("File.mp4", filesource.File);

			var urisource = new Core.UriMediaSource { Uri = new Uri("http://xamarin.com/media.mp4") };
			Assert.AreEqual("http://xamarin.com/media.mp4", urisource.Uri.AbsoluteUri);
		}

		[Test]
		public void TestHelpers()
		{
			var mediasource = Core.MediaSource.FromFile("File.mp4");
			Assert.IsInstanceOf<Core.FileMediaSource>(mediasource);
			Assert.AreEqual("File.mp4", ((Core.FileMediaSource)mediasource).File);

			var urisource = Core.MediaSource.FromUri(new Uri("http://xamarin.com/media.mp4"));
			Assert.IsInstanceOf<Core.UriMediaSource>(urisource);
			Assert.AreEqual("http://xamarin.com/media.mp4", ((Core.UriMediaSource?)urisource)?.Uri?.AbsoluteUri);
		}

		[Test]
		public void TestImplicitFileConversion()
		{
			var mediaElement = new UI.Views.MediaElement { Source = "File.mp4" };

			Assert.IsNotNull(mediaElement.Source);
			Assert.IsInstanceOf<Core.FileMediaSource>(mediaElement.Source);
			Assert.AreEqual("File.mp4", ((Core.FileMediaSource?)mediaElement.Source)?.File);
		}

		[Test]
		public void TestImplicitStringConversionWhenNull()
		{
			string? s = null;
			var sut = (Core.MediaSource?)s;

			Assert.IsInstanceOf<Core.FileMediaSource?>(sut);
			Assert.Null(((Core.FileMediaSource?)sut)?.File);
		}

		[Test]
		public void TestImplicitUriConversion()
		{
			var mediaElement = new UI.Views.MediaElement { Source = new Uri("http://xamarin.com/media.mp4") };

			Assert.IsNotNull(mediaElement.Source);
			Assert.IsInstanceOf<Core.UriMediaSource>(mediaElement.Source);
			Assert.AreEqual("http://xamarin.com/media.mp4", ((Core.UriMediaSource?)mediaElement.Source)?.Uri?.AbsoluteUri);
		}

		[Test]
		public void TestImplicitStringUriConversion()
		{
			var mediaElement = new UI.Views.MediaElement { Source = "http://xamarin.com/media.mp4" };

			Assert.IsNotNull(mediaElement.Source);
			Assert.IsInstanceOf<Core.UriMediaSource>(mediaElement.Source);
			Assert.AreEqual("http://xamarin.com/media.mp4", ((Core.UriMediaSource?)mediaElement.Source)?.Uri?.AbsoluteUri);
		}

		[Test]
		public void TestImplicitUriConversionWhenNull()
		{
			Uri? u = null;
			var sut = (Core.MediaSource?)u;

			Assert.Null(sut);
		}

		[Test]
		public void TestSetStringValue()
		{
			var mediaElement = new UI.Views.MediaElement();
			mediaElement.SetValue(UI.Views.MediaElement.SourceProperty, "media.mp4");

			Assert.IsNotNull(mediaElement.Source);
			Assert.IsInstanceOf<Core.FileMediaSource>(mediaElement.Source);
			Assert.AreEqual("media.mp4", ((Core.FileMediaSource?)mediaElement.Source)?.File);
		}

		[Test]
		public void TextBindToStringValue()
		{
			var mediaElement = new UI.Views.MediaElement();
			mediaElement.SetBinding(UI.Views.MediaElement.SourceProperty, ".");

			Assert.Null(mediaElement.Source);

			mediaElement.BindingContext = "media.mp4";

			Assert.IsNotNull(mediaElement.Source);
			Assert.IsInstanceOf<Core.FileMediaSource>(mediaElement.Source);
			Assert.AreEqual("media.mp4", ((Core.FileMediaSource?)mediaElement.Source)?.File);
		}

		[Test]
		public void TextBindToStringUriValue()
		{
			var mediaElement = new UI.Views.MediaElement();
			mediaElement.SetBinding(UI.Views.MediaElement.SourceProperty, ".");

			Assert.Null(mediaElement.Source);

			mediaElement.BindingContext = "http://xamarin.com/media.mp4";

			Assert.IsNotNull(mediaElement.Source);
			Assert.IsInstanceOf<Core.UriMediaSource>(mediaElement.Source);
			Assert.AreEqual("http://xamarin.com/media.mp4", ((Core.UriMediaSource?)mediaElement.Source)?.Uri?.AbsoluteUri);
		}

		[Test]
		public void TextBindToUriValue()
		{
			var mediaElement = new UI.Views.MediaElement();
			mediaElement.SetBinding(UI.Views.MediaElement.SourceProperty, ".");

			Assert.Null(mediaElement.Source);

			mediaElement.BindingContext = new Uri("http://xamarin.com/media.mp4");

			Assert.IsNotNull(mediaElement.Source);
			Assert.IsInstanceOf<Core.UriMediaSource>(mediaElement.Source);
			Assert.AreEqual("http://xamarin.com/media.mp4", ((Core.UriMediaSource?)mediaElement.Source)?.Uri?.AbsoluteUri);
		}

		[Test]
		public void TestBindingContextPropagation()
		{
			var context = new object();
			var mediaElement = new UI.Views.MediaElement
			{
				BindingContext = context
			};
			var source = new MockMediaSource();
			mediaElement.Source = source;

			Assert.AreSame(context, source.BindingContext);

			mediaElement = new UI.Views.MediaElement();
			source = new MockMediaSource();
			mediaElement.Source = source;
			mediaElement.BindingContext = context;

			Assert.AreSame(context, source.BindingContext);
		}

		[Test]
		public void ImplicitCastOnAbsolutePathsShouldCreateAFileMediaSource()
		{
			var path = "/private/var/mobile/Containers/Data/Application/B1E5AB19-F815-4B4A-AB97-BD4571D53743/Documents/temp/video.mp4";
			var mediaElement = new UI.Views.MediaElement { Source = path };

			Assert.IsInstanceOf<Core.FileMediaSource>(mediaElement.Source);
		}

		[Test]
		public void ImplicitCastOnWindowsAbsolutePathsShouldCreateAFileMediaSource()
		{
			var path = "C:\\Users\\Username\\Videos\\video.mp4";
			var mediaElement = new UI.Views.MediaElement { Source = path };

			Assert.IsInstanceOf<Core.FileMediaSource>(mediaElement.Source);
		}

		class MockMediaSource : Core.MediaSource
		{
		}
	}
}