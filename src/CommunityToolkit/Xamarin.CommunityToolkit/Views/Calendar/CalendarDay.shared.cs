using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	/// <summary>
	/// The <see cref="CalendarDay"/> represents specific day in <see cref="Calendar"/>.
	/// </summary>
	public class CalendarDay : TemplatedView
	{
		/// <summary>
		/// Backing BindableProperty for the <see cref="IsSelectable"/> property.
		/// </summary>
		public static readonly BindableProperty IsSelectableProperty =
			BindableProperty.Create(nameof(IsSelectable), typeof(bool), typeof(CalendarDay), defaultValue: true);

		/// <summary>
		/// Determines if day can be selected.
		/// </summary>
		public bool IsSelectable
		{
			get => (bool)GetValue(IsSelectableProperty);
			set => SetValue(IsSelectableProperty, value);
		}

		/// <summary>
		/// Backing BindableProperty for the <see cref="IsSelected"/> property.
		/// </summary>
		public static readonly BindableProperty IsSelectedProperty =
			BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(CalendarDay));

		/// <summary>
		/// Gets or sets if day is selected.
		/// </summary>
		public bool IsSelected
		{
			get => (bool)GetValue(IsSelectedProperty);
			set => SetValue(IsSelectedProperty, value);
		}

		/// <summary>
		/// Backing BindableProperty for the <see cref="Date"/> property.
		/// </summary>
		public static readonly BindableProperty DateProperty =
			BindableProperty.Create(nameof(Date), typeof(DateTimeOffset), typeof(CalendarDay), default(DateTimeOffset));

		/// <summary>
		/// Gets or sets date for day.
		/// </summary>
		public DateTimeOffset Date
		{
			get => (DateTimeOffset)GetValue(DateProperty);
			set => SetValue(DateProperty, value);
		}
	}
}