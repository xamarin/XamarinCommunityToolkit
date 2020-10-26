using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.AttachedProperties
{
	public class ProgressBarAttachedPropertiesViewModel : BaseViewModel
	{
		double progress;
		ICommand setTo0Command;
		ICommand setTo50Command;
		ICommand setTo100Command;

		public double Progress
		{
			get => progress;
			set
			{
				progress = value;
				OnPropertyChanged();
			}
		}

		public ICommand SetTo0Command => setTo0Command ??= new AsyncCommand(() => SetProgress(0));

		public ICommand SetTo50Command => setTo50Command ??= new AsyncCommand(() => SetProgress(0.5));

		public ICommand SetTo100Command => setTo100Command ??= new AsyncCommand(() => SetProgress(1));

		async Task SetProgress(double
			progress)
		{
			Progress = progress;
		}
	}
}