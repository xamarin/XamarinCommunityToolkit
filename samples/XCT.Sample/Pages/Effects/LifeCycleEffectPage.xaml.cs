using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.CommunityToolkit.Sample.Pages.Effects
{
	public partial class LifeCycleEffectPage
	{
		public LifeCycleEffectPage()
		{
			InitializeComponent();
		}

		void LifeCycleEffect_Loaded(object sender, EventArgs e)
		{
			if (sender is Button)
				Debug.WriteLine("Button loaded");
			if (sender is Image)
				Debug.WriteLine("Image loaded");
			if (sender is Label)
				Debug.WriteLine("Label loaded");
			if (sender is StackLayout)
				Debug.WriteLine("StackLayout loaded");
		}

		void LifeCycleEffect_Unloaded(object sender, EventArgs e)
		{
			if (sender is Button)
				Debug.WriteLine("Button unloaded");
			if (sender is Image)
				Debug.WriteLine("Image unloaded");
			if (sender is Label)
				Debug.WriteLine("Label unloaded");
			if (sender is StackLayout)
				Debug.WriteLine("StackLayout unloaded");

		}

		void Button_Clicked(object sender, EventArgs e)
		{
			img.IsVisible = !img.IsVisible;
		}
	}
}