using System.IO;
using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Xamarin.Toolkit.Droid.Controls;

namespace Xamarin.Toolkit.Droid.SampleApp.SamplePages
{
    [Activity(Label = "MarkdownTextViewSamplePage")]
    public class MarkdownTextViewSamplePage : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MarkdownTextViewSamplePage);
            GetText();
        }

        private void MarkdownTextView_Clicked(object sender, LinkClickedEventArgs e)
        {
            if (System.Uri.IsWellFormedUriString(e.Link, System.UriKind.Absolute))
            {
                var viewLink = new Intent(Intent.ActionView, Uri.Parse(e.Link));
                StartActivity(viewLink);
            }
        }

        private async void GetText()
        {
            using (var md = Assets.Open("InitialContent.md"))
            {
                using (var reader = new StreamReader(md))
                {
                    var content = await reader.ReadToEndAsync();
                    MarkdownTextView.Text = content;
                    markdownTextView.ImageClicked += MarkdownTextView_Clicked;
                    markdownTextView.LinkClicked += MarkdownTextView_Clicked;
                }
            }
        }

        public MarkdownTextView MarkdownTextView => markdownTextView ?? (markdownTextView = FindViewById<MarkdownTextView>(Resource.Id.MarkdownTextView_Sample));

        private MarkdownTextView markdownTextView;
    }
}
