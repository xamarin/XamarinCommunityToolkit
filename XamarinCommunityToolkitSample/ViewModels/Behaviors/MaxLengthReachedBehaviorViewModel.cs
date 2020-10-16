using System;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Sample.Resx;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Behaviors
{
	public class MaxLengthReachedBehaviorViewModel : BaseViewModel
	{
		#region Properties

		string commandExecutions;

		public string CommandExecutions
		{
			get => commandExecutions;
			set => SetProperty(ref commandExecutions, value);
		}

		public ICommand MaxLengthReachedCommand { get; }

		#endregion Properties

		public MaxLengthReachedBehaviorViewModel()
			=> MaxLengthReachedCommand = new Command<string>(OnCommandExecuted);

		void OnCommandExecuted(string text)
			=> CommandExecutions += string.Format(AppResources.MaxLengthReachedBehaviorCommandExecutionLabelFormat, text) + Environment.NewLine;
	}
}