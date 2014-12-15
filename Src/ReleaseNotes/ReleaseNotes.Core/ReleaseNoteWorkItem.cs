namespace ReleaseNotes.Core
{
    public class ReleaseNoteWorkItem
    {
        #region Properties

        public string Area { get; set; }
        public string BuildNumber { get; set; }
        public string Classification { get; set; }
        public string ClientRef { get; set; }
        public string Feedback { get; set; }
        public string ResolutionType { get; set; }
        public string Severity { get; set; }
        public string WorkItemAnchor { get; set; }
        public int WorkItemId { get; set; }
        public string WorkItemTitle { get; set; }

        #endregion
    }
}