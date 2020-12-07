using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
    /// <summary>
    /// The DockLayout makes it easy to dock content in all four directions (top, bottom, left and right).
    /// This makes it a great choice in many situations, where you want to divide the screen into specific areas,
    /// especially because by default, the last element inside the DockLayout, unless this feature is specifically disabled,
    /// will automatically fill the rest of the space (center).
    /// Inspired by WPF DockPanel: https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.dockpanel?view=netframework-4.8
    /// </summary>
    public class DockLayout : Layout<View>
    {
        public static readonly BindableProperty DockProperty =
            BindableProperty.CreateAttached(nameof(Dock), typeof(Dock), typeof(DockLayout), Dock.Left);

        public Dock Dock
        {
            get => (Dock)GetValue(DockProperty);
            set => SetValue(DockProperty, value);
        }

        public static Dock GetDock(BindableObject bindable)
            => (Dock)bindable.GetValue(DockProperty);

        public static void SetDock(BindableObject bindable, Dock value)
            => bindable.SetValue(DockProperty, value);

        public static readonly BindableProperty LastChildFillProperty =
           BindableProperty.Create(nameof(LastChildFill), typeof(bool), typeof(DockLayout), true,
               BindingMode.TwoWay, null);

        /// <summary>
        /// The default behavior is that the last child of the DockLayout takes up the rest of the space,
        /// but this can be disabled using the LastChildFill.
        /// </summary>
        public bool LastChildFill
        {
            get => (bool)GetValue(LastChildFillProperty);
            set => SetValue(LastChildFillProperty, value);
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            var i = 0;

            foreach (var child in Children)
            {
                if (child.IsVisible)
                {
                    i++;

                    var sizeRequest = child.Measure(width, height, MeasureFlags.IncludeMargins);

                    double childX = 0;
                    double childY = 0;
                    var request = sizeRequest.Request;
                    var childWidth = Math.Min(width, request.Width);
                    var childHeight = Math.Min(height, request.Height);

                    var lastItem = i == Children.Count;
                    if (lastItem & LastChildFill)
                    {
                        LayoutChildIntoBoundingRegion(child, new Rectangle(x, y, width, height));
                        return;
                    }

                    switch (GetDock(child))
                    {
                        default:
                        case Dock.Left:
                            {
                                childX = x;
                                childY = y;
                                childHeight = height;
                                x += childWidth;
                                width -= childWidth;
                                break;
                            }
                        case Dock.Top:
                            {
                                childX = x;
                                childY = y;
                                childWidth = width;
                                y += childHeight;
                                height -= childHeight;
                                break;
                            }
                        case Dock.Right:
                            {
                                childX = x + width - childWidth;
                                childY = y;
                                childHeight = height;
                                width -= childWidth;
                                break;
                            }
                        case Dock.Bottom:
                            {
                                childX = x;
                                childY = y + height - childHeight;
                                childWidth = width;
                                height -= childHeight;
                                break;
                            }
                    }

                    LayoutChildIntoBoundingRegion(child, new Rectangle(childX, childY, childWidth, childHeight));
                }
            }
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            double height = 0;
            double width = 0;
            double finalWidth = 0;
            double finalHeight = 0;

            foreach (var child in Children)
            {
                if (child.IsVisible)
                {
                    var sizeRequest = child.Measure(widthConstraint, heightConstraint, MeasureFlags.IncludeMargins);
                    var request = sizeRequest.Request;

                    switch (GetDock(child))
                    {
                        default:
                        case Dock.Left:
                        case Dock.Right:
                            {
                                width += request.Width;
                                finalWidth = Math.Max(finalWidth, width);
                                finalHeight = Math.Max(finalHeight, height + request.Height);
                                break;
                            }
                        case Dock.Top:
                        case Dock.Bottom:
                            {
                                height += request.Height;
                                finalWidth = Math.Max(finalWidth, width + request.Width);
                                finalHeight = Math.Max(finalHeight, height);
                                break;
                            }
                    }
                }
            }

            return new SizeRequest(new Size(finalWidth, finalHeight));
        }
    }
}
