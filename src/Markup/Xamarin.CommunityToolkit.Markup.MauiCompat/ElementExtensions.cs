using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Internals;
using FontElement = Microsoft.Maui.Controls.Label; // TODO: Get rid of this after we have default interface implementation in Forms for IFontElement
using PaddingElement = Microsoft.Maui.Controls.Label; // TODO: Get rid of this after we have default interface implementation in Forms for IPaddingElement

namespace Xamarin.CommunityToolkit.Markup
{
	public static class ElementExtensions
	{
		// Padding

		public static TLayout Padding<TLayout>(this TLayout paddingElement, Thickness padding) where TLayout : Element, IPaddingElement
		{ paddingElement.SetValue(PaddingElement.PaddingProperty, padding); return paddingElement; }

		public static TLayout Padding<TLayout>(this TLayout paddingElement, double horizontalSize, double verticalSize) where TLayout : Element, IPaddingElement
		{ paddingElement.SetValue(PaddingElement.PaddingProperty, new Thickness(horizontalSize, verticalSize)); return paddingElement; }

		public static TLayout Paddings<TLayout>(this TLayout paddingElement, double left = 0, double top = 0, double right = 0, double bottom = 0) where TLayout : Element, IPaddingElement
		{ paddingElement.SetValue(PaddingElement.PaddingProperty, new Thickness(left, top, right, bottom)); return paddingElement; }

		// DynamicResource

		public static TElement DynamicResource<TElement>(this TElement element, BindableProperty property, string key) where TElement : Element
		{ element.SetDynamicResource(property, key); return element; }

		public static TElement DynamicResources<TElement>(this TElement element, params (BindableProperty property, string key)[] resources) where TElement : Element
		{
			foreach (var resource in resources)
				element.SetDynamicResource(resource.property, resource.key);
			return element;
		}

		public static TElement RemoveDynamicResources<TElement>(this TElement element, params BindableProperty[] properties) where TElement : Element
		{
			foreach (var property in properties)
				element.RemoveDynamicResource(property);
			return element;
		}

		// Effects

		public static TElement Effects<TElement>(this TElement element, params Effect[] effects) where TElement : Element
		{
			for (var i = 0; i < effects.Length; i++)
				element.Effects.Add(effects[i]);
			return element;
		}

		// Font

		public static TFontElement FontSize<TFontElement>(this TFontElement fontElement, double size) where TFontElement : Element, IFontElement
		{ fontElement.SetValue(FontElement.FontSizeProperty, size); return fontElement; }

		public static TFontElement Bold<TFontElement>(this TFontElement fontElement) where TFontElement : Element, IFontElement
		{ fontElement.SetValue(FontElement.FontAttributesProperty, FontAttributes.Bold); return fontElement; }

		public static TFontElement Italic<TFontElement>(this TFontElement fontElement) where TFontElement : Element, IFontElement
		{ fontElement.SetValue(FontElement.FontAttributesProperty, FontAttributes.Italic); return fontElement; }

		public static TFontElement Font<TFontElement>(
			this TFontElement fontElement,
			string? family = null,
			double? size = null,
			bool? bold = null,
			bool? italic = null) where TFontElement : Element, IFontElement
		{
			if (family != null)
				fontElement.SetValue(FontElement.FontFamilyProperty, family);

			if (size.HasValue)
				fontElement.SetValue(FontElement.FontSizeProperty, size.Value);

			if (bold.HasValue || italic.HasValue)
			{
				var attributes = FontAttributes.None;
				if (bold == true)
					attributes |= FontAttributes.Bold;
				if (italic == true)
					attributes |= FontAttributes.Italic;
				fontElement.SetValue(FontElement.FontAttributesProperty, attributes);
			}

			return fontElement;
		}
	}
}