using System;
using ReleaseNotes.Core;
using ReleaseNotes.Output.Pdf;

namespace ReleaseNotes.TfsTool
{
    public static class ReleaseNotesWriterFactory
    {
        #region Methods

        public static IReleaseNotesWriter PdfGenerator(CommandLineOptions options)
        {
            var settings = new ReleaseNotesPdfWriterSettings
                           {
                               ProductName = options.ProductName,
                               BuildVersionNumber = options.BuildNumber,
                               GeneratedOn = DateTime.Now,
                               MergeReleaseNotesFile = options.MergeReleaseNotesFile,
                               LinkWorkItems = options.LinkWorkItems,
                               OutputFile = options.ExportFile
                           };

            var writer = new ReleaseNotesPdfWriter();
            writer.Configure(settings);

            return writer;
        }

        #endregion
    }
}