using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using ReleaseNotes.Core;

namespace ReleaseNotes.TfsTool
{
    public class ReportRunner
    {
        private TswaClientHyperlinkService _linkService;
        private TfsTeamProjectCollection _teamProjectCollection;
        private Project _tfsProject;
        private QueryFolder _tfsQueryFolder;
        private QueryItem _tfsQueryItem;
        private WorkItemStore _workItemStore;
        private FileStream _warningsFile;

        public ReportRunner()
        {
        }

        #region Methods

        public void Generate(CommandLineOptions options)
        {
            if (string.IsNullOrEmpty(options.ExportFile))
                throw new ArgumentException("No ExportFile specified");

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                Log(string.Format(CultureInfo.InvariantCulture, "Generating Release Notes to {0}...", options.ExportFile));

                ConnectToTfs(options);

                var workItems = GenerateReleaseNoteWorkItems(options);

                IReleaseNotesWriter reportWriter = ReleaseNotesWriterFactory.PdfGenerator(options);
                reportWriter.Publish(workItems);

            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

            Log("Completed Release Notes Generation");
        }

        private void ConnectToTfs(CommandLineOptions options)
        {
            Log(string.Format(CultureInfo.InvariantCulture, "Connecting to Tfs at {0}...", options.TfsServerUrl));

            // Connect to Team Foundation Server
            Uri tfsUri = new Uri(options.TfsServerUrl);
            _teamProjectCollection = new TfsTeamProjectCollection(tfsUri);

            _linkService = _teamProjectCollection.GetService<TswaClientHyperlinkService>();
            _workItemStore = _teamProjectCollection.GetService<WorkItemStore>();
            _tfsProject = _workItemStore.Projects[options.TfsProject];

            if (options.TfsQuery == null)
            {
                _tfsQueryFolder = _tfsProject.QueryHierarchy[options.TfsQueryHierarchy] as QueryFolder;
                _tfsQueryItem = _tfsQueryFolder[options.TfsQueryName];
            }
            else
            {
                _tfsQueryItem = options.TfsQuery;
            }
        }

        private List<ReleaseNoteWorkItem> GenerateReleaseNoteWorkItems(CommandLineOptions options)
        {
            if (!string.IsNullOrWhiteSpace(options.WarningsFile))
            {
                _warningsFile = File.Create(options.WarningsFile);
                Log(string.Format(CultureInfo.InvariantCulture, "Writing out warnings to {0}...", options.WarningsFile));
            }

            Log(string.Format(CultureInfo.InvariantCulture, "Querying Tfs using {0}...", _tfsQueryItem.ToString()));

            var releaseNotes = new List<ReleaseNoteWorkItem>();
            var queryDefinition = _workItemStore.GetQueryDefinition(_tfsQueryItem.Id);
            var variables = new Dictionary<string, string>
            {
                { "project", _tfsQueryItem.Project.Name }
            };

            var workItemCollection = _workItemStore.Query(queryDefinition.QueryText, variables);

            Log(string.Format(CultureInfo.InvariantCulture, "Found {0} Work Items", workItemCollection.Count));

            foreach (WorkItem workItem in workItemCollection)
            {
                Log(string.Format(CultureInfo.InvariantCulture, "WI: {0}, Title: {1}", workItem.Id, workItem.Title));

                bool isBug = workItem.Type.Name == "Bug";
                if (!releaseNotes.Any(r => r.WorkItemId == workItem.Id))
                {
                    string[] areas = workItem.AreaPath.Split(new[] { @"\" }, StringSplitOptions.RemoveEmptyEntries);
                    string area = string.Empty;
                    for (int i = 1; i < areas.Length; i++)
                    {
                        area += areas[i] + " : ";
                    }

                    var note = new ReleaseNoteWorkItem
                    {
                        Area = area.TrimEnd(' ', ':'),
                        BuildNumber = isBug ? (string)workItem["Integration Build"] : "N/A",
                        Classification = (string)workItem["Classification"],
                        ClientRef = isBug ? (string)workItem["Source reference"] : string.Empty,
                        Feedback = isBug ? (string)workItem["Release Note Html"] : "N/A",
                        ResolutionType = isBug ? (string)workItem["Resolution Type"] : "N/A",
                        Severity = isBug ? (string)workItem["Severity"] : string.Empty,
                        WorkItemId = workItem.Id,
                        WorkItemTitle = workItem.Title,
                        WorkItemAnchor = _linkService.GetWorkItemEditorUrl(workItem.Id).ToString(),
                    };

                    VerifyNote(options, workItem, note);
                    releaseNotes.Add(note);
                }
            }

            if (_warningsFile != null)
            {
                _warningsFile.Close();
                Log(string.Format(CultureInfo.InvariantCulture, "Warnings written to {0}", options.WarningsFile));
            }

            Log("Done Querying Tfs");
            return releaseNotes;
        }

        private void Log(string text)
        {
            Console.WriteLine("ReleaseNotes: {0}", text);
        }

        private void VerifyNote(CommandLineOptions options, WorkItem workItem, ReleaseNoteWorkItem note)
        {
            string warningText = string.Empty;
            if (string.IsNullOrWhiteSpace(note.Area))
            {
                warningText = string.Format(CultureInfo.InvariantCulture, "WI: {0}, No Area Path specified! Url to fix {1}{2}", workItem.Id, note.WorkItemAnchor, Environment.NewLine);
            }

            if (string.IsNullOrWhiteSpace(note.Feedback) || note.Feedback == options.IncompleteFeedbackTag)
            {
                warningText += string.Format(CultureInfo.InvariantCulture, "WI: {0}, No Feedback specified! Url to fix {1}{2}", workItem.Id, note.WorkItemAnchor, Environment.NewLine);
            }

            if (!string.IsNullOrWhiteSpace(warningText))
            {
                if (_warningsFile != null)
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(warningText);
                    _warningsFile.Write(info, 0, info.Length);
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Log(string.Format(CultureInfo.InvariantCulture, "Warn: {0}", warningText));
                Console.ResetColor();
            }
        }

        #endregion
    }
}
