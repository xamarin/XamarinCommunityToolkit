using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinCommunityToolkitSample.ViewModels;

namespace XamarinCommunityToolkitSample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BehaviorsPage : ContentPage
    {
        public BehaviorsPage()
        {
            InitializeComponent();
            BindingContext = BehaviorsPageViewModel.Instance;
        }
    }
}