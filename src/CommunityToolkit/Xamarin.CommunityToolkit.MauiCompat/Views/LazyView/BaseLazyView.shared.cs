using System;using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Internals;

namespace Xamarin.CommunityToolkit.UI.Views
{
	/// <summary>
	/// Abstract base class for <see cref="LazyView{TView}"/>
	/// </summary>
	[Microsoft.Maui.Controls.Internals.Preserve(Conditional = true)]
	public abstract class BaseLazyView : ContentView, IDisposable
	{
		internal static readonly BindablePropertyKey IsLoadedPropertyKey = BindableProperty.CreateReadOnly(nameof(IsLoaded), typeof(bool), typeof(BaseLazyView), default);

		/// <summary>
		/// This is a read-only <see cref="BindableProperty"/> that indicates when the view is loaded.
		/// </summary>
		public static readonly BindableProperty IsLoadedProperty = IsLoadedPropertyKey.BindableProperty;

		/// <summary>
		/// This is a read-only property that indicates when the view is loaded.
		/// </summary>
		public bool IsLoaded => (bool)GetValue(IsLoadedProperty);

		/// <summary>
		/// This method change the value of the <see cref="IsLoaded"/> property.
		/// </summary>
		/// <param name="isLoaded"></param>
		protected void SetIsLoaded(bool isLoaded) => SetValue(IsLoadedPropertyKey, isLoaded);

		/// <summary>
		/// Use this method to do the initialization of the <see cref="View"/> and change the status IsLoaded value here.
		/// </summary>
		/// <returns><see cref="ValueTask"/></returns>
		public abstract ValueTask LoadViewAsync();

		/// <summary>
		/// This method dispose the <see cref="ContentView.Content"/> if it's <see cref="IDisposable"/>.
		/// </summary>
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