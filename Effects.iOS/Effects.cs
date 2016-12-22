using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.Platform.iOS;
using System;
using Foundation;

namespace FormsCommunityToolkit.Effects.iOS
{
    [Preserve(AllMembers = true)]
    public static class iOSEffects
    {
#pragma warning disable 414
        private static List<PlatformEffect> _allEffects = new List<PlatformEffect>();
#pragma warning restore 414

        /// <summary>
        /// This is needed to ensure iOS loads the assembly with the effects in it
        /// </summary>
        public static void Init()
        {
            _allEffects = new List<PlatformEffect>(typeof(iOSEffects).Assembly.GetTypes().Where(t => typeof(PlatformEffect).IsAssignableFrom(t)).Select(t => (PlatformEffect)Activator.CreateInstance(t)));
        }
    }
}
