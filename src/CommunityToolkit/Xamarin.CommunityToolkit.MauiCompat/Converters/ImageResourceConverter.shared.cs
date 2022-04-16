using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
#if NETSTANDARD1_0 || WINDOWS
using System.Reflection;
#endif

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts embedded image resource ID to it ImageSource.
	/// </summary>
	public class ImageResourceConverter : BaseNullableConverterOneWay<string, ImageSource>
	{
		/// <summary>
		/// Converts embedded image resource ID to it ImageSource.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The ImageSource related to the provided resource ID of the embedded image. If it's null it will returns null.</returns>
		public override ImageSource? ConvertFrom(string? value)
		{
			if (value == null)
				return null;

			return ImageSource.FromResource(value, Application.Current.GetType()
#if NETSTANDARD1_0 || WINDOWS
				.GetTypeInfo()
#endif
				.Assembly);
		}
	}
}