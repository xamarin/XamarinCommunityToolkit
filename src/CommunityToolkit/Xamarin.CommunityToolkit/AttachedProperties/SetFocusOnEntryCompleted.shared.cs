using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.AttachedProperties
{
	/// <summary>
	/// The <see cref="SetFocusOnEntryCompleted"/> is an attached property for entries that allows the user to specify what <see cref="VisualElement"/> should gain focus after the user completes that entry.
	/// </summary>
	public class SetFocusOnEntryCompleted
	{
		/// <summary>
		/// The <see cref="NextElementProperty"/> attached property.
		/// </summary>
		public static readonly BindableProperty NextElementProperty =
			BindableProperty.CreateAttached(
				"NextElement",
				typeof(VisualElement),
				typeof(SetFocusOnEntryCompleted),
				default(VisualElement),
				propertyChanged: OnNextElementChanged);

		/// <summary>
		/// Required <see cref="GetNextElement"/> accessor for <see cref="NextElementProperty"/> attached property.
		/// </summary>
		public static VisualElement GetNextElement(BindableObject view)
		{
			return (VisualElement)view.GetValue(NextElementProperty);
		}

		/// <summary>
		/// Required <see cref="SetNextElement"/> accessor for <see cref="NextElementProperty"/> attached property.
		/// </summary>
		public static void SetNextElement(BindableObject view, VisualElement value)
		{
			view.SetValue(NextElementProperty, value);
		}

		static void OnNextElementChanged(BindableObject bindable, object oldValue, object newValue)
		{
            var entry = bindable as Entry;

            if (entry == null)
                return;

            entry.Completed += (s, e) => GetNextElement(entry)?.Focus();
		}
	}
}
