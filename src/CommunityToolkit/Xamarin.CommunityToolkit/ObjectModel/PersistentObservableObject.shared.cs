using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable enable

namespace Xamarin.CommunityToolkit.ObjectModel
{
	/// <summary>
	/// Observable object with INotifyPropertyChanged implemented
	/// </summary>
	public abstract class PersistentObservableObject : ObservableObject
	{
		public override event PropertyChangedEventHandler? PropertyChanged;

		protected override void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}