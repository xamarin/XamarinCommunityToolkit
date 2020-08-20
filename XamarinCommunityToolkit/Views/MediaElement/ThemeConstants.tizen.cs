using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Tizen.UI.Views
{
	public class ThemeConstants
	{
		public class MediaPlayer
		{
			public class Resources
			{
				public const string PlayImagePath = "Xamarin.CommunityToolkit.Resource.img_button_play.png";
				public const string PauseImagePath = "Xamarin.CommunityToolkit.Resource.img_button_pause.png";
			}

			public class ColorClass
			{
				public static readonly Color DefaultProgressLabelColor = Color.FromHex("#eeeeeeee");
				public static readonly Color DefaultProgressBarColor = Color.FromHex($"#4286f4");
				public static readonly Color DefaultProgressAreaColor = Color.FromHex("#80000000");
				public static readonly Color DefaultProgressAreaBackgroundColor = Color.FromHex("#50000000");
			}
		}
	}
}
