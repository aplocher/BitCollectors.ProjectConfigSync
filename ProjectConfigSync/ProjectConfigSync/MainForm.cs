using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectConfigSync.Controls;
using ProjectConfigSync.Entities;
using ProjectConfigSync.Helpers;
using Timer = System.Windows.Forms.Timer;

namespace ProjectConfigSync
{
    public partial class MainForm : Form
    {
        private const string FormTitle = "Project Config Sync";
        private const string FormTitleWithFile = FormTitle + " - {0}";

        private readonly RowCountUserControl _rowCountUserControl;
        private readonly Timer _animateRowCountControlTimer = new Timer();

        private DateTime _rowCountDetailsShownDateTime = DateTime.MinValue;
        private Size _rowCountUserControlSize;

        private bool _suppressTextChangedEvents = false;

        public MainForm()
        {
            InitializeComponent();

            projectConfigDataGridView1.FiltersChanged += (sender, args) =>
                {
                    _suppressTextChangedEvents = true;

                    try
                    {
                        if (cboProjects.Text != args.ProjectFilter)
                        {
                            cboProjects.Text = args.ProjectFilter;
                            this.RefreshProjectFilterCombo();
                        }

                        if (cboConfigurations.Text != args.ConfigurationFilter)
                        {
                            cboConfigurations.Text = args.ConfigurationFilter;
                            this.RefreshConfigFilterCombo();
                        }

                        if (cboPlatforms.Text != args.PlatformFilter)
                        {
                            cboPlatforms.Text = args.PlatformFilter;
                            this.RefreshPlatformFilterCombo();
                        }
                    }
                    finally
                    {
                        _suppressTextChangedEvents = false;
                    }
                };

            this.ChangeFormTitle();

            cboConfigurations.Items.Add(ProjectConfigList.AllConfigurationsComboText);
            cboConfigurations.SelectedIndex = 0;
            cboConfigurations.Text = ProjectConfigList.AllConfigurationsComboText;
            cboConfigurations.GotFocus += (sender, args) => cboConfigurations.ForeColor = Color.Black;
            cboConfigurations.LostFocus += (sender, args) => this.RefreshConfigFilterCombo();
            cboConfigurations.PreviewKeyDown += (sender, args) => ConfigFilterPreviewKeyDown(sender, args, ProjectConfigList.AllConfigurationsComboText);

            cboPlatforms.Items.Add(ProjectConfigList.AllPlatformsComboText);
            cboPlatforms.SelectedIndex = 0;
            cboPlatforms.Text = ProjectConfigList.AllPlatformsComboText;
            cboPlatforms.GotFocus += (sender, args) => cboPlatforms.ForeColor = Color.Black;
            cboPlatforms.LostFocus += (sender, args) => this.RefreshPlatformFilterCombo();
            cboPlatforms.PreviewKeyDown += (sender, args) => ConfigFilterPreviewKeyDown(sender, args, ProjectConfigList.AllPlatformsComboText);

            cboProjects.Items.Add(ProjectConfigList.AllProjectsComboText);
            cboProjects.SelectedIndex = 0;
            cboProjects.Text = ProjectConfigList.AllProjectsComboText;
            cboProjects.GotFocus += (sender, args) => cboProjects.ForeColor = Color.Black;
            cboProjects.LostFocus += (sender, args) => this.RefreshProjectFilterCombo();
            cboProjects.PreviewKeyDown += (sender, args) => ConfigFilterPreviewKeyDown(sender, args, ProjectConfigList.AllProjectsComboText);

            this._rowCountUserControl = new RowCountUserControl();

            this._rowCountUserControl.MouseLeave += RowCountUserControlMouseLeave;
            this._rowCountUserControl.Hide();
            this._rowCountUserControlSize = _rowCountUserControl.Size;
            this._rowCountUserControl.Size = new Size(this._rowCountUserControl.Width, 1);

            this.Controls.Add(this._rowCountUserControl);

            this.projectConfigDataGridView1.RowCountsChanged += (sender, args) =>
                {
                    lblRowCounts.Text = string.Format("Visible: {0}, Total: {1}", projectConfigDataGridView1.TotalVisibleCount, projectConfigDataGridView1.TotalRowCount);
                    lblRowCounts.Location = new Point(this.ClientSize.Width - lblRowCounts.Size.Width - 7, lblRowCounts.Location.Y);
                };

            this._animateRowCountControlTimer.Interval = 1;
            this._animateRowCountControlTimer.Tick += (sender, args) =>
                {
                    if (_rowCountUserControl.Height < _rowCountUserControlSize.Height)
                    {
                        int newHeight = _rowCountUserControl.Height + 15;
                        if (newHeight > _rowCountUserControlSize.Height)
                        {
                            newHeight = _rowCountUserControlSize.Height;
                        }

                        _rowCountUserControl.Size = new Size(_rowCountUserControl.Width, newHeight);
                        _rowCountUserControl.Refresh();
                    }
                    else if (this._animateRowCountControlTimer.Interval == 1)
                    {
                        if (this.IsMouseInRowCountUsercontrol)
                        {
                            this._animateRowCountControlTimer.Interval = 400;
                        }
                        else
                        {
                            this.HideRowCountDetails();
                        }
                    }
                    else if (!this.IsMouseInRowCountUsercontrol)
                    {
                        this.HideRowCountDetails();
                    }
                };
        }

