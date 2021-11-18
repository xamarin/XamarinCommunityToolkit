using System;
using Xamarin.CommunityToolkit.Extensions;

namespace Xamarin.CommunityToolkit.Sample.Pages.Effects
{
	public partial class SemanticExtensionsPage
	{
		public SemanticExtensionsPage()
		{
			InitializeComponent();
		}

		void SetSemanticFocusButton_Clicked(object sender, System.EventArgs e)
		{
			SemanticExtensions.SetSemanticFocus(semanticFocusLabel);
		}

		void Announce_Clicked(object sender, EventArgs e)
		{
			SemanticExtensions.Announce("This is the announcement text");
		}
	}
}