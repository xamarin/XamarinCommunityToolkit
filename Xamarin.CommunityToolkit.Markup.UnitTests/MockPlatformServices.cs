using System;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using FileAccess = System.IO.FileAccess;
using FileMode = System.IO.FileMode;
using FileShare = System.IO.FileShare;
using Internals = Xamarin.Forms.Internals;
using Stream = System.IO.Stream;

namespace Xamarin.CommunityToolkit.Markup.UnitTests
{
	// Minimum implementation - only what is needed to run the C# Markup unit tests
	internal class MockPlatformServices : Internals.IPlatformServices
	{
		public string GetHash(string input) => throw new NotImplementedException();

		string IPlatformServices.GetMD5Hash(string input)  => throw new NotImplementedException();

		public double GetNamedSize(NamedSize size, Type targetElement, bool useOldSizes)
		{
			switch (size)
			{
				case NamedSize.Default:
					return 10;
				default:
					throw new ArgumentOutOfRangeException(nameof(size));
			}
		}

		public Color GetNamedColor(string name) => throw new NotImplementedException();

		public void OpenUriAction(Uri uri) => throw new NotImplementedException();

		public bool IsInvokeRequired => throw new NotImplementedException();

		public string RuntimePlatform => throw new NotImplementedException();

		public void BeginInvokeOnMainThread(Action action) => throw new NotImplementedException();

		public Internals.Ticker CreateTicker() => throw new NotImplementedException();

		public void StartTimer(TimeSpan interval, Func<bool> callback) => throw new NotImplementedException();

		public Task<Stream> GetStreamAsync(Uri uri, CancellationToken cancellationToken) => throw new NotImplementedException();

		public Assembly[] GetAssemblies() => AppDomain.CurrentDomain.GetAssemblies();

		public Internals.IIsolatedStorageFile GetUserStoreForApplication() => new MockIsolatedStorageFile(IsolatedStorageFile.GetUserStoreForAssembly());

		public class MockIsolatedStorageFile : Internals.IIsolatedStorageFile
		{
			readonly IsolatedStorageFile isolatedStorageFile;

			public MockIsolatedStorageFile(IsolatedStorageFile isolatedStorageFile) => this.isolatedStorageFile = isolatedStorageFile;

			public Task<bool> GetDirectoryExistsAsync(string path) => Task.FromResult(isolatedStorageFile.DirectoryExists(path));

			public Task CreateDirectoryAsync(string path) => throw new NotImplementedException();

			public Task<Stream> OpenFileAsync(string path, FileMode mode, FileAccess access) => throw new NotImplementedException();

			public Task<Stream> OpenFileAsync(string path, FileMode mode, FileAccess access, FileShare share) => throw new NotImplementedException();

			public Task<bool> GetFileExistsAsync(string path) => throw new NotImplementedException();

			public Task<DateTimeOffset> GetLastWriteTimeAsync(string path) => throw new NotImplementedException();
		}

		public void QuitApplication() => throw new NotImplementedException();

		public SizeRequest GetNativeSize(VisualElement view, double widthConstraint, double heightConstraint) => throw new NotImplementedException();

		public OSAppTheme RequestedTheme  => throw new NotImplementedException();

	}
}