
namespace Microsoft.Toolkit.Xamarin.Forms.Sample.Models.Views
{
    public class ViewSectionModel
    {
        public ViewSectionModel(ViewSectionId id)
        {
            Id = id;
            Title = id.GetTitle();
        }

        public ViewSectionId Id { get; }
        public string Title { get; }
    }
}
