using System;
using Xamarin.Forms;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Views
{
	public class MediaSource_Tests
	{
		[Fact]
		public void TestConstructors()
		{
			var filesource = new Core.FileMediaSource { File = "File.mp4" };
			Assert.Equal("File.mp4", filesource.File);

			var urisource = new Core.UriMediaSource { Uri = new Uri("http://xamarin.com/media.mp4") };
			Assert.Equal("http://xamarin.com/media.mp4", urisource.Uri.AbsoluteUri);
		}

		[Fact]
		public void TestHelpers()
		{
			var mediasource = Core.MediaSource.FromFile("File.mp4");
			Assert.IsType<Core.FileMediaSource>(mediasource);
			Assert.Equal("File.mp4", ((Core.FileMediaSource)mediasource).File);

			var urisource = Core.MediaSource.FromUri(new Uri("http://xamarin.com/media.mp4"));
			Assert.IsType<Core.UriMediaSource>(urisource);
			Assert.Equal("http://xamarin.com/media.mp4", ((Core.UriMediaSource)urisource).Uri.AbsoluteUri);
		}

		[Fact]
		public void TestImplicitFileConversion()
		{
			var mediaElement = new UI.Views.MediaElement { Source = "File.mp4" };
			Assert.True(mediaElement.Source != null);
			Assert.IsType<Core.FileMediaSource>(mediaElement.Source);
			Assert.Equal("File.mp4", ((Core.FileMediaSource)mediaElement.Source).File);
		}

		[Fact]
		public void TestImplicitStringConversionWhenNull()
		{
			string s = null;
			var sut = (Core.MediaSource)s;
			Assert.IsType<Core.FileMediaSource>(sut);
			Assert.Null(((Core.FileMediaSource)sut).File);
		}

		[Fact]
		public void TestImplicitUriConversion()
		{
			var mediaElement = new UI.Views.MediaElement { Source = new Uri("http://xamarin.com/media.mp4") };
			Assert.True(mediaElement.Source != null);
			Assert.IsType<Core.UriMediaSource>(mediaElement.Source);
			Assert.Equal("http://xamarin.com/media.mp4", ((Core.UriMediaSource)mediaElement.Source).Uri.AbsoluteUri);
		}

		[Fact]
		public void TestImplicitStringUriConversion()
		{
			var mediaElement = new UI.Views.MediaElement { Source = "http://xamarin.com/media.mp4" };
			Assert.True(mediaElement.Source != null);
			Assert.IsType<Core.UriMediaSource>(mediaElement.Source);
			Assert.Equal("http://xamarin.com/media.mp4", ((Core.UriMediaSource)mediaElement.Source).Uri.AbsoluteUri);
		}

		[Fact]
		public void TestImplicitUriConversionWhenNull()
		{
			Uri u = null;
			var sut = (Core.MediaSource)u;
			Assert.Null(sut);
		}

		[Fact]
		public void TestSetStringValue()
		{
			var mediaElement = new UI.Views.MediaElement();
			mediaElement.SetValue(UI.Views.MediaElement.SourceProperty, "media.mp4");
			Assert.NotNull(mediaElement.Source);
			Assert.IsType<Core.FileMediaSource>(mediaElement.Source);
			Assert.Equal("media.mp4", ((Core.FileMediaSource)mediaElement.Source).File);
		}

		[Fact]
		public void TextBindToStringValue()
		{
			var mediaElement = new UI.Views.MediaElement();
			mediaElement.SetBinding(UI.Views.MediaElement.SourceProperty, ".");
			Assert.Null(mediaElement.Source);
			mediaElement.BindingContext = "media.mp4";
			Assert.NotNull(mediaElement.Source);
			Assert.IsType<Core.FileMediaSource>(mediaElement.Source);
			Assert.Equal("media.mp4", ((Core.FileMediaSource)mediaElement.Source).File);
		}

		[Fact]
		public void TextBindToStringUriValue()
		{
			var mediaElement = new UI.Views.MediaElement();
			mediaElement.SetBinding(UI.Views.MediaElement.SourceProperty, ".");
			Assert.Null(mediaElement.Source);
			mediaElement.BindingContext = "http://xamarin.com/media.mp4";
			Assert.NotNull(mediaElement.Source);
			Assert.IsType<Core.UriMediaSource>(mediaElement.Source);
			Assert.Equal("http://xamarin.com/media.mp4", ((Core.UriMediaSource)mediaElement.Source).Uri.AbsoluteUri);
		}

		[Fact]
		public void TextBindToUriValue()
		{
			var mediaElement = new UI.Views.MediaElement();
			mediaElement.SetBinding(UI.Views.MediaElement.SourceProperty, ".");
			Assert.Null(mediaElement.Source);
			mediaElement.BindingContext = new Uri("http://xamarin.com/media.mp4");
			Assert.NotNull(mediaElement.Source);
			Assert.IsType<Core.UriMediaSource>(mediaElement.Source);
			Assert.Equal("http://xamarin.com/media.mp4", ((Core.UriMediaSource)mediaElement.Source).Uri.AbsoluteUri);
		}

		[Fact]
		public void TestBindingContextPropagation()
		{
			var context = new object();
			var mediaElement = new UI.Views.MediaElement
			{
				BindingContext = context
			};
			var source = new MockMediaSource();
			mediaElement.Source = source;
			Assert.Same(context, source.BindingContext);

			mediaElement = new UI.Views.MediaElement();
			source = new MockMediaSource();
			mediaElement.Source = source;
			mediaElement.BindingContext = context;
			Assert.Same(context, source.BindingContext);
		}

		[Fact]
		public void ImplicitCastOnAbsolutePathsShouldCreateAFileMediaSource()
		{
			var path = "/private/var/mobile/Containers/Data/Application/B1E5AB19-F815-4B4A-AB97-BD4571D53743/Documents/temp/video.mp4";
			var mediaElement = new UI.Views.MediaElement { Source = path };
			Assert.IsType<Core.FileMediaSource>(mediaElement.Source);
		}

		[Fact]
		public void ImplicitCastOnWindowsAbsolutePathsShouldCreateAFileMediaSource()
		{
			var path = "C:\\Users\\Username\\Videos\\video.mp4";
			var mediaElement = new UI.Views.MediaElement { Source = path };
			Assert.IsType<Core.FileMediaSource>(mediaElement.Source);
		}

		class MockMediaSource : Core.MediaSource
		{
		}
	}
}