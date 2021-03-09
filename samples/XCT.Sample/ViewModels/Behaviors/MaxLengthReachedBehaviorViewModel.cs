using System;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Behaviors
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