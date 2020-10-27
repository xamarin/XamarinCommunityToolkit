using System;
using Xamarin.Forms;

[assembly: ResolutionGroupName("Xamarin.CommunityToolkit.Effects")]

namespace Xamarin.CommunityToolkit.Effects
{
	public sealed class EffectIds
	{
        static string effectResolutionGroupName = $"{nameof(Xamarin)}.{nameof(CommunityToolkit)}.{nameof(Effects)}";

        /// <summary>
        /// Id for <see cref="RemoveBorderEffect"/>
        /// </summary>
        public static string RemoveBorder => $"{effectResolutionGroupName}.{nameof(RemoveBorderEffect)}";

        /// <summary>
        /// Id for <see cref="SelectAllTextEffect"/>
        /// </summary>
        public static string SelectAllText => $"{effectResolutionGroupName}.{nameof(SelectAllTextEffect)}";

        /// <summary>
        /// Id for <see cref="SafeAreaEffect"/>
        /// </summary>
        public static string SafeArea => $"{effectResolutionGroupName}.{nameof(SafeAreaEffectRouter)}";
    }
}
