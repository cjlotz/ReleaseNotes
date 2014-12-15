using System.Collections.Generic;

namespace ReleaseNotes.Core
{
    public interface IReleaseNotesWriter
    {
        void Publish(IReadOnlyList<ReleaseNoteWorkItem> workItems);
        void Configure(IReleaseNotesWriterSettings settings);
    }
}