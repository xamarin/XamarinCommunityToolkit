using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinCommunityToolkitSample.ViewModels
{
    public class BehaviorsPageViewModel : BaseViewModel
    {
        private static readonly Lazy<BehaviorsPageViewModel> lazy = new Lazy<BehaviorsPageViewModel>(() => new BehaviorsPageViewModel());
        public static BehaviorsPageViewModel Instance => lazy.Value;

        private BehaviorsPageViewModel()
        {

        }
    }
}
