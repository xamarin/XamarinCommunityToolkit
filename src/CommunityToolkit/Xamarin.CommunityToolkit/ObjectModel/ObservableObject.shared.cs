using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.CommunityToolkit.Helpers;

#nullable enable

namespace Xamarin.CommunityToolkit.ObjectModel
{
	/// <summary>
	/// Observable object with INotifyPropertyChanged implemented using DelegateWeakEventManager
	/// </summary>
	public abstract class ObservableObject : PersistentObservableObject
	{
		readonly DelegateWeakEventManager weakEventManager = new DelegateWeakEventManager();

		public override event PropertyChangedEventHandler? PropertyChanged
		{
			add => weakEventManager.AddEventHandler(value);
			remove => weakEventManager.RemoveEventHandler(value);
		}

		protected override void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
			weakEventManager.RaiseEvent(this, new PropertyChangedEventArgs(propertyName), nameof(PropertyChanged));
	}
}