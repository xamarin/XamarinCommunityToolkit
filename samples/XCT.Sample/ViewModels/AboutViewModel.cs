﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Octokit;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace Xamarin.CommunityToolkit.Sample.ViewModels
{
	public class AboutViewModel : BaseViewModel
	{
		readonly GitHubClient gitHubClient = new GitHubClient(new ProductHeaderValue("XamarinCommunityToolkitSample"));

		RepositoryContributor[] contributors = Array.Empty<RepositoryContributor>();

		RepositoryContributor? selectedContributor;

		string emptyViewText = "Loading data...";

		public AboutViewModel()
		{
			PageAppearingCommand = CommandFactory.Create(OnAppearing);
			SelectedContributorCommand = CommandFactory.Create(async () =>
			{
				if (SelectedContributor == null)
					return;

				await Launcher.OpenAsync(SelectedContributor.HtmlUrl);
				SelectedContributor = null;
			});
		}

		public RepositoryContributor[] Contributors
		{
			get => contributors;
			set => SetProperty(ref contributors, value);
		}

		public RepositoryContributor? SelectedContributor
		{
			get => selectedContributor;
			set => SetProperty(ref selectedContributor, value);
		}

		public string EmptyViewText
		{
			get => emptyViewText;
			set => SetProperty(ref emptyViewText, value);
		}

		public ICommand PageAppearingCommand { get; }

		public ICommand SelectedContributorCommand { get; }

		public async Task OnAppearing()
		{
			if (Contributors.Any())
				return;

			try
			{
				var contributors = await gitHubClient.Repository.GetAllContributors("xamarin", "XamarinCommunityToolkit");

				// Initiate poor mans randomizer for lists
				// Note: there are better options for real production worthy large lists : https://stackoverflow.com/questions/273313/randomize-a-listt
				// But for now this linq version will do
				var random = new Random();
				var result = contributors?.OrderBy(x => random.Next()).ToArray();
				if (result != null)
					Contributors = result;
			}
			catch
			{
				// Suppress
			}

			if (Contributors.Any())
				return;

			EmptyViewText = "No data loaded...";
		}
	}
}