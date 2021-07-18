using System;
using System.Collections.Generic;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Represents a parameter to be used in the <see cref="EnumToObjectConverter"/>.
	/// </summary>
	public class EnumToObjectConverterParameter : List<Enum>
	{
		/// <summary>
		/// The object of this parameter.
		/// </summary>
		public object? Object { get; set; }

		/// <summary>
		/// The enum of this parameter.
		/// </summary>
		public object? Enum { get; set; }
	}
}