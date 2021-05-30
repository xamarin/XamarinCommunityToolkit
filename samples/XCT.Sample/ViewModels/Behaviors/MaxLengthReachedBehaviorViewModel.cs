using System;
using System.Windows.Input;
using CommunityToolkit.Maui.ObjectModel;

namespace CommunityToolkit.Maui.Sample.ViewModels.Behaviors
{
	public class MaxLengthReachedBehaviorViewModel : BaseViewModel
	{
		string commandExecutions = string.Empty;

		public string CommandExecutions
		{
			get => commandExecutions;
			set => SetProperty(ref commandExecutions, value);
		}

		public ICommand MaxLengthReachedCommand { get; }

		public MaxLengthReachedBehaviorViewModel()
			=> MaxLengthReachedCommand = CommandFactory.Create<string>(OnCommandExecuted);

		void OnCommandExecuted(string text)
			=> CommandExecutions += string.Format("MaxLength reached with value: '{0}'.", text) + Environment.NewLine;
	}
}