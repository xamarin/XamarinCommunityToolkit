using System;
using System.Linq;
using System.Collections.Generic;
using Foundation;

namespace XamarinCommunityToolkit.Controls.iOS
{
    [Preserve(AllMembers = true)]
    public static class Controls
    {
#pragma warning disable 414
        static List<object> allRenderers = new List<object>();
#pragma warning restore 414
        /// <summary>
        /// This is needed to ensure iOS loads the assembly with the controls in it
        /// </summary>
        public static void Init()
        {
            allRenderers = new List<object>(typeof(Controls).Assembly.GetTypes().Where(t => t.Name.Contains("Renderer")).Select(Activator.CreateInstance));
        }
    }
}