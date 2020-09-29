using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Xamarin.CommunityToolkit.Actions.Snackbar
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class SnackbarArguments : ActionArguments
	{
		public SnackbarArguments(string message, int duration, string actionButtonText, Func<Task> action) : base(message, duration)
		{
			ActionButtonText = actionButtonText;
			Action = action;
		}

		/// <summary>
		///     Gets the text for the action button. Can be null.
		/// </summary>
		public string ActionButtonText { get; }

		/// <summary>
		///     Gets the action for the snackbar.
		/// </summary>
		public Func<Task> Action { get; }
	}
}