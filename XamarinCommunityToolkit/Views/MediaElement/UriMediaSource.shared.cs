using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public sealed class UriMediaSource : MediaSource
	{
		public static readonly BindableProperty UriProperty = BindableProperty.Create(nameof(Uri), typeof(Uri), typeof(UriMediaSource), default(Uri),
			propertyChanged: (bindable, oldvalue, newvalue) => ((UriMediaSource)bindable).OnSourceChanged(),
			validateValue: (bindable, value) => value == null || ((Uri)value).IsAbsoluteUri);

		[TypeConverter(typeof(UriTypeConverter))]
		public Uri Uri
		{
			get => (Uri)GetValue(UriProperty);
			set => SetValue(UriProperty, value);
		}

		public override string ToString() => $"Uri: {Uri}";
	}
}