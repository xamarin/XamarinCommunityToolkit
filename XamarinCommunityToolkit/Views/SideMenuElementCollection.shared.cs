using System.Collections.ObjectModel;
using Xamarin.Forms;
using static Microsoft.Toolkit.Xamarin.Forms.UI.Views.SideMenuView;

namespace Microsoft.Toolkit.Xamarin.Forms.UI.Views
{
    sealed class SideMenuElementCollection : ObservableCollection<View>, ISideMenuList<View>
    {
        public void Add(View view, SideMenuPosition position)
        {
            SetPosition(view, position);
            Add(view);
        }

        public void Add(View view, SideMenuPosition position, double menuWidthPercentage)
        {
            SetMenuWidthPercentage(view, menuWidthPercentage);
            Add(view, position);
        }
    }
}