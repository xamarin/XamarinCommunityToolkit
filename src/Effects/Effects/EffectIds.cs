namespace FormsCommunityToolkit.Effects
{
    /// <summary>
    /// Effect ids of all the available effects.
    /// Can be used to resolve an effect from code.
    ///     e.g. MyEntry.Effects.Add(Effect.Resolve(EffectIds.ViewBlurEffect));
    /// </summary>
    public class EffectIds
    {
        /// <summary>
        /// Id for <see cref="ViewBlur"/>
        /// </summary>
        public static string ViewBlur => typeof(ViewBlurEffect).FullName;

        /// <summary>
        /// Id for <see cref="EntryCapitalizeKeyboard"/>
        /// </summary>
        public static string EntryCapitalizeKeyboard => typeof(EntryCapitalizeKeyboard).FullName;

        /// <summary>
        /// Id for <see cref="SwitchChangeColor"/>
        /// </summary>
        public static string SwitchChangeColor => typeof(SwitchChangeColorEffect).FullName;

        /// <summary>
        /// Id for <see cref="PickerChangeColor"/>
        /// </summary>
        public static string PickerChangeColor => typeof(PickerChangeColorEffect).FullName;

        /// <summary>
        /// Id for <see cref="EntryClear"/>
        /// </summary>
        public static string EntryClear => typeof(EntryClear).FullName;

        /// <summary>
        /// Id for <see cref="LabelCustomFont"/>
        /// </summary>
        public static string LabelCustomFont => typeof(LabelCustomFont).FullName;

        /// <summary>
        /// Id for <see cref="EntryDisableAutoCorrect"/>
        /// </summary>
        public static string EntryDisableAutoCorrect => typeof(EntryDisableAutoCorrect).FullName;

        /// <summary>
        /// Id for <see cref="EntryItalicPlaceholder"/>
        /// </summary>
        public static string EntryItalicPlaceholder => typeof(EntryItalicPlaceholder).FullName;

        /// <summary>
        /// Id for <see cref="LabelMultiLine"/>
        /// </summary>
        public static string LabelMultiLine => typeof(LabelMultiLine).FullName;

        /// <summary>
        /// Id for <see cref="EntryRemoveBorder"/>
        /// </summary>
        public static string EntryRemoveBorder => typeof(EntryRemoveBorder).FullName;

        /// <summary>
        /// Id for <see cref="EntryRemoveLine"/>
        /// </summary>
        public static string EntryRemoveLine => typeof(EntryRemoveLine).FullName;

        /// <summary>
        /// Id for <see cref="SearchBarSuggestion"/>
        /// </summary>
        public static string SearchBarSuggestion => typeof(SearchBarSuggestionEffect).FullName;

        /// <summary>
        /// Id for <see cref="EntrySelectAllText"/>
        /// </summary>
        public static string EntrySelectAllText => typeof(EntrySelectAllText).FullName;

        /// <summary>
        /// Id for <see cref="LabelSizeFontToFit"/>
        /// </summary>
        public static string LabelSizeFontToFit => typeof(LabelSizeFontToFit).FullName;

        /// <summary>
        /// Id for <see cref="HideEditorUnderbarEffect"/>
        /// </summary>
        public static string HideEditorUnderbar => typeof(HideEditorUnderbar).FullName;
    }
}
