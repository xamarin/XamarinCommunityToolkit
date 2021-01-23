using System;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Behaviors
{
	public class MaxLengthReachedBehaviorViewModel : BaseViewModel
	{
		string commandExecutions;

		public string CommandExecutions
		{
			get => commandExecutions;
			set => SetProperty(ref commandExecutions, value);
		}

		public ICommand MaxLengthReachedCommand { get; }

		public MaxLengthReachedBehaviorViewModel()
			=> MaxLengthReachedCommand = CommandHelper.Create<string>(OnCommandExecuted);

		void OnCommandExecuted(string text)
			=> CommandExecutions += string.Format("MaxLength reached with value: '{0}'.", text) + Environment.NewLine;
	}
}