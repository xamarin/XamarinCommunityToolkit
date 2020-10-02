using System.ComponentModel;
using System.Threading.Tasks;

namespace Xamarin.CommunityToolkit.UI.Views
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	class ActionArguments
	{
		public ActionArguments(string message, int duration)
		{
			Duration = duration;
			Message = message;
			Result = new TaskCompletionSource<bool>(false);
		}

		/// <summary>
		///     Gets the message for the snackbar. Can be null.
		/// </summary>
		public string Message { get; }

		/// <summary>
		///     Result is true if ActionButton is clicked.
		/// </summary>
		public TaskCompletionSource<bool> Result { get; }

		/// <summary>
		///     Gets the duration for the snackbar.
		/// </summary>
		public int Duration { get; }

		public void SetResult(bool result) => Result.TrySetResult(result);
	}
}