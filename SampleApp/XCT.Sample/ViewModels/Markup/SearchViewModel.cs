using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Markup
{
    public class SearchViewModel : BaseViewModel
    {
        ICommand backCommand, likeCommand, openTwitterSearchCommand, openHelpCommand;

        public string SearchText { get; set; }

        public List<Tweet> SearchResults { get; set; }

        public SearchViewModel()
        {
            SearchText = "#CSharpForMarkup";

            SearchResults = new List<Tweet>
            {
                new Tweet
                {
                    AuthorImage = "https://pbs.twimg.com/profile_images/1267649264391503873/I6w-glU8_400x400.jpg",
                    Header = "David Ortinau @davidortinau · 25/01/2020",
                    Body = new List<TextFragment>
                    {
                        new TextFragment { Text = "would it surprise you to know that the last project I personally shipped was Xamarin.Forms, C# w/ " },
                        new TextFragment { Text = "#CSharpForMarkup", IsMatch = true },
                        new TextFragment { Text = ", & @ReactiveXUI?" }
                    },
                },
                new Tweet
                {
                    AuthorImage = "https://pbs.twimg.com/profile_images/2159034926/MACAW_vincenth_LThumb_400x400.jpg",
                    Header = "VincentH.NET @vincenth_net · 26/03/2020",
                    Body = new List<TextFragment>
                    {
                        new TextFragment { Text = "Had an inspiring call with @matthewrdev about supporting #XamarinForms C# Markup in @mfractor\uD83D\uDE0E\n\nSo many great ideas. Autoformat, Convert XAML to C# Markup (all examples on internet!), auto split UI logic and markup...\n\nExcited " },
                        new TextFragment { Text = "#CSharpForMarkup", IsMatch = true },
                        new TextFragment { Text = "\uD83D\uDD25" }
                    },
                },
                new Tweet
                {
                    AuthorImage = "https://pbs.twimg.com/profile_images/1175428143944847361/0kfeW53l_400x400.jpg",
                    Header = "RK @rkonit · 05/02/2020",
                    Body = new List<TextFragment>
                    {
                        new TextFragment { Text = "\"Never Say Never\" in Open-source space. It's happening and reminds me early days of winforms.\n\n" },
                        new TextFragment { Text = "#CSharpForMarkup", IsMatch = true },
                        new TextFragment { Text = " #Xamarin #XamarinForms" }
                    },
                }
            };
        }

        public ICommand BackCommand => backCommand ??= new RelayCommand(Back);

        public ICommand LikeCommand => likeCommand ??= new RelayCommand<Tweet>(Like);

        public ICommand OpenTwitterSearchCommand => openTwitterSearchCommand ??= new RelayCommandAsync(OpenTwitterSearch);

        public ICommand OpenHelpCommand => openHelpCommand ??= new RelayCommandAsync(OpenHelp);

        void Back() { }

        void Like(Tweet tweet) => tweet.IsLikedByMe = !tweet.IsLikedByMe;

        Task OpenHelp() => Launcher.OpenAsync(new Uri("https://github.com/MicrosoftDocs/xamarin-communitytoolkit/blob/master/docs/markup.md"));

        Task OpenTwitterSearch() => Launcher.OpenAsync(new Uri("https://twitter.com/search?q=%23CSharpForMarkup"));

        public class Tweet : BaseViewModel
        {
            public string AuthorImage { get; set; }

            public string Header { get; set; }

            public List<TextFragment> Body { get; set; }

            public bool IsLikedByMe { get; set; }
        }

        public class TextFragment
        {
            public string Text { get; set; }

            public bool IsMatch { get; set; }
        }
    }
}