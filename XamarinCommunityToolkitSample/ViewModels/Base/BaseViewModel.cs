using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.CommunityToolkit.Helpers;

namespace Xamarin.CommunityToolkit.Sample.ViewModels
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		readonly WeakEventManager propertyChangedEventManager = new WeakEventManager();

		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add => propertyChangedEventManager.AddEventHandler(value);
			remove => propertyChangedEventManager.RemoveEventHandler(value);
		}

		protected bool Set<T>(ref T backingStore, T value, [CallerMemberName] string name = null)
		{
			if (EqualityComparer<T>.Default.Equals(backingStore, value))
				return false;

			backingStore = value;
			OnPropertyChanged(name);
			return true;
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
			=> propertyChangedEventManager.RaiseEvent(this, new PropertyChangedEventArgs(name), nameof(INotifyPropertyChanged.PropertyChanged));
	}
}