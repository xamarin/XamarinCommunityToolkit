using System;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;
using FileAccess = System.IO.FileAccess;
using FileMode = System.IO.FileMode;
using FileShare = System.IO.FileShare;
using Stream = System.IO.Stream;

namespace Xamarin.CommunityToolkit.Markup.MauiCompat.UnitTests
{
    // Minimum implementation - only what is needed to run the C# Markup unit tests
    class MockPlatformServices : IPlatformServices
    {
        public string GetHash(string input) => throw new NotImplementedException();

        public double GetNamedSize(NamedSize size, Type targetElement, bool useOldSizes) => size switch
        {
            NamedSize.Default => 10,
            _ => throw new ArgumentOutOfRangeException(nameof(size)),
        };

        public Color GetNamedColor(string name) => throw new NotImplementedException();

        public void OpenUriAction(Uri uri) => throw new NotImplementedException();

        public bool IsInvokeRequired => throw new NotImplementedException();

        public string RuntimePlatform => throw new NotImplementedException();

        public void BeginInvokeOnMainThread(Action action) => throw new NotImplementedException();

        public void StartTimer(TimeSpan interval, Func<bool> callback) => throw new NotImplementedException();

        public Task<Stream> GetStreamAsync(Uri uri, CancellationToken cancellationToken) => throw new NotImplementedException();

        public Assembly[] GetAssemblies() => AppDomain.CurrentDomain.GetAssemblies();

        public IIsolatedStorageFile GetUserStoreForApplication() => new MockIsolatedStorageFile(IsolatedStorageFile.GetUserStoreForAssembly());

        public class MockIsolatedStorageFile : IIsolatedStorageFile
        {
            readonly IsolatedStorageFile isolatedStorageFile;

            public MockIsolatedStorageFile(IsolatedStorageFile isolatedStorageFile) => this.isolatedStorageFile = isolatedStorageFile;

            public Task<bool> GetDirectoryExistsAsync(string path) => Task.FromResult(isolatedStorageFile.DirectoryExists(path));

            public Task CreateDirectoryAsync(string path)
            {
                isolatedStorageFile.CreateDirectory(path);
                return Task.FromResult(true);
            }

            public Task<Stream> OpenFileAsync(string path, FileMode mode, FileAccess access) => throw new NotImplementedException();

            public Task<Stream> OpenFileAsync(string path, FileMode mode, FileAccess access, FileShare share) => throw new NotImplementedException();

            public Task<bool> GetFileExistsAsync(string path) => throw new NotImplementedException();

            public Task<DateTimeOffset> GetLastWriteTimeAsync(string path) => throw new NotImplementedException();
        }

        public void QuitApplication() => throw new NotImplementedException();

        public SizeRequest GetNativeSize(VisualElement view, double widthConstraint, double heightConstraint) => throw new NotImplementedException();

        public OSAppTheme RequestedTheme => throw new NotImplementedException();
    }
}