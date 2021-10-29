using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Xamarin.CommunityToolkit.UnitTests.Mocks
{
	class MockPlatformServices : IPlatformServices
	{
		public string GetHash(string input) => string.Empty;

		public string GetMD5Hash(string input) => string.Empty;

		public double GetNamedSize(NamedSize size, Type targetElement, bool useOldSizes) => 0;

		public Color GetNamedColor(string name) => Color.Default;

		public void OpenUriAction(Uri uri)
		{
		}

		public bool IsInvokeRequired { get; } = false;

		public OSAppTheme RequestedTheme { get; } = OSAppTheme.Unspecified;

		public string RuntimePlatform { get; set; } = string.Empty;

		public void BeginInvokeOnMainThread(Action action) => action();

		public Ticker CreateTicker() => new MockTicker(TimeSpan.FromMilliseconds(16));

		public void StartTimer(TimeSpan interval, Func<bool> callback)
		{
		}

		public Task<Stream> GetStreamAsync(Uri uri, CancellationToken cancellationToken)
			=> Task.FromResult<Stream>(new MemoryStream());

		public Assembly[] GetAssemblies() => Array.Empty<Assembly>();

		public IIsolatedStorageFile? GetUserStoreForApplication() => null;

		Assembly[] IPlatformServices.GetAssemblies() => Array.Empty<Assembly>();

		public void QuitApplication()
		{
		}

		public SizeRequest GetNativeSize(VisualElement view, double widthConstraint, double heightConstraint) => default;
	}

	class MockTicker : Ticker
	{
		bool enabled;
		readonly TimeSpan delayBetweenSignals;

		public MockTicker(TimeSpan delayBetweenSignals)
		{
			this.delayBetweenSignals = delayBetweenSignals;
		}

		protected async override void EnableTimer()
		{
			enabled = true;

			while (enabled)
			{
				await Task.Delay(delayBetweenSignals);
				SendSignals((int)delayBetweenSignals.TotalMilliseconds);
			}
		}

		protected override void DisableTimer() => enabled = false;
	}
}