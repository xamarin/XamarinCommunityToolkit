using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views.Calendar
{
	public partial class CalendarPage
    {
        public CalendarPage()
        {
            InitializeComponent();

            PickerFirstDayOfWeek.ItemsSource = Enum.GetValues(typeof(DayOfWeek));
            PickerFirstDayOfWeek.SelectedItem = Calendar.FirstDayOfWeek;

            PickerSelectionMode.ItemsSource = Enum.GetValues(typeof(CalendarSelectionMode));
            PickerSelectionMode.SelectedItem = Calendar.SelectionMode;
        }

        void OnCalendarDayUpdated(object sender, CalendarDayUpdatedEventArgs e)
        {
            e.CalendarDay.ControlTemplate = Resources["CalendarDayControlTemplate"] as ControlTemplate;

            if (e.CalendarDay.Date.DayOfWeek == DayOfWeek.Saturday ||
                e.CalendarDay.Date.DayOfWeek == DayOfWeek.Sunday)
            {
                e.CalendarDay.ControlTemplate = Resources["CalendarDayControlTemplateWeekend"] as ControlTemplate;
            }

            if (e.CalendarDay.Date.Day == 3 ||
                e.CalendarDay.Date.Day == 10)
            {
                e.CalendarDay.IsSelectable = false;
            }
            else
            {
                e.CalendarDay.IsSelectable = true;
            }

            if (e.CalendarDay.Date.Day == 5 ||
                e.CalendarDay.Date.Day == 16)
            {
                var day = e.CalendarDay.BindingContext as Day;

                if (day == null)
                {
                    day = new Day
                    {
                        HasAppointments = true
                    };

                    e.CalendarDay.BindingContext = day;
                }

                day.HasAppointments = true;
            }
            else
            {
                var day = e.CalendarDay.BindingContext as Day;

                if (day != null)
                {
                    day.HasAppointments = false;
                }
            }

            if (e.CalendarDay.Date.Month != Calendar.Date.Month)
            {
                e.CalendarDay.IsSelectable = false;
                e.CalendarDay.ControlTemplate =
                    Resources["CalendarDayFromOtherMonthControlTemplate"] as ControlTemplate;
            }
        }

        void OnButtonPreviousMonthClicked(object sender, EventArgs e)
        {
            Calendar.Date = Calendar.Date.AddMonths(-1);
        }

        void OnButtonNextMonthClicked(object sender, EventArgs e)
        {
            Calendar.Date = Calendar.Date.AddMonths(1);
        }

        async void OnCalendarDayTapped(object sender, CalendarDayTappedEventArgs e)
        {
            // await DisplayAlert(title: "", message: "You clicked on: " + e.CalendarDay.Date, cancel: "ok");
        }

        void OnCheckBoxShowWeekendsCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (CheckBoxShowWeekends.IsChecked)
            {
                PickerFirstDayOfWeek.ItemsSource = Enum.GetValues(typeof(DayOfWeek));
            }
            else
            {
                var values = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToList();
                values.Remove(DayOfWeek.Saturday);
                values.Remove(DayOfWeek.Sunday);

                PickerFirstDayOfWeek.ItemsSource = values;
            }

            PickerFirstDayOfWeek.SelectedItem = Calendar.FirstDayOfWeek;
        }
    }

    public class Day : INotifyPropertyChanged
    {
        bool _hasAppointments;
        public bool HasAppointments
        {
            get => _hasAppointments;
            set
            {
                _hasAppointments = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}