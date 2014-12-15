using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace ReleaseNotes.TfsTool
{
// ReSharper disable InconsistentNaming
    public partial class frmReleaseNotesGenerator : Form
// ReSharper restore InconsistentNaming
    {
        private CommandLineOptions _options;

        public frmReleaseNotesGenerator()
        {
            InitializeComponent();
        }

        #region Properties

        public CommandLineOptions Options
        {
            get { return _options; }
            set
            {
                _options = value;
                OptionsBindingSource.DataSource = _options;
            }
        }

        #endregion

        #region Methods

        private void BuildQueryHierarchy(ProjectInfo project, QueryHierarchy queryHierarchy)
        {
            tvwQueries.Nodes.Clear();

            var root = new TreeNode(project.Name);
            foreach (QueryFolder query in queryHierarchy)
            {
                DefineFolder(query, root);
            }

            tvwQueries.Nodes.Add(root);
        }

        private void DefineFolder(QueryFolder query, TreeNode parent)
        {
            var item = new TreeNode { Text = query.Name, Tag = query };
            parent.Nodes.Add(item);

            foreach (QueryItem subQuery in query)
            {
                if (subQuery.GetType() == typeof (QueryFolder))
                    DefineFolder((QueryFolder) subQuery, item);
                else
                    DefineQuery((QueryDefinition) subQuery, item);
            }
        }

        private void DefineQuery(QueryDefinition query, TreeNode queryFolder)
        {
            var item = new TreeNode { Text = query.Name, Tag = query };
            queryFolder.Nodes.Add(item);
        }

        private string GetExportFile(string defaultExtension, string filter)
        {
            exportFileDialog.CheckPathExists = true;
            exportFileDialog.OverwritePrompt = true;
            exportFileDialog.DefaultExt = defaultExtension;
            exportFileDialog.FileName = Options.ExportFile;
            exportFileDialog.Filter = filter;
            exportFileDialog.FilterIndex = 1;

            if (exportFileDialog.ShowDialog() == DialogResult.OK)
                return exportFileDialog.FileName;

            return null;
        }

        #endregion

        #region Event Handlers

        private void btnNewReleaseMergeFile_Click(object sender, EventArgs e)
        {
            mergeFileDialog.CheckPathExists = true;
            mergeFileDialog.FileName = Options.MergeReleaseNotesFile;
            mergeFileDialog.FilterIndex = 1;

            if (mergeFileDialog.ShowDialog() == DialogResult.OK)
            {
                Options.MergeReleaseNotesFile = mergeFileDialog.FileName;
                txtMergeReleaseFile.Text = Options.MergeReleaseNotesFile;
            }
        }

        private void btnReleaseNotes_Click(object sender, EventArgs e)
        {
            QueryDefinition query = ValidateQueryNode();

            if (query != null)
            {
                Options.TfsQuery = query;

                string exportFile = GetExportFile(".pdf", "Pdf files (*.pdf)|*.pdf");

                if (!string.IsNullOrEmpty(exportFile))
                {
                    Options.ExportFile = exportFile;

                    var reportRunner = new ReportRunner();
                    reportRunner.Generate(Options);

                    // Launch the generated document
                    if (Options.Action == AppAction.View)
                        Process.Start(exportFile);
                }                
            }
        }

        private void btnTeamProjectPicker_Click(object sender, EventArgs e)
        {
            TfsTeamProjectCollection tpc;
            using (var picker = new TeamProjectPicker(TeamProjectPickerMode.SingleProject, false))
            {
                var result = picker.ShowDialog();
                if (result == DialogResult.OK)
                {
                    tpc = picker.SelectedTeamProjectCollection;
                    Options.TfsServerUrl = tpc.Uri.ToString();
                    txtTfsUrl.Text = Options.TfsServerUrl;
                    Options.TfsProject = picker.SelectedProjects[0].Name;
                    txtTfsProject.Text = Options.TfsProject;

                    var tfs = picker.SelectedTeamProjectCollection;
                    var projInfo = picker.SelectedProjects[0];
                    var store = tfs.GetService<WorkItemStore>();

                    BuildQueryHierarchy(projInfo, store.Projects[projInfo.Name].QueryHierarchy);
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Console.WriteLine("Query Node selected: {0}", e.Node.Text);
        }

        private QueryDefinition ValidateQueryNode()
        {
            if (IsQueryNodeSelected() && IsQueryNodeNotFolder() && IsQueryNodeSimpleListView())
            {
                return (QueryDefinition) tvwQueries.SelectedNode.Tag;
            }
            return null;
        }

        private bool IsQueryNodeSelected()
        {
            if (tvwQueries.SelectedNode == null || tvwQueries.SelectedNode.Tag == null)
            {
                MessageBox.Show("Please select a Tfs Query to use for the Release Notes", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool IsQueryNodeNotFolder()
        {
            var selectedNode = tvwQueries.SelectedNode;
            if (selectedNode.Tag is QueryFolder)
            {
                MessageBox.Show("Please select a Tfs Query that is not a folder to use for the Release Notes", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool IsQueryNodeSimpleListView()
        {
            var selectedNode = tvwQueries.SelectedNode;
            QueryDefinition query = (QueryDefinition)selectedNode.Tag;
            if (query.QueryType != QueryType.List)
            {
                MessageBox.Show("Please select a Tfs Query that returns a flat list of work items to use for the Release Notes", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        #endregion
    }
}