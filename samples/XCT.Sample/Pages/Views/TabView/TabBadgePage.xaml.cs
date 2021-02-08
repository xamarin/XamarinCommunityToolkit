using System;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views.TabView
{
	public partial class TabBadgePage : BasePage
	{
		int counter;

		public TabBadgePage()
		{
			InitializeComponent();

			BindingContext = this;

			Counter = 2;
		}

		public int Counter
		{
			get => counter;

			set
			{
				counter = value;
				OnPropertyChanged();
			}
		}

		void OnIncreaseClicked(object sender, EventArgs e) => Counter++;

		void OnDecreaseClicked(object sender, EventArgs e)
		{
			if (Counter == 0)
				return;

			Counter--;
		}
	}
}