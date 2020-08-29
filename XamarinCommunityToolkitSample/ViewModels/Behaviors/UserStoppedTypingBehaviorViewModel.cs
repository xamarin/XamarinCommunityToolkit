using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Behaviors
{
	public class UserStoppedTypingBehaviorViewModel : BaseViewModel
	{
		#region Properties

		private string _performedSearches;
		public string PerformedSearches
		{
			get => _performedSearches;
			set => Set(ref _performedSearches, value);
		}

		public ICommand SearchCommand { get; }

		#endregion Properties

		public UserStoppedTypingBehaviorViewModel()
		{
			SearchCommand = new Command<string>(PerformSearch);
		}

		private void PerformSearch(string searchTerms)
		{
			PerformedSearches += $"Performed search for '{searchTerms}'{Environment.NewLine}";
		}
	}
}