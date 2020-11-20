using System.ComponentModel;
using Xamarin.Essentials;

namespace Xamarin.CommunityToolkit.MarkupSample
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
				MainThread.BeginInvokeOnMainThread(() => handler(this, new PropertyChangedEventArgs(propertyName)));
        }
    }
}