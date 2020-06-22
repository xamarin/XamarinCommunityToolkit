using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Octokit;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XamarinCommunityToolkitSample.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        readonly GitHubClient gitHubClient = new GitHubClient(new ProductHeaderValue("XamarinCommunityToolkitSample"));

        ObservableCollection<RepositoryContributor> contributors;
        public ObservableCollection<RepositoryContributor> Contributors
        {
            get => contributors;
            set => Set(ref contributors, value);
        }

        RepositoryContributor selectedContributor;
        public RepositoryContributor SelectedContributor
        {
            get => selectedContributor;
            set => Set(ref selectedContributor, value);
        }

        string emptyViewText = "Loading data...";
        public string EmptyViewText
        {
            get => emptyViewText;
            set => Set(ref emptyViewText, value);
        }

        ICommand selectedContributorCommand;
        public ICommand SelectedContributorCommand => selectedContributorCommand ??= new Command(async _ =>
        {
            if (SelectedContributor is null)
                return;

            await Launcher.OpenAsync(SelectedContributor.HtmlUrl);
            SelectedContributor = null;
        });

        public async Task OnAppearing()
        {
            if (Contributors != null)
                return;

            try
            {
                var contributors = await gitHubClient.Repository.GetAllContributors("xamarin", "XamarinCommunityToolkit");

                if (contributors is null)
                    return;

                //Initiate poor mans randomizer for lists
                //Note: there are better options for real production worthy large lists : https://stackoverflow.com/questions/273313/randomize-a-listt
                //But for now this linq version will do
                var rnd = new Random();
                Contributors = new ObservableCollection<RepositoryContributor>(contributors.Select(x => new { value = x, order = rnd.Next() }).OrderBy(x => x.order).Select(x => x.value));
            }
            catch(Exception ex)
            {
            }
            finally
            {
                if(Contributors is null || !Contributors.Any())
                    EmptyViewText = "No data loaded...";
            }
        }
    }
}