namespace ReleaseNotes.Core
{
    public class Bookmark
    {
        public string Title { get; set; }
        public int PageNumber { get; set; }

        public Bookmark(string title, int pageNumber)
        {
            Title = title;
            PageNumber = pageNumber;
        }

        public bool IsReleaseHeader
        {
            get { return Title.StartsWith("On Key", System.StringComparison.Ordinal); }
        }
    }
}
