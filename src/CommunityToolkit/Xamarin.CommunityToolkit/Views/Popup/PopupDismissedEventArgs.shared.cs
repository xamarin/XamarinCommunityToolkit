using System;

namespace Xamarin.CommunityToolkit.UI.Views
{
	/// <summary>
	/// Popup dismissed event arguments used when a popup is dismissed.
	/// </summary>
	public class PopupDismissedEventArgs : PopupDismissedEventArgs<object?>
	{
		public PopupDismissedEventArgs(object? result)
			: base(result)
		{
		}
	}

	/// <summary>
	/// Popup dismissed event arguments used when a popup is dismissed.
	/// </summary>
	public class PopupDismissedEventArgs<T> : EventArgs
	{
		/// <summary>
		/// Initialization an instance of <see cref="PopupDismissedEventArgs"/>.
		/// </summary>
		/// <param name="result">
		/// The result of the popup.
		/// </param>
		public PopupDismissedEventArgs(T result) =>
			Result = result;

		/// <summary>
		/// The resulting object to return.
		/// </summary>
		public T Result { get; }
	}
}