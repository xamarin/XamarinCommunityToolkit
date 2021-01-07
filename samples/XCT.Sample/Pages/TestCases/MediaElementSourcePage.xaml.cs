using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.CommunityToolkit.Sample.Pages.TestCases
{
	public partial class MediaElementSourcePage
	{
		public MediaElementSourcePage()
		{
			InitializeComponent();
		}
	}


	class MediaElementViewModel : BindableObject
	{
		public string VideoAsString { get; set; } = "https://tipcalculator.appwithkiss.com/video/Hint_1_2_EN_12.mov";

		public MediaSource VideoAsMediaSource => MediaSource.FromUri(VideoAsString);
	}
}