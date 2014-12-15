using System;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ReleaseNotes.Core;

namespace ReleaseNotes.Output.Pdf
{
    internal class ReleaseNotesPdfPageEvents : IPdfPageEvent
    {
        private const float FooterFontSize = 9;
        private const float FooterYPos = 30;
        private const float HeadingFontSize = 18;
        private const float HorizontalSpacer = 60;

        private BaseFont _baseFont = null;
        private BaseFont _frontPageFont = null;

        private PdfContentByte _content;
        private readonly ReleaseNotesPdfWriterSettings _settings;
        private bool _skipPageFooter;
        private PdfImportedPage _template;

        public ReleaseNotesPdfPageEvents(ReleaseNotesPdfWriterSettings settings)
        {
            _settings = settings;
        }

        #region IPdfPageEvent Members

        public void OnChapter(PdfWriter writer, Document document, float paragraphPosition, Paragraph title)
        {
            Console.WriteLine("Chapter Start: " + title.Content);
        }

        public void OnChapterEnd(PdfWriter writer, Document document, float paragraphPosition)
        {
            Console.WriteLine("Chapter End:");
        }

        public void OnCloseDocument(PdfWriter writer, Document document)
        {
        }

        public void OnEndPage(PdfWriter writer, Document document)
        {
            WriterFooter(writer);
            WritePageTemplate(writer);
            _skipPageFooter = false;
        }

        public void OnGenericTag(PdfWriter writer, Document document, Rectangle rect, string text)
        {
        }

        public void OnOpenDocument(PdfWriter writer, Document document)
        {
            _baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.EMBEDDED);
            _frontPageFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.EMBEDDED);
            _content = writer.DirectContent;

            if (!string.IsNullOrEmpty(_settings.PageTemplate))
            {
                var reader = new PdfReader(_settings.PageTemplate);
                _template = writer.GetImportedPage(reader, 1);
            }

            WriteFrontPage(writer, document);
        }

        public void OnParagraph(PdfWriter writer, Document document, float paragraphPosition)
        {
        }

        public void OnParagraphEnd(PdfWriter writer, Document document, float paragraphPosition)
        {
        }

        public void OnSection(PdfWriter writer, Document document, float paragraphPosition, int depth, Paragraph title)
        {
            Console.WriteLine("Section Start: " + title.Content);
        }

        public void OnSectionEnd(PdfWriter writer, Document document, float paragraphPosition)
        {
            Console.WriteLine("Section End:");
        }

        public void OnStartPage(PdfWriter writer, Document document)
        {
        }

        #endregion

        #region Methods

        private float GetFooterTextRightPosition(string text, PdfWriter writer)
        {
            return writer.PageSize.Width - HorizontalSpacer - _baseFont.GetWidthPoint(text, FooterFontSize)/2;
        }

        private float GetFrontPageTextXPosition(string text, PdfWriter writer)
        {
            return (writer.PageSize.Width - _frontPageFont.GetWidthPoint(text, HeadingFontSize))/2;
        }

        private float GetFrontPageTextYPosition(PdfWriter writer)
        {
            return writer.PageSize.Height/2;
        }

        private void WriteFrontPage(PdfWriter writer, Document document)
        {
            if (!string.IsNullOrEmpty(_settings.FrontPageTemplate))
            {
                document.NewPage();

                // Write Generated On
                string title = _settings.GetDocumentTitle();

                _content.BeginText();
                _content.SetFontAndSize(_frontPageFont, HeadingFontSize);
                _content.SetTextMatrix(GetFrontPageTextXPosition(title, writer), GetFrontPageTextYPosition(writer));
                _content.ShowText(title);
                _content.EndText();

                var reader = new PdfReader(_settings.FrontPageTemplate);
                writer.DirectContentUnder.AddTemplate(writer.GetImportedPage(reader, 1), 0, 0);
                _skipPageFooter = true;
            }
        }

        private void WritePageTemplate(PdfWriter writer)
        {
            if (_template != null)
                writer.DirectContentUnder.AddTemplate(_template, 0, 0);
        }

        private void WriterFooter(PdfWriter writer)
        {
            if (_skipPageFooter)
                return;

            // Write footer text (page numbers)
            string text = writer.PageNumber.ToString(CultureInfo.InvariantCulture);
            _content.BeginText();
            _content.SetFontAndSize(_baseFont, FooterFontSize);
            _content.SetTextMatrix(GetFooterTextRightPosition(text, writer), FooterYPos);
            _content.ShowText(text);
            _content.EndText();

            // Write Generated On
            text = "Generated";
            if (!string.IsNullOrEmpty(_settings.BuildVersionNumber))
                text += " for Build " + _settings.BuildVersionNumber;

            text += " on " + _settings.GeneratedOn.ToString("G");

            _content.BeginText();
            _content.SetFontAndSize(_baseFont, FooterFontSize);
            _content.SetTextMatrix(HorizontalSpacer, FooterYPos);
            _content.ShowText(text);
            _content.EndText();
        }

        #endregion
    }
}