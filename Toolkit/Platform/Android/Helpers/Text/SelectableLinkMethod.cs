using System.Linq;
using Android.Text;
using Android.Text.Method;
using Android.Text.Style;
using Android.Views;
using Android.Widget;

namespace Xamarin.Toolkit.Droid.Helpers.Text
{
    // Based on: https://stackoverflow.com/a/23566268/5001796
    public class SelectableLinkMethod : ArrowKeyMovementMethod
    {
        private static SelectableLinkMethod instance;

        public static new SelectableLinkMethod Instance => instance ?? (instance = new SelectableLinkMethod());

        private SelectableLinkMethod()
        {
        }

        public override bool OnTouchEvent(TextView widget, ISpannable buffer, MotionEvent e)
        {
            if (e.Action == MotionEventActions.Up ||
                e.Action == MotionEventActions.Down)
            {
                var x = (int)e.GetX();
                var y = (int)e.GetY();

                x -= widget.TotalPaddingLeft;
                y -= widget.TotalPaddingTop;

                x += widget.ScrollX;
                y += widget.ScrollY;

                var layout = widget.Layout;
                var line = layout.GetLineForVertical(y);
                var off = layout.GetOffsetForHorizontal(line, x);

                var link = buffer
                    .GetSpans(off, off, Java.Lang.Class.FromType(typeof(ClickableSpan)))
                    .OfType<ClickableSpan>()
                    .ToArray();

                if (link.Length != 0)
                {
                    if (e.Action == MotionEventActions.Up)
                    {
                        link[0].OnClick(widget);
                    }
                    else if (e.Action == MotionEventActions.Down)
                    {
                        Selection.SetSelection(buffer, buffer.GetSpanStart(link[0]), buffer.GetSpanEnd(link[0]));
                    }

                    return true;
                }
                else
                {
                    Selection.RemoveSelection(buffer);
                }
            }

            return base.OnTouchEvent(widget, buffer, e);
        }
    }
}
