using System;

namespace ReleaseNotes.Core
{
    public interface IReleaseNotesWriterSettings
    {
        string ProductName { get; set; }
        string BuildVersionNumber { get; set; }
        DateTime GeneratedOn { get; set; }
        bool LinkWorkItems { get; set; }
        string MergeReleaseNotesFile { get; set; }
        string OutputFile { get; set; }
    }
}