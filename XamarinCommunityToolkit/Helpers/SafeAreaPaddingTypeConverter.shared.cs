using System;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Helpers
{
    [Xamarin.Forms.Xaml.TypeConversion(typeof(SafeAreaPadding))]
    public class SafeAreaPaddingTypeConverter : TypeConverter
    {
        public override object ConvertFromInvariantString(string value)
        {
            if (value != null)
            {
                value = value.Trim();

                if (value.Contains(","))
                {
                    // XAML based definition
                    var safeAreaPadding = value.Split(',');

                    switch (safeAreaPadding.Length)
                    {
                        case 2:
                            if (bool.TryParse(safeAreaPadding[0], out var h)
                                && bool.TryParse(safeAreaPadding[1], out var v))
                                return new SafeAreaPadding(h, v);
                            break;
                        case 4:
                            if (bool.TryParse(safeAreaPadding[0], out var l)
                                && bool.TryParse(safeAreaPadding[1], out var t)
                                && bool.TryParse(safeAreaPadding[2], out var r)
                                && bool.TryParse(safeAreaPadding[3], out var b))
                                return new SafeAreaPadding(l, t, r, b);
                            break;
                    }
                }
                else
                {
                    // Single uniform SafeAreaPadding
                    if (bool.TryParse(value, out var l))
                        return new SafeAreaPadding(l);
                }
            }

            throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(SafeAreaPadding)}");
        }
    }
}
