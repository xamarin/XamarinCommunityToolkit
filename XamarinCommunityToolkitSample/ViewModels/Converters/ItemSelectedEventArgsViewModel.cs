using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Resx;
using Xamarin.Forms;

namespace Microsoft.Toolkit.Xamarin.Forms.Sample.ViewModels.Converters
{
    public class ItemSelectedEventArgsViewModel
    {
        public IEnumerable<Person> Items { get; } =
            new List<Person>()
            {
                new Person() { Id = 1, Name = "Person 1" },
                new Person() { Id = 2, Name = "Person 2" },
                new Person() { Id = 3, Name = "Person 3" }
            };

        public ICommand ItemSelectedCommand { get; private set; } = new Command<Person>(async (person)
            => await Application.Current.MainPage.DisplayAlert($"{AppResources.ItemTapped}: ", person.Name, AppResources.Cancel));
    }
}