using System;
using Xamarin.CommunityToolkit.UnitTests.Mocks;
using Xamarin.Forms;
using Xunit;
using CommunityToolkit = Xamarin.CommunityToolkit.UI.Views;

namespace Xamarin.CommunityToolkit.UnitTests.Views
{
	public class MediaSource_Tests
	{
		public MediaSource_Tests()		
			=> Device.PlatformServices = new MockPlatformServices();

		[Fact]
		public void TestConstructors()
		{
			var filesource = new CommunityToolkit::FileMediaSource { File = "File.mp4" };
			Assert.Equal("File.mp4", filesource.File);

			var urisource = new CommunityToolkit::UriMediaSource { Uri = new Uri("http://xamarin.com/media.mp4") };
			Assert.Equal("http://xamarin.com/media.mp4", urisource.Uri.AbsoluteUri);
		}

		[Fact]
		public void TestHelpers()
		{
			var mediasource = CommunityToolkit::MediaSource.FromFile("File.mp4");
			Assert.IsType<CommunityToolkit::FileMediaSource>(mediasource);
			Assert.Equal("File.mp4", ((CommunityToolkit::FileMediaSource)mediasource).File);

			var urisource = CommunityToolkit::MediaSource.FromUri(new Uri("http://xamarin.com/media.mp4"));
			Assert.IsType<CommunityToolkit::UriMediaSource>(urisource);
			Assert.Equal("http://xamarin.com/media.mp4", ((CommunityToolkit::UriMediaSource)urisource).Uri.AbsoluteUri);
		}

		[Fact]
		public void TestImplicitFileConversion()
		{
			var mediaElement = new CommunityToolkit::MediaElement { Source = "File.mp4" };
			Assert.True(mediaElement.Source != null);
			Assert.IsType<CommunityToolkit::FileMediaSource>(mediaElement.Source);
			Assert.Equal("File.mp4", ((CommunityToolkit::FileMediaSource)(mediaElement.Source)).File);
		}

		[Fact]
		public void TestImplicitStringConversionWhenNull()
		{
			string s = null;
			var sut = (CommunityToolkit::MediaSource)s;
			Assert.IsType<CommunityToolkit::FileMediaSource>(sut);
			Assert.Null(((CommunityToolkit::FileMediaSource)sut).File);
		}

		[Fact]
		public void TestImplicitUriConversion()
		{
			var mediaElement = new CommunityToolkit::MediaElement { Source = new Uri("http://xamarin.com/media.mp4") };
			Assert.True(mediaElement.Source != null);
			Assert.IsType<CommunityToolkit::UriMediaSource>(mediaElement.Source);
			Assert.Equal("http://xamarin.com/media.mp4", ((CommunityToolkit::UriMediaSource)(mediaElement.Source)).Uri.AbsoluteUri);
		}

		[Fact]
		public void TestImplicitStringUriConversion()
		{
			var mediaElement = new CommunityToolkit::MediaElement { Source = "http://xamarin.com/media.mp4" };
			Assert.True(mediaElement.Source != null);
			Assert.IsType<CommunityToolkit::UriMediaSource>(mediaElement.Source);
			Assert.Equal("http://xamarin.com/media.mp4", ((CommunityToolkit::UriMediaSource)mediaElement.Source).Uri.AbsoluteUri);
		}

		[Fact]
		public void TestImplicitUriConversionWhenNull()
		{
			Uri u = null;
			var sut = (CommunityToolkit::MediaSource)u;
			Assert.Null(sut);
		}

		[Fact]
		public void TestSetStringValue()
		{
			var mediaElement = new CommunityToolkit::MediaElement();
			mediaElement.SetValue(CommunityToolkit::MediaElement.SourceProperty, "media.mp4");
			Assert.NotNull(mediaElement.Source);
			Assert.IsType<CommunityToolkit::FileMediaSource>(mediaElement.Source);
			Assert.Equal("media.mp4", ((CommunityToolkit::FileMediaSource)(mediaElement.Source)).File);
		}

		[Fact]
		public void TextBindToStringValue()
		{
			var mediaElement = new CommunityToolkit::MediaElement();
			mediaElement.SetBinding(CommunityToolkit::MediaElement.SourceProperty, ".");
			Assert.Null(mediaElement.Source);
			mediaElement.BindingContext = "media.mp4";
			Assert.NotNull(mediaElement.Source);
			Assert.IsType<CommunityToolkit::FileMediaSource>(mediaElement.Source);
			Assert.Equal("media.mp4", ((CommunityToolkit::FileMediaSource)(mediaElement.Source)).File);
		}

		[Fact]
		public void TextBindToStringUriValue()
		{
			var mediaElement = new CommunityToolkit::MediaElement();
			mediaElement.SetBinding(CommunityToolkit::MediaElement.SourceProperty, ".");
			Assert.Null(mediaElement.Source);
			mediaElement.BindingContext = "http://xamarin.com/media.mp4";
			Assert.NotNull(mediaElement.Source);
			Assert.IsType<CommunityToolkit::UriMediaSource>(mediaElement.Source);
			Assert.Equal("http://xamarin.com/media.mp4", ((CommunityToolkit::UriMediaSource)mediaElement.Source).Uri.AbsoluteUri);
		}

		[Fact]
		public void TextBindToUriValue()
		{
			var mediaElement = new CommunityToolkit::MediaElement();
			mediaElement.SetBinding(CommunityToolkit::MediaElement.SourceProperty, ".");
			Assert.Null(mediaElement.Source);
			mediaElement.BindingContext = new Uri("http://xamarin.com/media.mp4");
			Assert.NotNull(mediaElement.Source);
			Assert.IsType<CommunityToolkit::UriMediaSource>(mediaElement.Source);
			Assert.Equal("http://xamarin.com/media.mp4", ((CommunityToolkit::UriMediaSource)mediaElement.Source).Uri.AbsoluteUri);
		}

		class MockMediaSource : CommunityToolkit::MediaSource
		{
		}

		[Fact]
		public void TestBindingContextPropagation()
		{
			var context = new object();
			var mediaElement = new CommunityToolkit::MediaElement
			{
				BindingContext = context
			};
			var source = new MockMediaSource();
			mediaElement.Source = source;
			Assert.Same(context, source.BindingContext);

			mediaElement = new CommunityToolkit::MediaElement();
			source = new MockMediaSource();
			mediaElement.Source = source;
			mediaElement.BindingContext = context;
			Assert.Same(context, source.BindingContext);
		}

		[Fact]
		public void ImplicitCastOnAbsolutePathsShouldCreateAFileMediaSource()
		{
			var path = "/private/var/mobile/Containers/Data/Application/B1E5AB19-F815-4B4A-AB97-BD4571D53743/Documents/temp/video.mp4";
			var mediaElement = new CommunityToolkit::MediaElement { Source = path };
			Assert.IsType<CommunityToolkit::FileMediaSource>(mediaElement.Source);
		}

		[Fact]
		public void ImplicitCastOnWindowsAbsolutePathsShouldCreateAFileMediaSource()
		{
			var path = "C:\\Users\\Username\\Videos\\video.mp4";
			var mediaElement = new CommunityToolkit::MediaElement { Source = path };
			Assert.IsType<CommunityToolkit::FileMediaSource>(mediaElement.Source);
		}
	}
}