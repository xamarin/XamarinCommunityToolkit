using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace FormsCommunityToolkit.Effects.UWP
{
    static class VisualTreeExtensions
    {
        public static T GetChildOfType<T>(this DependencyObject obj) where T : DependencyObject
        {
            if (obj == null)
                return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);

                var result = (child as T) ?? GetChildOfType<T>(child);
                if (result != null)
                    return result;
            }
            return null;
        }

        public static FrameworkElement GetChildByName(this FrameworkElement obj, string name)
        {
            if (obj == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i) as FrameworkElement;
                if (child == null)
                    continue;

                if (child.Name == name)
                {
                    return child;
                }
                var result = GetChildByName(child, name);
                if (result != null)
                    return result;
            }
            return null;
        }
    }
}
