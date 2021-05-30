using System;
using System.Windows.Input;
using CommunityToolkit.Maui.ObjectModel;

namespace CommunityToolkit.Maui.Sample.ViewModels.Behaviors
{
	public class UserStoppedTypingBehaviorViewModel : BaseViewModel
	{
		string performedSearches = string.Empty;

		public string PerformedSearches
		{
			get => performedSearches;
			set => SetProperty(ref performedSearches, value);
		}

		public ICommand SearchCommand { get; }

		public UserStoppedTypingBehaviorViewModel()
			=> SearchCommand = CommandFactory.Create<string>(PerformSearch);

		void PerformSearch(string searchTerms)
			=> PerformedSearches += string.Format("Performed search for '{0}'.", searchTerms) + Environment.NewLine;
	}
}