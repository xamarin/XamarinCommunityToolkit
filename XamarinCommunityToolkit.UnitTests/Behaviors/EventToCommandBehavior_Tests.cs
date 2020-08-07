using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Behaviors
{
    public class EventToCommandBehavior_Tests
    {
        public EventToCommandBehavior_Tests()
            => Device.PlatformServices = new MockPlatformServices();

        [Fact]
        public void ArgumentExceptionIfSpecifiedEventDoesNotExist()
        {
            var listView = new ListView();
            var behavior = new EventToCommandBehavior
            {
                EventName = "Wrong Event Name"
            };
            Assert.Throws<ArgumentException>(() => listView.Behaviors.Add(behavior));
        }

        [Fact]
        public void NoExceptionIfSpecifiedEventExists()
        {
            var listView = new ListView();
            var behavior = new EventToCommandBehavior
            {
                EventName = nameof(ListView.ItemTapped)
            };
            listView.Behaviors.Add(behavior);
        }
    }

    #region Mock Services

    class MockPlatformServices : IPlatformServices
    {
        public string GetMD5Hash(string input)
            => string.Empty;

        public double GetNamedSize(NamedSize size, Type targetElement, bool useOldSizes)
            => 0;

        public void OpenUriAction(Uri uri)
        {
        }

        public bool IsInvokeRequired
            => false;

        public string RuntimePlatform { get; set; }

        public void BeginInvokeOnMainThread(Action action)
            => action();

        public Ticker CreateTicker()
            => new MockTicker();

        public void StartTimer(TimeSpan interval, Func<bool> callback)
        {
        }

        public Task<Stream> GetStreamAsync(Uri uri, CancellationToken cancellationToken)
            => Task.FromResult<Stream>(new MemoryStream());

        public Assembly[] GetAssemblies()
            => new Assembly[0];

        public IIsolatedStorageFile GetUserStoreForApplication()
            => null;

        Assembly[] IPlatformServices.GetAssemblies()
            => new Assembly[0];

        public void QuitApplication()
        {
        }

        public SizeRequest GetNativeSize(VisualElement view, double widthConstraint, double heightConstraint)
            => new SizeRequest();
    }

    class MockTicker : Ticker
    {
        protected override void DisableTimer()
        {
        }

        protected override void EnableTimer()
        {
        }
    }

    #endregion
}
