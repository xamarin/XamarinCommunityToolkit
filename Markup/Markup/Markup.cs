using System.Runtime.CompilerServices;

namespace Xamarin.Forms.Core.Markup
{
	public static class Markup // TODO: Markup remove class in XTC
	{
		static bool isExperimentalFlagSet = false;

		public static void VerifyExperimental([CallerMemberName] string memberName = "", string constructorHint = null)
		{
			if (isExperimentalFlagSet)
				return;

			ExperimentalFlags.VerifyFlagEnabled(nameof(Markup), ExperimentalFlags.MarkupExperimental, constructorHint, memberName);

			isExperimentalFlagSet = true;
		}

		public static void ClearExperimental() => isExperimentalFlagSet = false;
	}
}
