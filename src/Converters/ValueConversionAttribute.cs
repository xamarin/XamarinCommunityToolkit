using System;

namespace FormsCommunityToolkit.Converters
{
    /// <summary>
    /// Represents an attribute that allows the author of a value converter to specify the data types involved in the implementation of the converter.
    /// 
    /// The ValueConversionAttribute attribute enables designer tools, such as WPF Designer for Visual Studio or MFractor for Visual Studio for Mac, to discover value converters to assist developers in setting up bindings and provide code warnings. The attribute is not required for a value converter to work, but it is a good practice to use it.
    /// </summary>
    [AttributeUsage (AttributeTargets.Class, AllowMultiple = true)]
    public class ValueConversionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:FormsCommunityToolkit.Converters.ValueConversionAttribute"/> class.
        /// </summary>
        /// <param name="input">The expected input type for this value converter. It is recommended to use a typeof(MyType) expression to provide the type.</param>
        /// <param name="output">The expected output type for this value converter. It is recommended to use a typeof(MyType) expression to provide the type.</param>
        public ValueConversionAttribute (Type input, Type output)
        {
        }

        /// <summary>
        /// The type that can be provided as the parameter for the value converter.
        /// </summary>
        /// <value>The type of the parameter.</value>
        public Type ParameterType { get; set; }
    }
}
