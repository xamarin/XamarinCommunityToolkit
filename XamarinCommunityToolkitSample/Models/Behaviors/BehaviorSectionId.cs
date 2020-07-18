
namespace Microsoft.Toolkit.Xamarin.Forms.Sample.Models.Behaviors
{
    public enum BehaviorSectionId
    {
        EventToCommand,
        EmailValidation,
        NumericValidation
    }

    public static class ViewSectionIdIdExtensions
    {
        public static string GetTitle(this BehaviorSectionId id)
            => id switch
            {
                _ => id.ToString()
            };
    }
}
