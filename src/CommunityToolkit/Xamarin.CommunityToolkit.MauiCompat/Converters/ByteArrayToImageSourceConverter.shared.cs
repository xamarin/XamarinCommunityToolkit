using System;using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts the incoming value from <see cref="byte"/>[] and returns the object of a type <see cref="ImageSource"/> or vice versa.
	/// </summary>
	public class ByteArrayToImageSourceConverter : BaseNullableConverter<byte[], ImageSource?>
	{
		/// <summary>
		/// Converts the incoming value from <see cref="StreamImageSource"/> and returns a <see cref="byte"/>[].
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>An object of type <see cref="ImageSource"/>.</returns>
		public override byte[]? ConvertBackTo(ImageSource? value)
		{
			if (value == null)
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

		/// <summary>
		/// Converts the incoming value from <see cref="byte"/>[] and returns the object of a type <see cref="ImageSource"/>.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>An object of type <see cref="ImageSource"/>.</returns>
		public override ImageSource? ConvertFrom(byte[]? value)
		{
			if (value == null)
				return null;

			return ImageSource.FromStream(() => new MemoryStream(value));
		}
	}
}