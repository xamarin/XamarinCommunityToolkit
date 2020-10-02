using System.Linq;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Effects
{
	/// <summary>
	/// Manages adding a <see cref="FormattedString"/> to an existing
	/// input control such as <see cref="Entry"/>.
	/// </summary>
	public static class FormattedTextEffect
	{
		public static readonly BindableProperty FormattedTextProperty = BindableProperty.CreateAttached(
			"FormattedText", typeof(FormattedString), typeof(FormattedTextEffect), null, propertyChanged: OnPropertyChanged);

		/// <summary>
		/// Gets the value of the "FormattedText".
		/// </summary>
		/// <param name="view">
		/// The element to retrieve the property from.
		/// </param>
		/// <returns>
		/// The <see cref="FormattedString"/> property from the <see cref="BindableObject"/>.
		/// </returns>
		public static FormattedString GetFormattedText(BindableObject view) =>
			(FormattedString)view.GetValue(FormattedTextProperty);

		/// <summary>
		/// Sets the value of the "FormattedText".
		/// </summary>
		/// <param name="view">
		/// The element on which to set the "FormattedText".
		/// </param>
		/// <param name="value">
		/// The new <see cref="FormattedString"/> value.
		/// </param>
		public static void SetFormattedText(BindableObject view, FormattedString value) =>
			view.SetValue(FormattedTextProperty, value);

		/// <summary>
		/// Automates attaching the Xamarin.Forms Effect to the control. This
		/// method automatically clears out any existing <see cref="EntryFormattedText"/>
		/// as there should only ever be 1.
		/// </summary>
		/// <param name="bindable">The bindable object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is View view && newValue != oldValue)
			{
				var effectsToRemove = view.Effects.OfType<EntryFormattedTextEffect>().ToArray();
				for (var index = 0; index < effectsToRemove.Length; index++)
				{
					view.Effects.Remove(effectsToRemove[index]);
				}

				view.Effects.Add(new EntryFormattedTextEffect());
			}
		}

		class EntryFormattedTextEffect : RoutingEffect
		{
			public EntryFormattedTextEffect()
				: base($"{nameof(Xamarin.CommunityToolkit)}.{nameof(EntryFormattedTextEffect)}")
			{
			}
		}
	}
}
