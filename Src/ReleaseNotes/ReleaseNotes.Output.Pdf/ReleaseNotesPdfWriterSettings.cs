using System;
using System.Globalization;

namespace ReleaseNotes.Core
{
    public class ReleaseNotesPdfWriterSettings : IReleaseNotesWriterSettings
    {
        public ReleaseNotesPdfWriterSettings()
        {
            FrontPageTemplate = @".\Templates\ReleaseNotes_FrontPage.pdf";
            PageTemplate = @".\Templates\ReleaseNotes_PageTemplate.pdf";
        }

        #region IReleaseNotesWriterSettings Members

        public string ProductName { get; set; }
        public string BuildVersionNumber { get; set; }
        public DateTime GeneratedOn { get; set; }
        public bool LinkWorkItems { get; set; }
        public string MergeReleaseNotesFile { get; set; }
        public string OutputFile { get; set; }

        #endregion

        #region Properties

        public string FrontPageTemplate { get; set; }
        public string PageTemplate { get; set; }

        #endregion

        #region Methods

        public string GetDocumentTitle()
        {
            string title = ProductName + " Release Notes";
            if (!string.IsNullOrEmpty(BuildVersionNumber))
            {
                string[] parts = BuildVersionNumber.Split('.');
                if (parts.Length == 4)
                    title = string.Format(CultureInfo.InvariantCulture, "{0} {1}.{2} Release Notes", ProductName, parts[0], parts[1]);
            }
            return title;
        }

        #endregion
    }
}