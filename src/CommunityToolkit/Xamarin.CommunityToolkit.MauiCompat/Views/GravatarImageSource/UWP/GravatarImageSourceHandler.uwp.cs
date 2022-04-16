﻿using System;using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Maui.Controls.Compatibility.Platform.UWP;
using FormsImageSource = Microsoft.Maui.Controls.ImageSource;
using WindowsImageSource = Windows.UI.Xaml.Media.ImageSource;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public partial class GravatarImageSourceHandler : IImageSourceHandler
	{
		public async Task<WindowsImageSource?> LoadImageAsync(FormsImageSource imagesource, CancellationToken cancellationToken = default)
		{
			var fileInfo = await LoadInternal(imagesource, 1, ApplicationData.Current.LocalCacheFolder.Path);

			BitmapImage? bitmap = null;
			try
			{
				await semaphore.WaitAsync();

				if (fileInfo?.Exists ?? false)
					bitmap = new BitmapImage(new Uri(fileInfo.FullName));
			}
			finally
			{
				semaphore.Release();
			}

			return bitmap;
		}
	}
}