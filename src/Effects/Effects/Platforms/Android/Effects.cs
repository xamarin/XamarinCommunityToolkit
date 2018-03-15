using System;
using System.Collections.Generic;
using System.Linq;
using Android.Runtime;
using Xamarin.Forms.Platform.Android;

namespace Xamarin.Toolkit.Effects.Droid
{
    [Preserve(AllMembers = true)]
    public static class Effects
    {
#pragma warning disable 414
        private static List<PlatformEffect> allEffects = new List<PlatformEffect>();
#pragma warning restore 414

        /// <summary>
        /// This is needed to ensure iOS loads the assembly with the effects in it
        /// </summary>
        public static void Init()
        {
            allEffects = new List<PlatformEffect>(typeof(Effects).Assembly.GetTypes().Where(t => typeof(PlatformEffect).IsAssignableFrom(t)).Select(t => (PlatformEffect)Activator.CreateInstance(t)));
        }
    }
}