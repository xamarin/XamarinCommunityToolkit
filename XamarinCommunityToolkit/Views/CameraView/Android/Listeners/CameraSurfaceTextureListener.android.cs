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
		readonly CameraFragment owner;

		public CameraSurfaceTextureListener(CameraFragment cameraFragment) =>
			owner = cameraFragment ?? throw new ArgumentNullException(nameof(cameraFragment));

		public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
		{

		}

		public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
		{
			return true;
		}

		public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
		{
		}

		public void OnSurfaceTextureUpdated(SurfaceTexture surface)
		{
		}
	}
}
