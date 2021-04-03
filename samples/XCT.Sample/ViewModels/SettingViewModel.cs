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
		IList<Language> supportedLanguages = Enumerable.Empty<Language>().ToList();

		Language selectedLanguage = new Language(AppResources.English, "en");

		public SettingViewModel()
		{
			LoadLanguages();

			ChangeLanguageCommand = CommandFactory.Create(() =>
			{
				LocalizationResourceManager.Current.CurrentCulture = CultureInfo.GetCultureInfo(SelectedLanguage.CI);
				LoadLanguages();
			});
		}

		public LocalizedString AppVersion { get; } = new LocalizedString(() => string.Format(AppResources.Version, AppInfo.VersionString));

		public ICommand ChangeLanguageCommand { get; }

		public Language SelectedLanguage
		{
			get => selectedLanguage;
			set => SetProperty(ref selectedLanguage, value);
		}

		public IList<Language> SupportedLanguages
		{
			get => supportedLanguages;
			private set => SetProperty(ref supportedLanguages, value);
		}

		void LoadLanguages()
		{
			SupportedLanguages = new List<Language>()
			{
				new Language(AppResources.English, "en"),
				new Language(AppResources.Spanish, "es")
			};
			SelectedLanguage = SupportedLanguages.FirstOrDefault(pro => pro.CI == LocalizationResourceManager.Current.CurrentCulture.TwoLetterISOLanguageName);
		}
	}
}