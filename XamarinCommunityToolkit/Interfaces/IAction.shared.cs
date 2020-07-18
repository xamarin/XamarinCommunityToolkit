using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace Microsoft.Toolkit.Xamarin.Forms.Interfaces
{
    [Preserve(AllMembers = true)]
    public interface IAction
    {
        Task<bool> Execute(object sender, object parameter);
    }
}
