
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinCommunityToolkitSample.ViewModels.Behaviors
{
    public class EventToCommandBehaviorViewModel : BaseViewModel
    {
        ICommand incrementCommand;

        int clickCount;

        public ICommand IncrementCommand => incrementCommand ??= new Command(() => ClickCount++);

        public int ClickCount
        {
            get => clickCount;
            set => Set(ref clickCount, value);
        }
    }
}
