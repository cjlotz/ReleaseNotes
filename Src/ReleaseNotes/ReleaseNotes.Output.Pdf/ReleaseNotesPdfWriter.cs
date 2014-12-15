using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using ReleaseNotes.Core;

namespace ReleaseNotes.Output.Pdf
{
    public class ReleaseNotesPdfWriter : IReleaseNotesWriter
    {
        private const float BorderWidth = 1f;
        private const float CellPadding = 9f;
        private static readonly Font _cellFont = FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK);
        private static readonly Font _chapterFont = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.BLACK);
        private static readonly BaseColor _headerColor = new BaseColor(79, 129, 189);
        private static readonly BaseColor _tableBorderColor = _headerColor;
        private static readonly Font _tableHeaderFont = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.WHITE);
        private static readonly Font _urlFont = FontFactory.GetFont("Arial", 9, Font.UNDERLINE, BaseColor.BLUE);

        private Document _document;
        private Rectangle _pageSize;
        private PdfPTable _table;
        private PdfWriter _writer;

        public ReleaseNotesPdfWriter()
        {
            Bookmarks = new List<Bookmark>();
        }

        #region IReleaseNotesWriter Members

        public void Configure(IReleaseNotesWriterSettings settings)
        {
            ReleaseNotesPdfWriterSettings pdfSettings = settings as ReleaseNotesPdfWriterSettings;
            if (pdfSettings == null)
                throw new ArgumentException("Expected ReleaseNotesPdfWriterSettings", "settings");

            Settings = pdfSettings;
        }

        public void Publish(IReadOnlyList<ReleaseNoteWorkItem> workItems)
        {
            var stream = new MemoryStream();
            try
            {
                _document = new Document(PageSize.A4, 36, 36, 90, 72);

                // Initialize pdf writer
                _writer = PdfWriter.GetInstance(_document, stream);
                _writer.PageEvent = new ReleaseNotesPdfPageEvents(Settings);

                // Open document to write
                _document.Open();
                _document.AddTitle(Settings.GetDocumentTitle());
                _document.AddSubject(Settings.ProductName + " Release Notes");
                _document.AddAuthor("ReleaseNotes Generator");
                _document.AddKeywords(Settings.ProductName + "Release Notes");
                _document.AddCreationDate();
                _document.AddCreator("ReleaseNotes Generator");

                // Add manual release notes for current release
                int chapterNumber = 1;

                if (!string.IsNullOrEmpty(Settings.MergeReleaseNotesFile) && File.Exists(Settings.MergeReleaseNotesFile))
                {
                    Bookmarks.AddRange(Merge(Settings.MergeReleaseNotesFile, 1));
                    if (Bookmarks.Count > 0)
                        chapterNumber = Bookmarks.Count;
                }

                // Add automatic releases notes for current release
                WriteWorkItems("How do I?", ref chapterNumber, workItems.Where(x => x.ResolutionType == "How do I" || x.ResolutionType == "As Designed"));
                WriteWorkItems("Bug Fixes", ref chapterNumber, workItems.Where(x => x.ResolutionType == "Bug Fix"));
                WriteWorkItems("Known Issues", ref chapterNumber, workItems.Where(x => x.ResolutionType == "Known Issue"));
                WriteWorkItems("User Manual", ref chapterNumber, workItems.Where(x => x.ResolutionType == "User Manual"));

                CreateBookmarks();
            }
            catch (Exception exception)
            {
                throw new Exception("There has an unexpected exception occured whilst creating the release notes: " + exception.Message, exception);
            }
            finally
            {
                _document.Close();
            }

            File.WriteAllBytes(Settings.OutputFile, stream.GetBuffer());
        }

        #endregion

        #region Properties

        internal List<Bookmark> Bookmarks { get; private set; }
        private ReleaseNotesPdfWriterSettings Settings { get; set; }

        #endregion

        #region Methods

        private void CreateBookmarks()
        {
            PdfContentByte content = _writer.DirectContent;
            PdfOutline outline = content.RootOutline;

            foreach (var bookmark in Bookmarks)
            {
                if (bookmark.IsReleaseHeader)
                    outline = CreatePdfOutline(content.RootOutline, bookmark);
                else
                    CreatePdfOutline(outline, bookmark);
            }
        }

        private PdfPCell CreateCell(string text, Font font, BaseColor backgroundColor)
        {
            return new PdfPCell(new Phrase(text, font))
                   {
                       BorderColor = _tableBorderColor,
                       BorderWidth = BorderWidth,
                       BackgroundColor = backgroundColor,
                       PaddingTop = 9,
                       PaddingBottom = 9
                   };
        }

        private PdfOutline CreatePdfOutline(PdfOutline parent, Bookmark bookmark)
        {
            return new PdfOutline(parent, PdfAction.GotoLocalPage(bookmark.PageNumber, new PdfDestination(bookmark.PageNumber), _writer), bookmark.Title);
        }

        private PdfPCell CreateTableHeaderCell(string text)
        {
            return CreateCell(text, _tableHeaderFont, _headerColor);
        }

        private IList<Bookmark> ExportBookmarks(PdfReader reader, int pageShift)
        {
            IList<Dictionary<string, object>> bookmarks = SimpleBookmark.GetBookmark(reader);
            SimpleBookmark.ShiftPageNumbers(bookmarks, pageShift, null);

            var bookmarkItems = new List<Bookmark>();
            if (bookmarks != null)
            {
                bookmarkItems.AddRange(bookmarks.Select(bookmark => new Bookmark((string) bookmark["Title"], ParseBookmarkPageNumber((string) bookmark["Page"]))));
            }
            return bookmarkItems;
        }

        private IList<Bookmark> Merge(string mergeFile, int pageShift)
        {
            PdfContentByte content = _writer.DirectContent;

            // Create pdf reader
            PdfReader reader = new PdfReader(File.ReadAllBytes(mergeFile));

            // Export the bookmarks
            var bookmarks = ExportBookmarks(reader, pageShift);
            int numberOfPages = reader.NumberOfPages;

            // Iterate through all pages
            for (int currentPageIndex = 1; currentPageIndex <= numberOfPages; currentPageIndex++)
            {
                // Determine page size for the current page
                _pageSize = reader.GetPageSizeWithRotation(currentPageIndex);
                _document.SetPageSize(_pageSize);

                // Create page
                _document.NewPage();

                PdfImportedPage importedPage = _writer.GetImportedPage(reader, currentPageIndex);

                // Determine page orientation
                int pageOrientation = reader.GetPageRotation(currentPageIndex);
                if ((pageOrientation == 90) || (pageOrientation == 270))
                {
                    content.AddTemplate(importedPage, 0, -1f, 1f, 0, 0, _pageSize.Height);
                }
                else
                {
                    content.AddTemplate(importedPage, 1f, 0, 0, 1f, 0, 0);
                }
            }

            return bookmarks;
        }

        private int ParseBookmarkPageNumber(string pageText)
        {
            Regex expr = new Regex(@"^(\d+) (.+)$");
            Match match = expr.Match(pageText);
            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
            }
            return 1;
        }

        private PdfPCell WriteClientRef(ReleaseNoteWorkItem workItem)
        {
            var cell = new PdfPCell
                       {
                           BorderWidth = BorderWidth,
                           BorderColor = _headerColor,
                           PaddingTop = CellPadding,
                           PaddingBottom = CellPadding
                       };

            cell.AddElement(new Phrase(workItem.ClientRef.Trim(), _cellFont));
            if (Settings.LinkWorkItems)
            {
                var idText = new Chunk(string.Format(CultureInfo.InvariantCulture, "TFS{0}", workItem.WorkItemId.ToString(CultureInfo.InvariantCulture)), _urlFont);
                idText.SetAnchor(workItem.WorkItemAnchor);
                cell.AddElement(idText);
            }

            return cell;
        }

        private PdfPCell WriteFeedback(ReleaseNoteWorkItem workItem)
        {
            var sb = new StringBuilder();
            foreach (var element in HTMLWorker.ParseToList(new StringReader(workItem.Feedback), null))
            {
                if (element.IsContent())
                {
                    foreach (var chunk in element.Chunks)
                    {
                        sb.AppendLine(chunk.Content);
                    }
                }
            }
            return new PdfPCell(new Phrase(sb.ToString(), _cellFont))
                   {
                       BorderWidth = BorderWidth,
                       BorderColor = _headerColor,
                       PaddingTop = CellPadding,
                       PaddingBottom = CellPadding
                   };
        }

        private void WriteTableHeader()
        {
            _table.AddCell(CreateTableHeaderCell("Area"));
            _table.AddCell(CreateTableHeaderCell("Description"));
            _table.AddCell(CreateTableHeaderCell("Feedback"));
            _table.AddCell(CreateTableHeaderCell("Helpdesk Reference"));
        }

        private void WriteTableRow(ReleaseNoteWorkItem workItem)
        {
            _table.AddCell(CreateCell(workItem.Area, _cellFont, BaseColor.WHITE));
            _table.AddCell(CreateCell(workItem.WorkItemTitle, _cellFont, BaseColor.WHITE));
            _table.AddCell(WriteFeedback(workItem));
            _table.AddCell(WriteClientRef(workItem));
        }

        private void WriteWorkItems(string chapterText, ref int chapterNumber, IEnumerable<ReleaseNoteWorkItem> workItems)
        {
            if (!workItems.Any())
                return;

            _document.NewPage();

            string chapterTitle = string.Format(CultureInfo.InvariantCulture, "{0}. {1}", chapterNumber++, chapterText);

            var chapter = new Chunk(chapterTitle, _chapterFont);
            var paragraph = new Paragraph(20f, chapter);
            chapter.SetLocalDestination(_writer.PageNumber.ToString(CultureInfo.InvariantCulture));

            Bookmarks.Add(new Bookmark(chapterTitle, _writer.PageNumber));

            _document.Add(paragraph);

            _table = new PdfPTable(4);
            _table.SpacingBefore = 10f;
            _table.WidthPercentage = 100;

            _table.DefaultCell.BorderColor = _headerColor;
            _table.DefaultCell.BorderWidth = 100;
            _table.DefaultCell.Padding = 100;

            _table.SetWidths(new[] { 18f, 35f, 35f, 12f });
            _table.DefaultCell.VerticalAlignment = Element.ALIGN_TOP;

            WriteTableHeader();

            foreach (var grouping in workItems.OrderBy(x => x.Area).GroupBy(x => x.Area))
            {
                foreach (var workItem in grouping.OrderBy(x => x.WorkItemId))
                {
                    WriteTableRow(workItem);
                }
            }

            _document.Add(_table);
        }

        #endregion
    }
}