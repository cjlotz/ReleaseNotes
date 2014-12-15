namespace ReleaseNotes.TfsTool
{
    partial class frmReleaseNotesGenerator
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReleaseNotesGenerator));
            this.grpTfsSettings = new System.Windows.Forms.GroupBox();
            this.tvwQueries = new System.Windows.Forms.TreeView();
            this.btnTeamProjectPicker = new System.Windows.Forms.Button();
            this.lblTfsQueryHierarchy = new System.Windows.Forms.Label();
            this.txtTfsProject = new System.Windows.Forms.TextBox();
            this.lblTfsProject = new System.Windows.Forms.Label();
            this.txtTfsUrl = new System.Windows.Forms.TextBox();
            this.lblTfsServerUrl = new System.Windows.Forms.Label();
            this.btnReleaseNotes = new System.Windows.Forms.Button();
            this.mergeFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.exportFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.lblBuildNumber = new System.Windows.Forms.Label();
            this.txtBuildNumber = new System.Windows.Forms.TextBox();
            this.lblMergeReleaseFile = new System.Windows.Forms.Label();
            this.txtMergeReleaseFile = new System.Windows.Forms.TextBox();
            this.btnNewReleaseMergeFile = new System.Windows.Forms.Button();
            this.grpReportSettings = new System.Windows.Forms.GroupBox();
            this.chkLinkWorkItems = new System.Windows.Forms.CheckBox();
            this.OptionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.grpTfsSettings.SuspendLayout();
            this.grpReportSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OptionsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // grpTfsSettings
            // 
            this.grpTfsSettings.Controls.Add(this.tvwQueries);
            this.grpTfsSettings.Controls.Add(this.btnTeamProjectPicker);
            this.grpTfsSettings.Controls.Add(this.lblTfsQueryHierarchy);
            this.grpTfsSettings.Controls.Add(this.txtTfsProject);
            this.grpTfsSettings.Controls.Add(this.lblTfsProject);
            this.grpTfsSettings.Controls.Add(this.txtTfsUrl);
            this.grpTfsSettings.Controls.Add(this.lblTfsServerUrl);
            this.grpTfsSettings.Location = new System.Drawing.Point(21, 12);
            this.grpTfsSettings.Name = "grpTfsSettings";
            this.grpTfsSettings.Size = new System.Drawing.Size(735, 385);
            this.grpTfsSettings.TabIndex = 0;
            this.grpTfsSettings.TabStop = false;
            this.grpTfsSettings.Text = "TFS Settings";
            // 
            // tvwQueries
            // 
            this.tvwQueries.Location = new System.Drawing.Point(10, 62);
            this.tvwQueries.Name = "tvwQueries";
            this.tvwQueries.Size = new System.Drawing.Size(715, 310);
            this.tvwQueries.TabIndex = 8;
            this.tvwQueries.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // btnTeamProjectPicker
            // 
            this.btnTeamProjectPicker.Location = new System.Drawing.Point(334, 16);
            this.btnTeamProjectPicker.Name = "btnTeamProjectPicker";
            this.btnTeamProjectPicker.Size = new System.Drawing.Size(28, 20);
            this.btnTeamProjectPicker.TabIndex = 7;
            this.btnTeamProjectPicker.Text = "...";
            this.btnTeamProjectPicker.UseVisualStyleBackColor = true;
            this.btnTeamProjectPicker.Click += new System.EventHandler(this.btnTeamProjectPicker_Click);
            // 
            // lblTfsQueryHierarchy
            // 
            this.lblTfsQueryHierarchy.AutoSize = true;
            this.lblTfsQueryHierarchy.Location = new System.Drawing.Point(7, 46);
            this.lblTfsQueryHierarchy.Name = "lblTfsQueryHierarchy";
            this.lblTfsQueryHierarchy.Size = new System.Drawing.Size(83, 13);
            this.lblTfsQueryHierarchy.TabIndex = 4;
            this.lblTfsQueryHierarchy.Text = "Query Hierarchy";
            // 
            // txtTfsProject
            // 
            this.txtTfsProject.Location = new System.Drawing.Point(467, 20);
            this.txtTfsProject.Name = "txtTfsProject";
            this.txtTfsProject.ReadOnly = true;
            this.txtTfsProject.Size = new System.Drawing.Size(258, 20);
            this.txtTfsProject.TabIndex = 1;
            // 
            // lblTfsProject
            // 
            this.lblTfsProject.AutoSize = true;
            this.lblTfsProject.Location = new System.Drawing.Point(379, 23);
            this.lblTfsProject.Name = "lblTfsProject";
            this.lblTfsProject.Size = new System.Drawing.Size(71, 13);
            this.lblTfsProject.TabIndex = 2;
            this.lblTfsProject.Text = "Project Name";
            // 
            // txtTfsUrl
            // 
            this.txtTfsUrl.Location = new System.Drawing.Point(95, 17);
            this.txtTfsUrl.Name = "txtTfsUrl";
            this.txtTfsUrl.ReadOnly = true;
            this.txtTfsUrl.Size = new System.Drawing.Size(233, 20);
            this.txtTfsUrl.TabIndex = 0;
            // 
            // lblTfsServerUrl
            // 
            this.lblTfsServerUrl.AutoSize = true;
            this.lblTfsServerUrl.Location = new System.Drawing.Point(7, 20);
            this.lblTfsServerUrl.Name = "lblTfsServerUrl";
            this.lblTfsServerUrl.Size = new System.Drawing.Size(54, 13);
            this.lblTfsServerUrl.TabIndex = 0;
            this.lblTfsServerUrl.Text = "Server Url";
            // 
            // btnReleaseNotes
            // 
            this.btnReleaseNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReleaseNotes.Image = global::ReleaseNotes.TfsTool.Properties.Resources.text_binary;
            this.btnReleaseNotes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReleaseNotes.Location = new System.Drawing.Point(602, 488);
            this.btnReleaseNotes.Name = "btnReleaseNotes";
            this.btnReleaseNotes.Size = new System.Drawing.Size(154, 34);
            this.btnReleaseNotes.TabIndex = 2;
            this.btnReleaseNotes.Text = "&Generate Release Notes";
            this.btnReleaseNotes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnReleaseNotes.UseVisualStyleBackColor = true;
            this.btnReleaseNotes.Click += new System.EventHandler(this.btnReleaseNotes_Click);
            // 
            // lblBuildNumber
            // 
            this.lblBuildNumber.AutoSize = true;
            this.lblBuildNumber.Location = new System.Drawing.Point(7, 22);
            this.lblBuildNumber.Name = "lblBuildNumber";
            this.lblBuildNumber.Size = new System.Drawing.Size(70, 13);
            this.lblBuildNumber.TabIndex = 2;
            this.lblBuildNumber.Text = "Build Number";
            // 
            // txtBuildNumber
            // 
            this.txtBuildNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.OptionsBindingSource, "BuildNumber", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtBuildNumber.Location = new System.Drawing.Point(128, 19);
            this.txtBuildNumber.Name = "txtBuildNumber";
            this.txtBuildNumber.Size = new System.Drawing.Size(144, 20);
            this.txtBuildNumber.TabIndex = 1;
            // 
            // lblMergeReleaseFile
            // 
            this.lblMergeReleaseFile.AutoSize = true;
            this.lblMergeReleaseFile.Location = new System.Drawing.Point(7, 49);
            this.lblMergeReleaseFile.Name = "lblMergeReleaseFile";
            this.lblMergeReleaseFile.Size = new System.Drawing.Size(110, 13);
            this.lblMergeReleaseFile.TabIndex = 9;
            this.lblMergeReleaseFile.Text = "Merge Release Notes";
            // 
            // txtMergeReleaseFile
            // 
            this.txtMergeReleaseFile.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.OptionsBindingSource, "MergeReleaseNotesFile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtMergeReleaseFile.Location = new System.Drawing.Point(128, 46);
            this.txtMergeReleaseFile.Name = "txtMergeReleaseFile";
            this.txtMergeReleaseFile.Size = new System.Drawing.Size(563, 20);
            this.txtMergeReleaseFile.TabIndex = 5;
            // 
            // btnNewReleaseMergeFile
            // 
            this.btnNewReleaseMergeFile.Location = new System.Drawing.Point(697, 45);
            this.btnNewReleaseMergeFile.Name = "btnNewReleaseMergeFile";
            this.btnNewReleaseMergeFile.Size = new System.Drawing.Size(28, 20);
            this.btnNewReleaseMergeFile.TabIndex = 6;
            this.btnNewReleaseMergeFile.Text = "...";
            this.btnNewReleaseMergeFile.UseVisualStyleBackColor = true;
            this.btnNewReleaseMergeFile.Click += new System.EventHandler(this.btnNewReleaseMergeFile_Click);
            // 
            // grpReportSettings
            // 
            this.grpReportSettings.Controls.Add(this.chkLinkWorkItems);
            this.grpReportSettings.Controls.Add(this.btnNewReleaseMergeFile);
            this.grpReportSettings.Controls.Add(this.txtMergeReleaseFile);
            this.grpReportSettings.Controls.Add(this.lblMergeReleaseFile);
            this.grpReportSettings.Controls.Add(this.txtBuildNumber);
            this.grpReportSettings.Controls.Add(this.lblBuildNumber);
            this.grpReportSettings.Location = new System.Drawing.Point(21, 403);
            this.grpReportSettings.Name = "grpReportSettings";
            this.grpReportSettings.Size = new System.Drawing.Size(735, 79);
            this.grpReportSettings.TabIndex = 1;
            this.grpReportSettings.TabStop = false;
            this.grpReportSettings.Text = "Report Settings";
            // 
            // chkLinkWorkItems
            // 
            this.chkLinkWorkItems.AutoSize = true;
            this.chkLinkWorkItems.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.OptionsBindingSource, "LinkWorkItems", true));
            this.chkLinkWorkItems.Location = new System.Drawing.Point(467, 21);
            this.chkLinkWorkItems.Name = "chkLinkWorkItems";
            this.chkLinkWorkItems.Size = new System.Drawing.Size(103, 17);
            this.chkLinkWorkItems.TabIndex = 10;
            this.chkLinkWorkItems.Text = "Link Work Items";
            this.chkLinkWorkItems.UseVisualStyleBackColor = true;
            // 
            // OptionsBindingSource
            // 
            this.OptionsBindingSource.DataSource = typeof(ReleaseNotes.TfsTool.CommandLineOptions);
            // 
            // frmReleaseNotesGenerator
            // 
            this.ClientSize = new System.Drawing.Size(773, 534);
            this.Controls.Add(this.grpReportSettings);
            this.Controls.Add(this.grpTfsSettings);
            this.Controls.Add(this.btnReleaseNotes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmReleaseNotesGenerator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Release Notes Generator";
            this.grpTfsSettings.ResumeLayout(false);
            this.grpTfsSettings.PerformLayout();
            this.grpReportSettings.ResumeLayout(false);
            this.grpReportSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OptionsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpTfsSettings;
        private System.Windows.Forms.Label lblTfsQueryHierarchy;
        private System.Windows.Forms.TextBox txtTfsProject;
        private System.Windows.Forms.Label lblTfsProject;
        private System.Windows.Forms.TextBox txtTfsUrl;
        private System.Windows.Forms.Label lblTfsServerUrl;
        private System.Windows.Forms.Button btnReleaseNotes;
        private System.Windows.Forms.OpenFileDialog mergeFileDialog;
        private System.Windows.Forms.SaveFileDialog exportFileDialog;
        private System.Windows.Forms.Label lblBuildNumber;
        private System.Windows.Forms.TextBox txtBuildNumber;
        private System.Windows.Forms.Label lblMergeReleaseFile;
        private System.Windows.Forms.TextBox txtMergeReleaseFile;
        private System.Windows.Forms.Button btnNewReleaseMergeFile;
        private System.Windows.Forms.GroupBox grpReportSettings;
        private System.Windows.Forms.Button btnTeamProjectPicker;
        private System.Windows.Forms.CheckBox chkLinkWorkItems;
        private System.Windows.Forms.TreeView tvwQueries;
        private System.Windows.Forms.BindingSource OptionsBindingSource;
    }
}

