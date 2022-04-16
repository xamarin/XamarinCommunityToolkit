using System;using Microsoft.Extensions.Logging;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Core
{
	[System.ComponentModel.TypeConverter(typeof(FileMediaSourceConverter))]
	public sealed class FileMediaSource : MediaSource
	{
		public static readonly BindableProperty FileProperty
			= BindableProperty.Create(nameof(File), typeof(string), typeof(FileMediaSource), propertyChanged: OnFileMediaSourceChanged);

		public string? File
		{
			get => (string?)GetValue(FileProperty);
			set => SetValue(FileProperty, value);
		}

		public override string ToString() => $"File: {File}";

		public static implicit operator FileMediaSource(string file) => (FileMediaSource)FromFile(file);

		public static implicit operator string?(FileMediaSource? file) => file?.File;

		static void OnFileMediaSourceChanged(BindableObject bindable, object oldValue, object newValue) =>
			((FileMediaSource)bindable).OnSourceChanged();
	}
}