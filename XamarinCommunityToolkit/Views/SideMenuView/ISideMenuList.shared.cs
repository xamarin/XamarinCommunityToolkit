using System.Collections.Generic;
using Xamarin.Forms;

namespace Microsoft.Toolkit.Xamarin.Forms.UI.Views
{
	public interface ISideMenuList<T> : IList<T> where T : View
	{
		void Add(View view, SideMenuPosition position);

		void Add(View view, SideMenuPosition position, double menuWidthPercentage);
	}
}