using System;
using System.Linq;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace ReleaseNotes.TfsTool
{
    public enum AppAction
    {
        View = 0,
        Export = 1
    }

    public class CommandLineOptions
    {
        public CommandLineOptions()
        {
            IncompleteFeedbackTag = "<Incomplete>";
            ProductName = "<MyProduct>";
            BuildNumber = string.Empty;
            MergeReleaseNotesFile = string.Empty;
            LinkWorkItems = false;
            WarningsFile = "ReleaseNotesWarnings.txt";
        }

        #region Properties

        public AppAction Action { get; set; }
        public string BuildNumber { get; set; }
        public string ExportFile { get; set; }
        public string IncompleteFeedbackTag { get; set; }
        public bool LinkWorkItems { get; set; }
        public string MergeReleaseNotesFile { get; set; }
        public string ProductName { get; set; }
        public string TfsProject { get; set; }
        public QueryDefinition TfsQuery { get; set; }
        public string TfsQueryHierarchy { get; set; }
        public string TfsQueryName { get; set; }
        public string TfsServerUrl { get; set; }
        public string WarningsFile { get; set; }

        #endregion

        #region Methods

        public void Parse(string[] args)
        {
            string optionValue;
            if (TryFindOptionValue("/ProductName", args, out optionValue))
            {
                ProductName = optionValue;
            }
            if (TryFindOptionValue("/BuildNumber", args, out optionValue))
            {
                BuildNumber = optionValue;
            }
            if (TryFindOptionValue("/MergeFile", args, out optionValue))
            {
                MergeReleaseNotesFile = optionValue;
            }
            if (TryFindOptionValue("/LinkWorkItems", args, out optionValue))
            {
                LinkWorkItems = bool.Parse(optionValue);
            }
            if (TryFindOptionValue("/Action", args, out optionValue))
            {
                Action = (AppAction) Enum.Parse(typeof (AppAction), optionValue);
            }
            if (TryFindOptionValue("/ExportFile", args, out optionValue))
            {
                ExportFile = optionValue;
            }
            if (TryFindOptionValue("/WarningsFile", args, out optionValue))
            {
                WarningsFile = optionValue;
            }
            if (TryFindOptionValue("/IncompleteFeedbackTag", args, out optionValue))
            {
                IncompleteFeedbackTag = optionValue;
            }
            if (TryFindOptionValue("/TfsProject", args, out optionValue))
            {
                TfsProject = optionValue;
            }
            if (TryFindOptionValue("/TfsQueryHierarchy", args, out optionValue))
            {
                TfsQueryHierarchy = optionValue;
            }
            if (TryFindOptionValue("/TfsQueryName", args, out optionValue))
            {
                TfsQueryName = optionValue;
            }
            if (TryFindOptionValue("/TfsServerUrl", args, out optionValue))
            {
                TfsServerUrl = optionValue;
            }
        }

        private static bool TryFindOptionValue(string name, string[] args, out string optionValue)
        {
            optionValue = null;
            string option = args.FirstOrDefault(x => x.StartsWith(name, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(option))
            {
                optionValue = option.Split('=')[1].Trim();
                return true;
            }
            return false;
        }

        #endregion
    }
}