        private void ConfigFilterPreviewKeyDown(object sender, PreviewKeyDownEventArgs args, string noneSelectedText)
        {
            Keys k = args.KeyCode;

            bool isAlphaNum = false;

            if (k != Keys.Back)
            {
                bool isAlpha = !args.Alt && !args.Control && (k >= Keys.A && k <= Keys.Z);
                bool d0d9 = k >= Keys.D0 && k <= Keys.D9;
                bool numpad = k >= Keys.NumPad0 && k <= Keys.NumPad9;
                bool isNumber = (!args.Alt && !args.Control && !args.Shift) && (d0d9 || numpad);

                isAlphaNum = isAlpha || isNumber;
            }

            var combo = (sender as ComboBox);

            if (!isAlphaNum || combo == null)
            {
                return;
            }

            combo.ForeColor = Color.Black;
            if (string.Equals(combo.Text, noneSelectedText, StringComparison.CurrentCultureIgnoreCase))
            {
                combo.Text = "";
            }
        }


        private void RefreshProjectFilterCombo()
        {
            if (string.IsNullOrEmpty(cboProjects.Text) || cboProjects.Text.Equals(ProjectConfigList.AllProjectsComboText, StringComparison.InvariantCultureIgnoreCase))
            {
                cboProjects.Text = ProjectConfigList.AllProjectsComboText;
                cboProjects.ForeColor = Color.LightSlateGray;
                cboProjects.Refresh();
            }
            else
            {
                cboProjects.ForeColor = Color.Black;
            }
        }

        private void RefreshConfigFilterCombo()
        {
            if (string.IsNullOrEmpty(cboConfigurations.Text) || cboConfigurations.Text.Equals(ProjectConfigList.AllConfigurationsComboText, StringComparison.InvariantCultureIgnoreCase))
            {
                cboConfigurations.Text = ProjectConfigList.AllConfigurationsComboText;
                cboConfigurations.ForeColor = Color.LightSlateGray;
                cboConfigurations.Refresh();
            }
            else
            {
                cboConfigurations.ForeColor = Color.Black;
            }
        }

        private void RefreshPlatformFilterCombo()
        {
            if (string.IsNullOrEmpty(cboPlatforms.Text) || cboPlatforms.Text.Equals(ProjectConfigList.AllPlatformsComboText, StringComparison.InvariantCultureIgnoreCase))
            {
                cboPlatforms.Text = ProjectConfigList.AllPlatformsComboText;
                cboPlatforms.ForeColor = Color.LightSlateGray;
                cboPlatforms.Refresh();
            }
            else
            {
                cboPlatforms.ForeColor = Color.Black;
            }
        }

        private ProjectConfigList _projectConfigList = null;

