using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace XamarinCommunityToolkit.Interfaces
{
    [Preserve(AllMembers = true)]
    public interface IAction
    {
        Task<bool> Execute(object sender, object parameter);
    }
}
