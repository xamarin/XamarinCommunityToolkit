using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.Sample.Models;
using Xamarin.CommunityToolkit.Sample.Resx;
using Xamarin.Essentials;

namespace Xamarin.CommunityToolkit.Sample.ViewModels
{
	public class SettingViewModel : BaseViewModel
	{
		Language selectedLanguage = new Language(AppResources.English, "en");

		public SettingViewModel()
		{
			SupportedLanguages = new List<Language>()
			{
				new Language(AppResources.English, "en"),
				new Language(AppResources.Spanish, "es")
			};

			LoadLanguages();

			ChangeLanguageCommand = CommandFactory.Create(() =>
			{
				LocalizationResourceManager.Current.CurrentCulture = CultureInfo.GetCultureInfo(SelectedLanguage.CI);
				LoadLanguages();
			});
		}

		public LocalizedString AppVersion { get; } = new (() => string.Format(AppResources.Version, AppInfo.VersionString));

		public ICommand ChangeLanguageCommand { get; }

		public IList<Language> SupportedLanguages { get; }

		public Language SelectedLanguage
		{
			get => selectedLanguage;
			set => SetProperty(ref selectedLanguage, value);
		}

		void LoadLanguages()
		{
			var currentCulture = LocalizationResourceManager.Current.CurrentCulture;
			if (currentCulture == null)
			{
				SelectedLanguage = SupportedLanguages.First();
			}
			else
			{
				SelectedLanguage = SupportedLanguages.Single(pro => pro.CI == currentCulture.TwoLetterISOLanguageName);
			}
		}
	}
}