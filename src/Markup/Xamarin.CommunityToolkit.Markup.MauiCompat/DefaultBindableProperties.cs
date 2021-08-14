using System.Reflection;
using Microsoft.Maui.Controls;

namespace Xamarin.CommunityToolkit.Markup.MauiCompat
{
	public static class DefaultBindableProperties
	{
		static readonly Dictionary<string, BindableProperty> bindableObjectTypeDefaultProperty = new()
		{ // Key: full type name of BindableObject, Value: the default BindableProperty
		  // Note that we don't specify default properties for unconstructed generic types
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(ActivityIndicator)}", ActivityIndicator.IsRunningProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(BackButtonBehavior)}", BackButtonBehavior.CommandProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(BoxView)}", BoxView.ColorProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(Button)}", Button.CommandProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(CarouselPage)}", Page.TitleProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(CheckBox)}", CheckBox.IsCheckedProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(ClickGestureRecognizer)}", ClickGestureRecognizer.CommandProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(CollectionView)}", CollectionView.ItemsSourceProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(ContentPage)}", Page.TitleProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(DatePicker)}", DatePicker.DateProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(Editor)}", Editor.TextProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(Entry)}", Entry.TextProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(EntryCell)}", EntryCell.TextProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(FileImageSource)}", FileImageSource.FileProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(FlyoutPage)}", FlyoutPage.IsPresentedProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(GraphicsView)}", GraphicsView.DrawableProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(HtmlWebViewSource)}", HtmlWebViewSource.HtmlProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(Image)}", Image.SourceProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(ImageButton)}", ImageButton.CommandProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(ImageCell)}", ImageCell.ImageSourceProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(ItemsView)}", ItemsView.ItemsSourceProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(Label)}", Label.TextProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(ListView)}", ListView.ItemsSourceProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(MenuItem)}", MenuItem.CommandProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(MultiPage<Page>)}", Page.TitleProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(NavigationPage)}", Page.TitleProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(Page)}", Page.TitleProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(Picker)}", Picker.SelectedIndexProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(ProgressBar)}", ProgressBar.ProgressProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(RadioButton)}", RadioButton.IsCheckedProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(RefreshView)}", RefreshView.CommandProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(SearchBar)}", SearchBar.SearchCommandProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(SearchHandler)}", SearchHandler.CommandProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(Slider)}", Slider.ValueProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(SolidColorBrush)}", SolidColorBrush.ColorProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(Span)}", Span.TextProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(Stepper)}", Stepper.ValueProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(StreamImageSource)}", StreamImageSource.StreamProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(SwipeGestureRecognizer)}", SwipeGestureRecognizer.CommandProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(SwipeItem)}", SwipeItem.CommandProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(Switch)}", Switch.IsToggledProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(SwitchCell)}", SwitchCell.OnProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(TabbedPage)}", Page.TitleProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(TableRoot)}", TableRoot.TitleProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(TableSection)}", TableSection.TitleProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(TableSectionBase)}", TableSectionBase.TitleProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(TapGestureRecognizer)}", TapGestureRecognizer.CommandProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(TemplatedPage)}", Page.TitleProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(TextCell)}", TextCell.TextProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(TimePicker)}", TimePicker.TimeProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(ToolbarItem)}", ToolbarItem.CommandProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(UriImageSource)}", UriImageSource.UriProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(UrlWebViewSource)}", UrlWebViewSource.UrlProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(WebView)}", WebView.SourceProperty },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(Window)}", Window.MenuProperty }
		};

		static readonly Dictionary<string, (BindableProperty, BindableProperty)> bindableObjectTypeDefaultCommandAndParameterProperties = new()
		{ // Key: full type name of BindableObject, Value: command property and corresponding commandParameter property
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(Button)}", (Button.CommandProperty, Button.CommandParameterProperty) },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(TextCell)}", (TextCell.CommandProperty, TextCell.CommandParameterProperty) },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(ClickGestureRecognizer)}", (ClickGestureRecognizer.CommandProperty, ClickGestureRecognizer.CommandParameterProperty) },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(ImageButton)}", (ImageButton.CommandProperty, ImageButton.CommandParameterProperty) },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(MenuItem)}", (MenuItem.CommandProperty, MenuItem.CommandParameterProperty) },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(RefreshView)}", (RefreshView.CommandProperty, RefreshView.CommandParameterProperty) },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(SwipeGestureRecognizer)}", (SwipeGestureRecognizer.CommandProperty, SwipeGestureRecognizer.CommandParameterProperty) },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(SwipeItemView)}", (SwipeItemView.CommandProperty, SwipeItemView.CommandParameterProperty) },
			{ $"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.{nameof(Microsoft.Maui.Controls)}.{nameof(TapGestureRecognizer)}", (TapGestureRecognizer.CommandProperty, TapGestureRecognizer.CommandParameterProperty) }
		};

		public static void Register(params BindableProperty[] properties)
		{
			foreach (var property in properties)
			{
				if (property.DeclaringType.FullName is not null)
					bindableObjectTypeDefaultProperty.Add(property.DeclaringType.FullName, property);
			}
		}

		public static void RegisterForCommand(params (BindableProperty commandProperty, BindableProperty parameterProperty)[] propertyPairs)
		{
			foreach (var propertyPair in propertyPairs)
			{
				if (propertyPair.commandProperty.DeclaringType.FullName is not null)
					bindableObjectTypeDefaultCommandAndParameterProperties.Add(propertyPair.commandProperty.DeclaringType.FullName, propertyPair);
			}
		}

		internal static void Unregister(BindableProperty property)
		{
			if (property.DeclaringType.FullName is null)
				throw new InvalidOperationException($"{nameof(BindableProperty)}.{nameof(BindableProperty.DeclaringType)}.{nameof(BindableProperty.DeclaringType.FullName)} cannot be null");

			bindableObjectTypeDefaultProperty.Remove(property.DeclaringType.FullName);
		}

		internal static BindableProperty GetFor(BindableObject bindableObject)
		{
			var type = bindableObject.GetType();
			var defaultProperty = GetFor(type);

			if (defaultProperty is null)
			{
				throw new ArgumentException(
					"No default bindable property is registered for BindableObject type " + type.FullName +
					"\r\nEither specify a property when calling Bind() or register a default bindable property for this BindableObject type");
			}

			return defaultProperty;
		}

		internal static BindableProperty? GetFor(Type? bindableObjectType)
		{
			BindableProperty? defaultProperty = null;

			do
			{
				var bindableObjectTypeName = bindableObjectType?.FullName;
				if (bindableObjectTypeName is not null && bindableObjectTypeDefaultProperty.TryGetValue(bindableObjectTypeName, out defaultProperty))
				{
					break;
				}

				if (bindableObjectTypeName?.StartsWith($"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.", StringComparison.Ordinal) is true)
				{
					break;
				}

				bindableObjectType = bindableObjectType?.GetTypeInfo().BaseType;
			}
			while (bindableObjectType != null);

			return defaultProperty;
		}

		internal static void UnregisterForCommand(BindableProperty commandProperty)
		{
			if (commandProperty.DeclaringType.FullName is null)
				throw new InvalidOperationException($"{nameof(BindableProperty)}.{nameof(BindableProperty.DeclaringType)}.{nameof(BindableProperty.DeclaringType.FullName)} cannot be null");

			bindableObjectTypeDefaultCommandAndParameterProperties.Remove(commandProperty.DeclaringType.FullName);
		}

		internal static (BindableProperty, BindableProperty) GetForCommand(BindableObject bindableObject)
		{
			var type = bindableObject.GetType();
			(var commandProperty, var parameterProperty) = GetForCommand(type);
			if (commandProperty is null || parameterProperty is null)
			{
				throw new ArgumentException(
					"No command + command parameter properties are registered for BindableObject type " + type.FullName +
					"\r\nRegister command + command parameter properties for this BindableObject type");
			}

			return (commandProperty, parameterProperty);
		}

		internal static (BindableProperty?, BindableProperty?) GetForCommand(Type? bindableObjectType)
		{
			(BindableProperty?, BindableProperty?) commandAndParameterProperties = (null, null);

			do
			{
				var bindableObjectTypeName = bindableObjectType?.FullName;
				if (bindableObjectTypeName is not null && bindableObjectTypeDefaultCommandAndParameterProperties.TryGetValue(bindableObjectTypeName, out var dictionaryResult))
				{
					commandAndParameterProperties.Item1 = dictionaryResult.Item1;
					commandAndParameterProperties.Item2 = dictionaryResult.Item2;
					break;
				}

				if (bindableObjectTypeName?.StartsWith($"{nameof(Microsoft)}.{nameof(Microsoft.Maui)}.", StringComparison.Ordinal) is true)
				{
					break;
				}

				bindableObjectType = bindableObjectType?.GetTypeInfo().BaseType;
			}
			while (bindableObjectType != null);

			return commandAndParameterProperties;
		}
	}
}
