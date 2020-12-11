using System;
using System.Globalization;
using System.IO;
using System.Threading;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts the incoming value from byte array and returns the object of a type ImageSource.
	/// </summary>
	public class ByteArrayToImageSourceConverter : ValueConverterExtension, IValueConverter
	{
		/// <summary>
		/// Converts the incoming value from byte array and returns the object of a type ImageSource.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">Additional parameter for the converter to handle. Not implemented.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>An object of type ImageSource.</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is null)
				return null;

			if (value is byte[] imageBytes)
				return ImageSource.FromStream(() => new MemoryStream(imageBytes));

			throw new ArgumentException("Expected value to be of type byte[].", nameof(value));
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is null)
				return null;

			if (value is StreamImageSource streamImageSource)
			{
				var streamFromImageSource = streamImageSource.Stream(CancellationToken.None).Result;

				if (streamFromImageSource == null)
					return null;

				using var memoryStream = new MemoryStream();
				streamFromImageSource.CopyTo(memoryStream);

				return memoryStream.ToArray();
			}

			throw new ArgumentException("Expected value to be of type StreamImageSource.", nameof(value));
		}
	}
}