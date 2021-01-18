﻿using System;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Behaviors
{
	public class UserStoppedTypingBehaviorViewModel : BaseViewModel
	{
		#region Properties

		string performedSearches;

		public string PerformedSearches
		{
			get => performedSearches;
			set => SetProperty(ref performedSearches, value);
		}

		public ICommand SearchCommand { get; }

		#endregion Properties

		public UserStoppedTypingBehaviorViewModel()
			=> SearchCommand = CommandHelper.Create<string>(PerformSearch);

		void PerformSearch(string searchTerms)
			=> PerformedSearches += string.Format("Performed search for '{0}'.", searchTerms) + Environment.NewLine;
	}
}