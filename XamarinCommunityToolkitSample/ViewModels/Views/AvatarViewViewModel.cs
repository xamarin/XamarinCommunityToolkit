namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views
{
	public class AvatarViewViewModel : BaseViewModel
	{
		public object[] Items { get; } =
		{
			new { Initials = "AM", Source = string.Empty, Name = "Andrei Misiukevich" },
			new { Initials = "DO", Source = string.Empty, Name = "David Ortinau" },
			new { Initials = "ST", Source = string.Empty, Name = "Steven Thewissen" },
			new { Initials = "GV", Source = string.Empty, Name = "Glenn Versweyveld" },
			new { Initials = "JSR", Source = string.Empty, Name = "Javier Suárez Ruiz" },
			new { Initials = "GV", Source = string.Empty, Name = "Gerald Versluis" },
			new { Initials = "XM", Source = string.Empty, Name = "Xamarin Monkey" },
			new { Initials = string.Empty, Source = string.Empty, Name = "Unknown" }
		};
	}
}