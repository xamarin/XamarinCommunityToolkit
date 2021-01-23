using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract class BaseLazyView : ContentView, IDisposable
	{
		internal static readonly BindablePropertyKey IsLoadedPropertyKey = BindableProperty.CreateReadOnly(nameof(IsLoaded), typeof(bool), typeof(BaseLazyView), default);

		public static readonly BindableProperty IsLoadedProperty = IsLoadedPropertyKey.BindableProperty;

		public bool IsLoaded => (bool)GetValue(IsLoadedProperty);

		internal void SetIsLoaded(bool isLoaded) => SetValue(IsLoadedPropertyKey, isLoaded);

		public abstract void LoadView();

		public void Dispose()
		{
			if (Content is IDisposable disposable)
				disposable.Dispose();
		}

		protected override void OnBindingContextChanged()
		{
			if (Content != null && !(Content is ActivityIndicator))
				Content.BindingContext = BindingContext;
		}
	}
}