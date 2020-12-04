using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	/// <summary>
	/// Default popup implementation that uses a
	/// generic <see cref="object"/> result.
	/// </summary>
	public class Popup : Popup<object>
	{
		/// <summary>
		/// Initalizes a default implementation of <see cref="Popup"/>.
		/// </summary>
		public Popup() { }

		/// <summary>
		/// Initializes the <see cref="Popup"/> with a <see cref="View"/>.
		/// </summary>
		/// <param name="view">
		/// The <see cref="View"/> to render in the <see cref="Popup"/>.
		/// </param>
		public Popup(View view)
		{
			View = view;
		}
	}
}
