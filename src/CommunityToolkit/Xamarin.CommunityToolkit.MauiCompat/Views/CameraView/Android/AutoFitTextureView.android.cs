using Paint = Android.Graphics.Paint;using Path = Android.Graphics.Path;﻿using System;using Microsoft.Extensions.Logging;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class AutoFitTextureView : TextureView
	{
		int mRatioWidth = 0;
		int mRatioHeight = 0;

		public AutoFitTextureView(Context context)
			: this(context, null)
		{
		}

		public AutoFitTextureView(Context context, IAttributeSet? attrs)
			: this(context, attrs, 0)
		{
		}

		public AutoFitTextureView(Context context, IAttributeSet? attrs, int defStyle)
			: base(context, attrs, defStyle)
		{
		}

		protected AutoFitTextureView(IntPtr javaReference, JniHandleOwnership transfer)
			: base(javaReference, transfer)
		{
		}

		public void SetAspectRatio(int width, int height)
		{
			if (width == 0 || height == 0)
				throw new ArgumentException("Size cannot be negative.");
			mRatioWidth = width;
			mRatioHeight = height;
			RequestLayout();
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
			var width = MeasureSpec.GetSize(widthMeasureSpec);
			var height = MeasureSpec.GetSize(heightMeasureSpec);

			if (mRatioWidth == 0 || mRatioHeight == 0)
				SetMeasuredDimension(width, height);
			else
			{
				if (width < (float)height * mRatioWidth / mRatioHeight)
					SetMeasuredDimension(height * mRatioWidth / mRatioHeight, height);
				else
					SetMeasuredDimension(width, width * mRatioHeight / mRatioWidth);
			}
		}
	}
}