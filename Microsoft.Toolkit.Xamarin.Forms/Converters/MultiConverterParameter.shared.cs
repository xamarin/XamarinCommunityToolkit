using System;
using Xamarin.Forms;

namespace Microsoft.Toolkit.Xamarin.Forms.Converters
{
    /// <summary>
    /// Represents a parameter to be used in the MultiConverter.
    /// </summary>
    public class MultiConverterParameter : BindableObject
    {
        public Type ConverterType { get; set; }

        public object Value { get; set; }
    }
}
