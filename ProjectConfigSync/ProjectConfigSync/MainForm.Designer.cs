namespace ProjectConfigSync
{
    partial class Form1
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmnuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmnuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Project = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Configuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Platform = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OutputPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlatformTarget = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DebugType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DebugSymbols = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DefineConstants = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AllowUnsafeBlocks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tmnuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tmnuPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tmnuCopyToAllRows = new System.Windows.Forms.ToolStripMenuItem();
            this.cboProjects = new System.Windows.Forms.ComboBox();
            this.cboConfigurations = new System.Windows.Forms.ComboBox();
            this.cboPlatforms = new System.Windows.Forms.ComboBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmnuFile});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(646, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmnuFile
            // 
            this.tsmnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmnuOpen,
            this.tsmnuSave,
            this.toolStripMenuItem1,
            this.tsmnuExit});
            this.tsmnuFile.Name = "tsmnuFile";
            this.tsmnuFile.Size = new System.Drawing.Size(37, 20);
            this.tsmnuFile.Text = "File";
            // 
            // tsmnuOpen
            // 
            this.tsmnuOpen.Name = "tsmnuOpen";
            this.tsmnuOpen.Size = new System.Drawing.Size(192, 22);
            this.tsmnuOpen.Text = "Open Project/Solution";
            this.tsmnuOpen.Click += new System.EventHandler(this.tsmnuOpen_Click);
            // 
            // tsmnuSave
            // 
            this.tsmnuSave.Enabled = false;
            this.tsmnuSave.Name = "tsmnuSave";
            this.tsmnuSave.Size = new System.Drawing.Size(192, 22);
            this.tsmnuSave.Text = "Save";
            this.tsmnuSave.Click += new System.EventHandler(this.tsmnuSave_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(189, 6);
            // 
            // tsmnuExit
            // 
            this.tsmnuExit.Name = "tsmnuExit";
            this.tsmnuExit.Size = new System.Drawing.Size(192, 22);
            this.tsmnuExit.Text = "Exit";
            this.tsmnuExit.Click += new System.EventHandler(this.tsmnuExit_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Project,
            this.Configuration,
            this.Platform,
            this.OutputPath,
            this.PlatformTarget,
            this.DebugType,
            this.DebugSymbols,
            this.DefineConstants,
            this.AllowUnsafeBlocks});
            this.dataGridView1.Location = new System.Drawing.Point(12, 54);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(622, 304);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDown);
            // 
            // Project
            // 
            this.Project.DataPropertyName = "Project";
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Silver;
            this.Project.DefaultCellStyle = dataGridViewCellStyle1;
            this.Project.Frozen = true;
            this.Project.HeaderText = "Project";
            this.Project.Name = "Project";
            this.Project.ReadOnly = true;
            // 
            // Configuration
            // 
            this.Configuration.DataPropertyName = "Configuration";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Silver;
            this.Configuration.DefaultCellStyle = dataGridViewCellStyle2;
            this.Configuration.Frozen = true;
            this.Configuration.HeaderText = "Configuration";
            this.Configuration.Name = "Configuration";
            this.Configuration.ReadOnly = true;
            // 
            // Platform
            // 
            this.Platform.DataPropertyName = "Platform";
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Silver;
            this.Platform.DefaultCellStyle = dataGridViewCellStyle3;
            this.Platform.Frozen = true;
            this.Platform.HeaderText = "Platform";
            this.Platform.Name = "Platform";
            this.Platform.ReadOnly = true;
            // 
            // OutputPath
            // 
            this.OutputPath.DataPropertyName = "OutputPath";
            this.OutputPath.HeaderText = "Output Path";
            this.OutputPath.Name = "OutputPath";
            this.OutputPath.ReadOnly = true;
            // 
            // PlatformTarget
            // 
            this.PlatformTarget.DataPropertyName = "PlatformTarget";
            this.PlatformTarget.HeaderText = "Platform Target";
            this.PlatformTarget.Name = "PlatformTarget";
            this.PlatformTarget.ReadOnly = true;
            // 
            // DebugType
            // 
            this.DebugType.DataPropertyName = "DebugType";
            this.DebugType.HeaderText = "Debug Type";
            this.DebugType.Name = "DebugType";
            this.DebugType.ReadOnly = true;
            // 
            // DebugSymbols
            // 
            this.DebugSymbols.DataPropertyName = "DebugSymbols";
            this.DebugSymbols.HeaderText = "Debug Symbols";
            this.DebugSymbols.Name = "DebugSymbols";
            this.DebugSymbols.ReadOnly = true;
            // 
            // DefineConstants
            // 
            this.DefineConstants.DataPropertyName = "DefineConstants";
            this.DefineConstants.HeaderText = "Define Constants";
            this.DefineConstants.Name = "DefineConstants";
            this.DefineConstants.ReadOnly = true;
            // 
            // AllowUnsafeBlocks
            // 
            this.AllowUnsafeBlocks.DataPropertyName = "AllowUnsafeBlocks";
            this.AllowUnsafeBlocks.HeaderText = "Allow Unsafe Blocks";
            this.AllowUnsafeBlocks.Name = "AllowUnsafeBlocks";
            this.AllowUnsafeBlocks.ReadOnly = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmnuCopy,
            this.tmnuPaste,
            this.toolStripSeparator1,
            this.tmnuCopyToAllRows});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(196, 76);
            // 
            // tmnuCopy
            // 
            this.tmnuCopy.Name = "tmnuCopy";
            this.tmnuCopy.Size = new System.Drawing.Size(195, 22);
            this.tmnuCopy.Text = "Copy";
            this.tmnuCopy.Click += new System.EventHandler(this.tmnuCopy_Click);
            // 
            // tmnuPaste
            // 
            this.tmnuPaste.Name = "tmnuPaste";
            this.tmnuPaste.Size = new System.Drawing.Size(195, 22);
            this.tmnuPaste.Text = "Paste";
            this.tmnuPaste.Click += new System.EventHandler(this.tmnuPaste_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(192, 6);
            // 
            // tmnuCopyToAllRows
            // 
            this.tmnuCopyToAllRows.Name = "tmnuCopyToAllRows";
            this.tmnuCopyToAllRows.Size = new System.Drawing.Size(195, 22);
            this.tmnuCopyToAllRows.Text = "Copy to all visible rows";
            this.tmnuCopyToAllRows.Click += new System.EventHandler(this.tmnuCopyToAllRows_Click);
            // 
            // cboProjects
            // 
            this.cboProjects.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProjects.FormattingEnabled = true;
            this.cboProjects.Location = new System.Drawing.Point(12, 27);
            this.cboProjects.Name = "cboProjects";
            this.cboProjects.Size = new System.Drawing.Size(172, 21);
            this.cboProjects.TabIndex = 1;
            this.cboProjects.SelectedIndexChanged += new System.EventHandler(this.cboFilters_SelectedIndexChanged);
            // 
            // cboConfigurations
            // 
            this.cboConfigurations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboConfigurations.FormattingEnabled = true;
            this.cboConfigurations.Location = new System.Drawing.Point(190, 27);
            this.cboConfigurations.Name = "cboConfigurations";
            this.cboConfigurations.Size = new System.Drawing.Size(172, 21);
            this.cboConfigurations.TabIndex = 2;
            this.cboConfigurations.SelectedIndexChanged += new System.EventHandler(this.cboFilters_SelectedIndexChanged);
            // 
            // cboPlatforms
            // 
            this.cboPlatforms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPlatforms.FormattingEnabled = true;
            this.cboPlatforms.Location = new System.Drawing.Point(368, 27);
            this.cboPlatforms.Name = "cboPlatforms";
            this.cboPlatforms.Size = new System.Drawing.Size(172, 21);
            this.cboPlatforms.TabIndex = 3;
            this.cboPlatforms.SelectedIndexChanged += new System.EventHandler(this.cboFilters_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 370);
            this.Controls.Add(this.cboPlatforms);
            this.Controls.Add(this.cboConfigurations);
            this.Controls.Add(this.cboProjects);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Project Configuration Sync";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmnuFile;
        private System.Windows.Forms.ToolStripMenuItem tsmnuOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsmnuExit;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tmnuCopy;
        private System.Windows.Forms.ToolStripMenuItem tmnuPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tmnuCopyToAllRows;
        private System.Windows.Forms.ToolStripMenuItem tsmnuSave;
        private System.Windows.Forms.ComboBox cboProjects;
        private System.Windows.Forms.ComboBox cboConfigurations;
        private System.Windows.Forms.ComboBox cboPlatforms;
        private System.Windows.Forms.DataGridViewTextBoxColumn Project;
        private System.Windows.Forms.DataGridViewTextBoxColumn Configuration;
        private System.Windows.Forms.DataGridViewTextBoxColumn Platform;
        private System.Windows.Forms.DataGridViewTextBoxColumn OutputPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlatformTarget;
        private System.Windows.Forms.DataGridViewTextBoxColumn DebugType;
        private System.Windows.Forms.DataGridViewTextBoxColumn DebugSymbols;
        private System.Windows.Forms.DataGridViewTextBoxColumn DefineConstants;
        private System.Windows.Forms.DataGridViewTextBoxColumn AllowUnsafeBlocks;
    }
}

