using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class StreamMediaSource : MediaSource, IStreamImageSource
	{
		readonly object synchandle = new object();
		CancellationTokenSource cancellationTokenSource;

		TaskCompletionSource<bool> completionSource;

		public static readonly BindableProperty StreamProperty = BindableProperty.Create(nameof(Stream), typeof(Func<CancellationToken, Task<Stream>>), typeof(StreamMediaSource),
			default(Func<CancellationToken, Task<Stream>>));

		protected CancellationTokenSource CancellationTokenSource
		{
			get => cancellationTokenSource;
			private set
			{
				if (cancellationTokenSource == value)
					return;
				if (cancellationTokenSource != null)
				{
					cancellationTokenSource.Cancel();
					cancellationTokenSource.Dispose();
				}
				cancellationTokenSource = value;
			}
		}

		bool IsLoading => cancellationTokenSource != null;

		public virtual Func<CancellationToken, Task<Stream>> Stream
		{
			get => (Func<CancellationToken, Task<Stream>>)GetValue(StreamProperty);
			set => SetValue(StreamProperty, value);
		}

		protected override void OnPropertyChanged(string propertyName)
		{
			if (propertyName == StreamProperty.PropertyName)
				OnSourceChanged();
			base.OnPropertyChanged(propertyName);
		} 

		async Task<Stream> IStreamImageSource.GetStreamAsync(CancellationToken userToken)
		{
			if (Stream == null)
				return null;

			OnLoadingStarted();
			userToken.Register(CancellationTokenSource.Cancel);
			try
			{
				var stream = await Stream(CancellationTokenSource.Token);
				OnLoadingCompleted(false);
				return stream;
			}
			catch (OperationCanceledException)
			{
				OnLoadingCompleted(true);
				throw;
			}
		}

		protected void OnLoadingCompleted(bool cancelled)
		{
			if (!IsLoading || completionSource == null)
				return;

			var tcs = Interlocked.Exchange(ref completionSource, null);
			if (tcs != null)
				tcs.SetResult(cancelled);

			lock (synchandle)
			{
				CancellationTokenSource = null;
			}
		}

		protected void OnLoadingStarted()
		{
			lock (synchandle)
			{
				CancellationTokenSource = new CancellationTokenSource();
			}
		}

		public virtual Task<bool> Cancel()
		{
			if (!IsLoading)
				return Task.FromResult(false);

			var tcs = new TaskCompletionSource<bool>();
			var original = Interlocked.CompareExchange(ref completionSource, tcs, null);

			if (original == null)
			{
				CancellationTokenSource = null;
			}
			else
			{
				tcs = original;
			}

			return tcs.Task;
		}
	}
}