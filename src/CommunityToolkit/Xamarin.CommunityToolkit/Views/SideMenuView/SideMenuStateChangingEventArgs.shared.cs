using System;

namespace Xamarin.CommunityToolkit.UI.Views
{
	/// <summary>
	/// Event Arguments for the <see cref="SideMenuView"/> State that
	/// is triggered right before the state is changed.
	/// </summary>
	public class SideMenuStateChangingEventArgs : EventArgs
	{
		/// <summary>
		/// Instantiates the default instance of <see cref="SideMenuStateChangingEventArgs"/>.
		/// </summary>
		/// <param name="state">
		/// The new <see cref="SideMenuState"/> for the current <see cref="SideMenuView"/>.
		/// </param>
		public SideMenuStateChangingEventArgs(SideMenuState state) =>
			State = state;

		/// <summary>
		/// Gets or sets the <see cref="SideMenuState"/>.
		/// </summary>
		public SideMenuState State { get; }
	}
}
