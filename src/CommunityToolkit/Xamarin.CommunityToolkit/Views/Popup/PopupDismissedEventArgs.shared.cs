using System;

namespace Xamarin.CommunityToolkit.UI.Views
{
	/// <summary>
	/// Popup dismissed event arguments used when a popup is dismissed.
	/// </summary>
	public class PopupDismissedEventArgs : EventArgs
	{
		/// <summary>
		/// The resulting object to return.
		/// </summary>
		public object Result { get; set; }
	}
}
