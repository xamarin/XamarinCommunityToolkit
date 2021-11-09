using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.CommunityToolkit.Extensions
{
	static class TaskExtensions
	{
		/// <summary>
		/// Provides a mechanism to await until the supplied <paramref name="cancellationToken"/> has been cancelled.
		/// </summary>
		/// <param name="cancellationToken">The <see cref="CancellationToken"/> to await.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		public static Task WhenCanceled(this CancellationToken cancellationToken)
		{
			var completionSource = new TaskCompletionSource<bool>();

			cancellationToken.Register(
				s =>
				{
					if (s is TaskCompletionSource<bool> taskCompletionSource)
					{
						taskCompletionSource.SetResult(true);
					}
				},
				completionSource);

			return completionSource.Task;
		}
	}
}