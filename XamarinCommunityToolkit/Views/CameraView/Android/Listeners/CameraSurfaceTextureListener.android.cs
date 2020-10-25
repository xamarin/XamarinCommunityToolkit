using System;
using System.Collections.Generic;
using System.Text;
using Android.Graphics;
using Android.Views;
using Xamarin.CommunityToolkit.UI.Views;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class CameraSurfaceTextureListener : Java.Lang.Object, TextureView.ISurfaceTextureListener
	{
		readonly CameraDroid owner;

		public CameraSurfaceTextureListener(CameraDroid cameraFragment) =>
			owner = cameraFragment ?? throw new ArgumentNullException(nameof(cameraFragment));

		public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
		{
			owner.OpenCamera(width, height);
		}

		public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
		{
			return true;
		}

		public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
		{
			owner.ConfigureTransform(width, height);
		}

		public void OnSurfaceTextureUpdated(SurfaceTexture surface)
		{
		}
	}
}
