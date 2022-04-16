﻿using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;using Microsoft.Maui.Layouts;using FlexLayout = Microsoft.Maui.Controls.FlexLayout;

namespace Xamarin.CommunityToolkit.Markup
{
	public static class ViewInFlexLayoutExtensions
	{
		public static TView AlignSelf<TView>(this TView view, FlexAlignSelf value) where TView : View
		{
			FlexLayout.SetAlignSelf(view, value);
			return view;
		}

		public static TView Basis<TView>(this TView view, FlexBasis value) where TView : View
		{
			FlexLayout.SetBasis(view, value);
			return view;
		}

		public static TView Grow<TView>(this TView view, float value) where TView : View
		{
			FlexLayout.SetGrow(view, value);
			return view;
		}

		public static TView Order<TView>(this TView view, int value) where TView : View
		{
			FlexLayout.SetOrder(view, value);
			return view;
		}

		public static TView Shrink<TView>(this TView view, float value) where TView : View
		{
			FlexLayout.SetShrink(view, value);
			return view;
		}
	}
}