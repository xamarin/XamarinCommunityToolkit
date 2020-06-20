
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinCommunityToolkitSample.ViewModels.Behaviors
{
    public class EventToCommandBehaviorViewModel : BaseViewModel
    {
        ICommand command;

        int clickCount;

        public ICommand Command => command ??= new Command(() => ClickCount++);

        public int ClickCount
        {
            get => clickCount;
            set => Set(ref clickCount, value);
        }
    }
}
