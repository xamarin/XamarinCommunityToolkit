
namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views
{
    public class AvatarViewViewModel : BaseViewModel
    {
        public object[] Items { get; } =
        {
            new { Initials = "AM", Source = "", Name = "Andrei Misiukevich" },
            new { Initials = "DO", Source = "https://picsum.photos/500/500?image=472", Name = "David Ortinau" },
            new { Initials = "ST", Source = "https://picsum.photos/500/500?image=473", Name = "Steven Thewissen" },
            new { Initials = "GV", Source = "", Name = "Glenn Versweyveld" },
            new { Initials = "JSR", Source = "", Name = "Javier Suárez Ruiz" },
            new { Initials = "GV", Source = "https://picsum.photos/500/500?image=474", Name = "Gerald Versluis" },
            new { Initials = "XM", Source = "https://picsum.photos/500/500?image=475", Name = "Xamarin Monkey" },
            new { Initials = "", Source = "", Name = "Unknown" }
        };
    }
}