using Android.Util;
using Android.Content;
using System;
using Android.Runtime;
using AndroidX.AppCompat.Widget;

namespace Xamarin.CommunityToolkit.Views
{
	public class FormsAppCompatTextView : AppCompatTextView
	{
		public FormsAppCompatTextView(Context? context)
			: base(context)
		{
		}

		[Obsolete]
		public FormsAppCompatTextView(Context? context, IAttributeSet? attrs)
			: base(context, attrs)
		{
		}

		[Obsolete]
		public FormsAppCompatTextView(Context? context, IAttributeSet attrs, int defStyleAttr)
			: base(context, attrs, defStyleAttr)
		{
		}

		[Obsolete]
		protected FormsAppCompatTextView(IntPtr javaReference, JniHandleOwnership transfer)
			: base(javaReference, transfer)
		{
		}
	}
}
