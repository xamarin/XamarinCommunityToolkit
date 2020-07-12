namespace XamarinCommunityToolkitSample.Models.Converters
{
    public enum ConverterSectionId
    {
        ItemTappedEventArgs,
    }

    public static class ViewSectionIdIdExtensions
    {
        public static string GetTitle(this ConverterSectionId id)
            => id switch
            {
                _ => id.ToString()
            };
    }
}
