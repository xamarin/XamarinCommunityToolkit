using System;

namespace Xamarin.CommunityToolkit.UI.Views
{
	/// <summary>
	/// Event Arguments for the <see cref="SideMenuView"/> State
	/// that is triggered right after the state has changed.
	/// </summary>
	public class SideMenuStateChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Instantiates the default instance of <see cref="SideMenuStateChangedEventArgs"/>.
		/// </summary>
		/// <param name="state">
		/// The new <see cref="SideMenuState"/> for the current <see cref="SideMenuView"/>.
		/// </param>
		public SideMenuStateChangedEventArgs(SideMenuState state) =>
			State = state;

		/// <summary>
		/// Gets or sets the <see cref="SideMenuState"/>.
		/// </summary>
		public SideMenuState State { get; }
	}
}
