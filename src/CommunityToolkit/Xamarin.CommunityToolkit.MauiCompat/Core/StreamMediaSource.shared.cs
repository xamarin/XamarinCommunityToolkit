using System;using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Core
{
	public class StreamMediaSource : MediaSource, IStreamImageSource
	{
		public bool IsEmpty => Stream == null; readonly object synchandle = new object();
		CancellationTokenSource? cancellationTokenSource;

		TaskCompletionSource<bool>? completionSource;

		public static readonly BindableProperty StreamProperty
			= BindableProperty.Create(nameof(Stream), typeof(Func<CancellationToken, Task<Stream>>), typeof(StreamMediaSource));

		protected CancellationTokenSource? CancellationTokenSource
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

		public virtual Func<CancellationToken, Task<Stream>>? Stream
		{
			get => (Func<CancellationToken, Task<Stream>>?)GetValue(StreamProperty);
			set => SetValue(StreamProperty, value);
		}

		protected override void OnPropertyChanged(string propertyName)
		{
			if (propertyName == StreamProperty.PropertyName)
				OnSourceChanged();
			base.OnPropertyChanged(propertyName);
		}

		async Task<Stream?> IStreamImageSource.GetStreamAsync(CancellationToken userToken)
		{
			if (Stream == null)
				return null;

			OnLoadingStarted();

			if (CancellationTokenSource == null)
				throw new InvalidOperationException($"{nameof(OnLoadingStarted)} not called");

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

			var tcs = Interlocked.Exchange<TaskCompletionSource<bool>?>(ref completionSource, null);
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
				CancellationTokenSource = null;
			else
				tcs = original;

			return tcs.Task;
		}
	}
}