        #region Event Handlers
        private void tsmnuOpen_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "(*.sln;*.csproj)|*.sln;*.csproj";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    tstrProgressBar.Style = ProgressBarStyle.Marquee;
                    tstrProgressBar.MarqueeAnimationSpeed = 40;
                    tstrProgressBar.Enabled = true;
                    tstrProgressBar.Visible = true;

                    Task.Factory.StartNew(() => FileHelper.LoadFile(dialog.FileName)).ContinueWith(task =>
                        {
                            Thread.Sleep(400);

                            Action invokeGrid = () =>
                                {
                                    _projectConfigList = task.Result;
                                    projectConfigDataGridView1.DataSource = _projectConfigList;

                                    cboConfigurations.DataSource = task.Result.UniqueConfigurations;
                                    cboProjects.DataSource = task.Result.UniqueProjects;
                                    cboPlatforms.DataSource = task.Result.UniquePlatforms;

                                    Console.WriteLine(DateTime.Now.Ticks);

                                    projectConfigDataGridView1.AutoResizeColumn(0, DataGridViewAutoSizeColumnMode.DisplayedCells);
                                    projectConfigDataGridView1.AutoResizeColumn(1, DataGridViewAutoSizeColumnMode.DisplayedCells);
                                    projectConfigDataGridView1.AutoResizeColumn(2, DataGridViewAutoSizeColumnMode.DisplayedCells);

                                    Console.WriteLine(DateTime.Now.Ticks);

                                    tsmnuSave.Enabled = true;

                                    tstrProgressBar.Visible = false;
                                    tstrProgressBar.Enabled = false;
                                    tstrProgressBar.Style = ProgressBarStyle.Continuous;
                                    tstrProgressBar.MarqueeAnimationSpeed = 0;
                                };

                            if (projectConfigDataGridView1.InvokeRequired)
                            {
                                projectConfigDataGridView1.BeginInvoke(new MethodInvoker(invokeGrid));
                            }
                            else
                            {
                                invokeGrid();
                            }
                        });
                }
            }
        }

        private void ChangeFormTitle(string filename = null)
        {
            if (string.IsNullOrEmpty(filename))
            {
                this.Text = FormTitle;
            }
            else
            {
                FileInfo fileInfo = new FileInfo(filename);
                this.Text = string.Format(FormTitleWithFile, fileInfo.Name);
            }
        }

        private void FilterComboBoxTextChanged(object sender, EventArgs e)
        {
            if (this._suppressTextChangedEvents)
            {
                return;
            }

            this.UpdateFilterState();
        }

        private void UpdateFilterState()
        {
            string projectFilter = null;
            if (!string.IsNullOrWhiteSpace(cboProjects.Text) && cboProjects.Text.Trim() != ProjectConfigList.AllProjectsComboText)
            {
                projectFilter = cboProjects.Text.Trim();
            }

            string configFilter = null;
            if (!string.IsNullOrWhiteSpace(cboConfigurations.Text) && cboConfigurations.Text.Trim() != ProjectConfigList.AllConfigurationsComboText)
            {
                configFilter = cboConfigurations.Text.Trim();
            }

            string platformFilter = null;
            if (!string.IsNullOrWhiteSpace(cboPlatforms.Text) && cboPlatforms.Text.Trim() != ProjectConfigList.AllPlatformsComboText)
            {
                platformFilter = cboPlatforms.Text.Trim();
            }

            btnDisableFilters.Enabled = !string.IsNullOrEmpty(platformFilter) ||
                                        !string.IsNullOrEmpty(configFilter) ||
                                        !string.IsNullOrEmpty(projectFilter);

            projectConfigDataGridView1.SetFilters(projectFilter, configFilter, platformFilter);
        }

        private void LabelRowCountMouseEnter(object sender, EventArgs e)
        {
            lblRowCounts.BackColor = Color.LightSlateGray;

            Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(400);
                    Action action = () =>
                        {
                            this._rowCountUserControl.Size = new Size(this._rowCountUserControl.Width, 1);
                            this._animateRowCountControlTimer.Interval = 1;
                            this._animateRowCountControlTimer.Start();

                            var mouseToLabel = lblRowCounts.PointToClient(Cursor.Position);
                            var labelRectangle = lblRowCounts.ClientRectangle;
                            labelRectangle.Inflate(4, 4);
                            var labelContainsMouse = labelRectangle.Contains(mouseToLabel);

                            if (!labelContainsMouse)
                            {
                                return;
                            }

                            this._rowCountUserControl.Anchor = AnchorStyles.None;
                            this._rowCountUserControl.Location = new Point(this.Width - this._rowCountUserControl.Width - 22, this.lblRowCounts.Location.Y + 2);
                            this._rowCountUserControl.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                            this._rowCountUserControl.BringToFront();

                            if (this._rowCountUserControl.Visible)
                            {
                                return;
                            }

                            this._rowCountUserControl.Show();
                            this._rowCountUserControl.Visible = true;
                            this._rowCountDetailsShownDateTime = DateTime.Now;
                            this._rowCountUserControl.BringToFront();
                        };

                    if (!_rowCountUserControl.Visible && (IsMouseInRowCountLabel || IsMouseInRowCountUsercontrol))
                    {
                        if (lblRowCounts.InvokeRequired)
                        {
                            lblRowCounts.BeginInvoke(new MethodInvoker(action));
                        }
                        else
                        {
                            action();
                        }
                    }
                });
        }

        private void LabelRowCountMouseLeave(object sender, EventArgs e)
        {
            this.HideRowCountDetails();
        }

        private void RowCountUserControlMouseLeave(object sender, EventArgs e)
        {
            this.HideRowCountDetails();
        }

        private void HideRowCountDetails()
        {
            lblRowCounts.BackColor = SystemColors.Control;

            if (this.IsMouseInRowCountLabel || this.IsMouseInRowCountUsercontrol)
            {
                return;
            }

            if (!this._rowCountUserControl.Visible || !((DateTime.Now - this._rowCountDetailsShownDateTime).TotalMilliseconds >= 400))
            {
                return;
            }

            this._rowCountUserControl.Visible = false;
            this._rowCountUserControl.Size = new Size(this._rowCountUserControl.Width, 1);
            this._rowCountUserControl.Hide();
            this.lblRowCounts.BorderStyle = BorderStyle.None;
        }

        public bool IsMouseInRowCountLabel
        {
            get
            {
                if (lblRowCounts == null)
                {
                    return false;
                }

                bool returnValue = false;
                Action action = () =>
                {
                    var position = Cursor.Position;
                    var mouseToLabel = lblRowCounts.PointToClient(position);
                    var labelRectangle = lblRowCounts.ClientRectangle;

                    labelRectangle.Inflate(4, 4);

                    returnValue = labelRectangle.Contains(mouseToLabel);
                };

                if (lblRowCounts.InvokeRequired)
                {
                    lblRowCounts.Invoke(action);
                }
                else
                {
                    action();
                }

                return returnValue;
            }
        }

        public bool IsMouseInRowCountUsercontrol
        {
            get
            {
                if (_rowCountUserControl == null)
                {
                    return false;
                }

                bool returnValue = false;
                Action action = () =>
                {
                    var position = Cursor.Position;
                    var mouseToControl = _rowCountUserControl.PointToClient(position);
                    var clientRectangle = _rowCountUserControl.ClientRectangle;

                    clientRectangle.Inflate(14, 14);

                    returnValue = clientRectangle.Contains(mouseToControl);
                };

                if (_rowCountUserControl.InvokeRequired)
                {
                    _rowCountUserControl.Invoke(action);
                }
                else
                {
                    action();
                }

                return returnValue;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnDisableFilters_Click(object sender, EventArgs e)
        {
            projectConfigDataGridView1.DisableFilters();
        }

        private void tsmnuSave_Click(object sender, EventArgs e)
        {
            this.Save(FileHelper.CurrentFilename);
        }

        private void Save(string filename = null)
        {
            FileHelper.SaveFile(filename, filename, (ProjectConfigList)projectConfigDataGridView1.DataSource);
        }

        private void tsmnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

    }
}
