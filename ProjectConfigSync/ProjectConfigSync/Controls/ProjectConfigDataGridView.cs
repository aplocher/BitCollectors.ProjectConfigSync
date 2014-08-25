using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ProjectConfigSync.Entities;
using ProjectConfigSync.EventArguments;

namespace ProjectConfigSync.Controls
{
    internal class ProjectConfigDataGridView : DataGridView//, IDataGridViewEditingControl
    {
        private readonly ContextMenuStrip _contextMenuStrip;
        private readonly ToolStripMenuItem _ctxMenuPaste;
        private readonly ToolStripMenuItem _ctxMenuCopyToAllVisible;
        private readonly ToolStripMenuItem _ctxMenuDeleteThisRow;
        private readonly ToolStripMenuItem _ctxMenuCopy;
        private readonly ToolStripMenuItem _ctxMenuHideThisRow;
        private readonly ToolStripMenuItem _ctxMenuHideAllVisible;
        private readonly ToolStripMenuItem _ctxMenuDeleteAllVisible;
        private readonly DataGridViewCellStyle _frozeCellStyle = new DataGridViewCellStyle() { BackColor = Color.Silver };
        private readonly DataGridViewCellStyle _highlightCellStyle = new DataGridViewCellStyle() { BackColor = Color.Wheat };

        private ProjectConfigList _newDataSource = null;
        private List<ProjectConfig> _newDataSourceFiltered = null;
        private string _lastProjectFilter = null;
        private string _lastConfigFilter = null;
        private string _lastPlatformFilter = null;

        private object _copiedValue = null;
        private int _copiedValueColumn = -1;

        public event EventHandler RowCountsChanged;

        public event EventHandler<FiltersChangedEventArgs> FiltersChanged;

        public ProjectConfigDataGridView()
        {
            TotalRowCount = 0;
            TotalVisibleCount = 0;
            TotalHiddenCount = 0;
            DeletePendingCount = 0;
            ExplicitlyHiddenCount = 0;
            FilteredCount = 0;

            this.AutoGenerateColumns = false;

            if (this.DesignMode)
            {
                this.Columns.Clear();
                return;
            }

            this.Initialize();

            base.DoubleBuffered = true;

            this.VirtualMode = true;
            this.ScrollBars = ScrollBars.Both;
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToResizeRows = false;
            this.AllowUserToOrderColumns = false;
            this.AllowUserToResizeColumns = true;
            this.MultiSelect = false;
            this.SelectionMode = DataGridViewSelectionMode.CellSelect;

            _ctxMenuCopy = new ToolStripMenuItem("Copy");
            _ctxMenuCopy.Click += ContextMenuCopyClick;

            _ctxMenuPaste = new ToolStripMenuItem("Paste");
            _ctxMenuPaste.Click += ContextMenuPasteClick;

            _ctxMenuCopyToAllVisible = new ToolStripMenuItem("Copy to all visible rows");
            _ctxMenuCopyToAllVisible.Click += ContextMenuCopyToAllVisiblClick;

            _ctxMenuHideThisRow = new ToolStripMenuItem("Hide this row");
            _ctxMenuHideThisRow.Click += ContextMenuHideThisRowClick;

            _ctxMenuHideAllVisible = new ToolStripMenuItem("Hide all visible rows");
            _ctxMenuHideAllVisible.Click += ContextMenuHideAllVisibleClick;

            _ctxMenuDeleteThisRow = new ToolStripMenuItem("Delete this row");
            _ctxMenuDeleteThisRow.Click += ContextMenuDeleteThisRowClick;

            _ctxMenuDeleteAllVisible = new ToolStripMenuItem("Delete all visible rows");
            _ctxMenuDeleteAllVisible.Click += ContextMenuDeleteAllVisibleClick;

            _contextMenuStrip = new ContextMenuStrip();
            _contextMenuStrip.Items.Add(_ctxMenuCopy);
            _contextMenuStrip.Items.Add(_ctxMenuPaste);
            _contextMenuStrip.Items.Add(new ToolStripSeparator());
            _contextMenuStrip.Items.Add(_ctxMenuCopyToAllVisible);
            _contextMenuStrip.Items.Add(new ToolStripSeparator());
            _contextMenuStrip.Items.Add(_ctxMenuHideThisRow);
            _contextMenuStrip.Items.Add(_ctxMenuHideAllVisible);
            _contextMenuStrip.Items.Add(new ToolStripSeparator());
            _contextMenuStrip.Items.Add(_ctxMenuDeleteThisRow);
            _contextMenuStrip.Items.Add(_ctxMenuDeleteAllVisible);
        }

        public int FilteredCount { get; private set; }

        public int ExplicitlyHiddenCount { get; private set; }

        public int DeletePendingCount { get; private set; }

        public int TotalHiddenCount { get; private set; }

        public int TotalVisibleCount { get; private set; }

        public int TotalRowCount { get; private set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new object DataSource
        {
            get
            {
                if (DesignMode)
                {
                    return null;
                }

                return this._newDataSource;
            }
            set
            {
                if (DesignMode)
                {
                    return;
                }

                var list = value as ProjectConfigList;
                if (list != null)
                {
                    this._newDataSource = list;
                    this.SetFiltersInternal();
                }
            }
        }

        public void CalculateRowCounts(bool allData = false)
        {
            int lastTotalRowCount = this.TotalRowCount;
            int lastTotalHiddenCount = this.TotalHiddenCount;
            int lastExplicitlyHiddenCount = this.ExplicitlyHiddenCount;

            this.TotalRowCount = this._newDataSource == null ? 0 : this._newDataSource.Count(x => !x.IsDeleted);
            this.TotalVisibleCount = this._newDataSourceFiltered == null ? 0 : this._newDataSourceFiltered.Count;
            this.TotalHiddenCount = this.TotalRowCount - this.TotalVisibleCount;

            if (allData)
            {
                this.ExplicitlyHiddenCount = _newDataSource.Count(x => x.IsHidden);
                this.DeletePendingCount = _newDataSource.Count(x => x.IsDeleted);

                if (this.FilteredCount == 0 || this.TotalHiddenCount != lastTotalHiddenCount)
                {
                    this.FilteredCount = this.GetFilteredDataSource(_lastProjectFilter, _lastConfigFilter, _lastPlatformFilter, false).Count();
                }

                if ((lastTotalRowCount != this.TotalRowCount || lastTotalHiddenCount != this.TotalHiddenCount || lastExplicitlyHiddenCount != this.ExplicitlyHiddenCount)
                    && this.RowCountsChanged != null)
                {
                    this.RowCountsChanged(this, new EventArgs());
                }
            }
            else
            {
                if ((lastTotalRowCount != this.TotalRowCount || lastTotalHiddenCount != this.TotalHiddenCount) && this.RowCountsChanged != null)
                {
                    this.RowCountsChanged(this, new EventArgs());
                }
            }
        }

        public List<ProjectConfig> GetFilteredDataSource(string project = null, string config = null, string platform = null, bool filterHidden = true, bool filterDeleted = true)
        {
            if (this._newDataSource == null)
            {
                return null;
            }

            return (from s in this._newDataSource
                    where
                        ((filterHidden && !s.IsHidden) || !filterHidden)
                        && ((filterDeleted && !s.IsDeleted) || !filterDeleted)
                        && (string.IsNullOrWhiteSpace(project) || s.ProjectName.StartsWith(project, StringComparison.InvariantCultureIgnoreCase))
                        && (string.IsNullOrWhiteSpace(config) || s.ConfigurationName.StartsWith(config, StringComparison.InvariantCultureIgnoreCase))
                        && (string.IsNullOrWhiteSpace(platform) || s.PlatformName.StartsWith(platform, StringComparison.InvariantCultureIgnoreCase))
                    select s).ToList();
        }

        public void SetFilters(string project = null, string config = null, string platform = null, bool filterHidden = true, bool filterDeleted = true)
        {
            this._lastProjectFilter = project;
            this._lastConfigFilter = config;
            this._lastPlatformFilter = platform;

            this._newDataSourceFiltered = this.GetFilteredDataSource(project, config, platform, filterHidden, filterDeleted);

            this.RowCount = this._newDataSourceFiltered == null
                ? 0
                : this._newDataSourceFiltered.Count;

            this.CalculateRowCounts();
            this.Refresh();
        }

        public void DisableFilters()
        {
            _newDataSource.Select(x => x.IsHidden = false);
            this.SetFilters(null, null, null, false);
        }

        // Added on 2014-04-04
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DataGridViewColumnCollection Columns
        {
            get { return base.Columns; }
            set { }
        }

        public void RefreshFilters()
        {
            this.SetFiltersInternal(_lastProjectFilter, _lastConfigFilter, _lastPlatformFilter);
        }

        private void SetFiltersInternal(string project = null, string config = null, string platform = null, bool filterHidden = true, bool filterDeleted = true)
        {
            this.SetFilters(project, config, platform, filterHidden, filterDeleted);
            if (FiltersChanged != null)
            {
                FiltersChanged(this, new FiltersChangedEventArgs() { ConfigurationFilter = config, PlatformFilter = platform, ProjectFilter = project });
            }
        }

        //protected override void PaintBackground(Graphics graphics, Rectangle clipBounds, Rectangle gridBounds)
        //{
        //    // Added 2014-02-19 2:10am
        //    if (!DesignMode)
        //    {
        //        base.PaintBackground(graphics, clipBounds, gridBounds);
        //    }
        //}

        protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
        {
            base.OnCellMouseDown(e);

            if (e.Button == MouseButtons.Right && e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                this.CurrentCell = this[e.ColumnIndex, e.RowIndex];
                this._contextMenuStrip.Show(this, this.PointToClient(Cursor.Position));

                this._ctxMenuCopyToAllVisible.Enabled = (e.ColumnIndex > 2 && this.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null);
                this._ctxMenuPaste.Enabled = e.ColumnIndex > 2 && _copiedValue != null && _copiedValueColumn == e.ColumnIndex;
                this._ctxMenuDeleteThisRow.Text = string.Format("Delete project config ({0}|{1})", this.CurrentRow.Cells[1].Value, this.CurrentRow.Cells[2].Value);
                this._ctxMenuCopyToAllVisible.Text = "Copy to all " + this.TotalVisibleCount + " visible rows";
                this._ctxMenuHideAllVisible.Text = "Hide all " + this.TotalVisibleCount + " visible rows";
                this._ctxMenuDeleteAllVisible.Text = "Delete all " + this.TotalVisibleCount + " visible rows";

            }
        }

        //protected override void InitLayout()
        //{
        //    if (!this.DesignMode)
        //    {
        //        base.InitLayout();
        //    }
        //}

        //// Added on 2014-04-04
        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //protected override void OnColumnAdded(DataGridViewColumnEventArgs e)
        //{
        //    if (!this.DesignMode)
        //    {
        //        base.OnColumnAdded(e);
        //    }
        //}

        //// Added on 2014-04-04
        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //protected override void OnAutoGenerateColumnsChanged(EventArgs e)
        //{
        //    // Added 2014-02-19 2am
        //    if (!this.DesignMode)
        //    {
        //        base.OnAutoGenerateColumnsChanged(e);
        //    }
        //}

        protected override DataGridViewColumnCollection CreateColumnsInstance()
        {
            // Added 2014-02-19 2am
            if (!this.DesignMode)
            {
                return base.CreateColumnsInstance();
            }

            return null;
        }

        protected override void OnColumnStateChanged(DataGridViewColumnStateChangedEventArgs e)
        {
            if (!this.DesignMode)
            {
                base.OnColumnStateChanged(e);
            }
        }

        private int previousSortIndex = -1;
        private int newSortIndex = 0;
        private ListSortDirection sortDirection = ListSortDirection.Ascending;

        //public override void Sort(DataGridViewColumn dataGridViewColumn, ListSortDirection direction)
        //{
        //    base.Sort(dataGridViewColumn, direction);

        //    //this.sortColumnIndex = dataGridViewColumn.Index;
        //    //this.sortDirection = direction;
        //    //this.Refresh();
        //}

        protected override void OnCreateControl()
        {
            if (!this.DesignMode)
            {
                base.OnCreateControl();
            }
        }

        protected override void OnCellValueNeeded(DataGridViewCellValueEventArgs e)
        {
            if (this._newDataSourceFiltered != null && this._newDataSourceFiltered.Count > e.RowIndex)
            {
                //if (newSortIndex != previousSortIndex)
                //{
                //    switch (newSortIndex)
                //    {
                //        case 0:
                //            this._newDataSourceFiltered.Sort((x, y) => x.ProjectName.CompareTo(y.ProjectName));
                //            previousSortIndex = newSortIndex;
                //            break;

                //        case 1:
                //            this._newDataSourceFiltered.Sort((x, y) => x.ConfigurationName.CompareTo(y.ConfigurationName));
                //            previousSortIndex = newSortIndex;
                //            break;

                //        case 2:
                //            this._newDataSourceFiltered.Sort((x, y) => x.PlatformName.CompareTo(y.PlatformName));
                //            previousSortIndex = newSortIndex;
                //            break;
                //    }
                //}

                object value = this._newDataSourceFiltered[e.RowIndex].GetValueFromOrdinal(e.ColumnIndex);

                if (e.ColumnIndex > 2 && value == null)
                {
                    this.Rows[e.RowIndex].Cells[e.ColumnIndex].Style = _highlightCellStyle;
                }

                e.Value = value;

                this.Rows[e.RowIndex].HeaderCell.Value = (e.RowIndex + 1).ToString();
            }
        }

        protected override void OnRowHeightInfoNeeded(DataGridViewRowHeightInfoNeededEventArgs e)
        {
            e.Height = 26;
            base.OnRowHeightInfoNeeded(e);
        }

        private void ContextMenuCopyClick(object sender, EventArgs e)
        {
            if (this.CurrentCell == null || this.CurrentCell.Value == null)
                return;

            _copiedValue = this.CurrentCell.Value;
            _copiedValueColumn = this.CurrentCell.ColumnIndex;

            Clipboard.SetText(_copiedValue.ToString());
        }

        private void ContextMenuPasteClick(object sender, EventArgs e)
        {
            if (this.CurrentCell == null || this.CurrentCell.Value == null)
            {
                return;
            }

            if (this._copiedValue != null && this._copiedValueColumn == this.CurrentCell.ColumnIndex)
            {
                this._newDataSourceFiltered[this.CurrentCell.RowIndex].SetValueFromOrdinal(this.CurrentCell.ColumnIndex, _copiedValue);
                this.Refresh();
            }
        }

        private void ContextMenuCopyToAllVisiblClick(object sender, EventArgs e)
        {
            object currentValue = this.CurrentCell.Value;

            foreach (var item in this._newDataSourceFiltered)
            {
                item.SetValueFromOrdinal(this.CurrentCell.ColumnIndex, currentValue);
            }

            this.Refresh();
        }

        private void ContextMenuHideAllVisibleClick(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show(string.Format("Are you sure you want to temporarily hide all visible rows ({0} total)?", _newDataSourceFiltered.Count), string.Format("Delete {0} configs?", _newDataSourceFiltered.Count), MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                foreach (var filteredRow in this._newDataSourceFiltered)
                {
                    var row = filteredRow;
                    foreach (var sourceRow in this._newDataSource.Where(sourceRow => sourceRow.ConfigurationName == row.ConfigurationName && sourceRow.ProjectFullFilename == row.ProjectFullFilename && sourceRow.PlatformName == row.PlatformName))
                    {
                        sourceRow.IsHidden = true;
                    }
                }

                _newDataSourceFiltered.Clear();
                this.RefreshFilters();
                //this.SetFiltersInternal();
            }
        }

        private void ContextMenuHideThisRowClick(object sender, EventArgs e)
        {
            if (this.CurrentCell == null)
                return;

            _newDataSourceFiltered[this.CurrentCell.RowIndex].IsHidden = true;
            this.RefreshFilters();
            //this.SetFiltersInternal();
        }

        private void ContextMenuDeleteThisRowClick(object sender, EventArgs e)
        {
            if (this.CurrentCell == null)
                return;

            _newDataSourceFiltered[this.CurrentCell.RowIndex].IsDeleted = true;
            this.RefreshFilters();
            //this.SetFiltersInternal();
        }

        private void ContextMenuDeleteAllVisibleClick(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show(string.Format("Are you sure you want to DELETE all visible rows ({0} total)?", _newDataSourceFiltered.Count), string.Format("Delete {0} configs?", _newDataSourceFiltered.Count), MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                foreach (var filteredRow in this._newDataSourceFiltered)
                {
                    var row = filteredRow;
                    foreach (var sourceRow in this._newDataSource.Where(sourceRow => sourceRow.ConfigurationName == row.ConfigurationName && sourceRow.ProjectFullFilename == row.ProjectFullFilename && sourceRow.PlatformName == row.PlatformName))
                    {
                        sourceRow.IsDeleted = true;
                    }
                }

                _newDataSourceFiltered.Clear();
                this.RefreshFilters();
                //this.SetFiltersInternal();
            }
        }

        protected override void OnCellValuePushed(DataGridViewCellValueEventArgs e)
        {
            if (e.Value is CheckState)
            {
                switch ((CheckState)e.Value)
                {
                    case CheckState.Checked:
                        this._newDataSourceFiltered[e.RowIndex].SetValueFromOrdinal(e.ColumnIndex, true);
                        break;

                    case CheckState.Unchecked:
                        this._newDataSourceFiltered[e.RowIndex].SetValueFromOrdinal(e.ColumnIndex, false);
                        break;

                    case CheckState.Indeterminate:
                        this._newDataSourceFiltered[e.RowIndex].SetValueFromOrdinal(e.ColumnIndex, null);
                        break;
                }
            }
            else if (e.ColumnIndex > 2 && e.Value is string)
            {
                this._newDataSourceFiltered[e.RowIndex].SetValueFromOrdinal(e.ColumnIndex, e.Value);
            }

            base.OnCellValuePushed(e);
        }

        protected override void OnCellValueChanged(DataGridViewCellEventArgs e)
        {
            base.OnCellValueChanged(e);

            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;

            this.Rows[e.RowIndex].Cells[e.ColumnIndex].Style = this.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null
                ? _highlightCellStyle
                : null;
        }

        public void Initialize()
        {
            if (this.DesignMode)
            {
                return;
            }

            //this.ColumnHeadersDefaultCellStyle.Font = new Font(FontFamily.Families.);
            this.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.ColumnHeadersHeight = 24;
            this.RowHeadersWidth = 60;
            this.Columns.Clear();
            this.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { SortMode = DataGridViewColumnSortMode.Automatic, HeaderText = "Project", DataPropertyName = "ProjectName", Frozen = true, ReadOnly = true, DefaultCellStyle = _frozeCellStyle },
                new DataGridViewTextBoxColumn { SortMode = DataGridViewColumnSortMode.Automatic, HeaderText = "Configuration", DataPropertyName = "ConfigurationName", Frozen = true, ReadOnly = true, DefaultCellStyle = _frozeCellStyle },
                new DataGridViewTextBoxColumn { SortMode = DataGridViewColumnSortMode.Automatic, HeaderText = "Platform", DataPropertyName = "PlatformName", Frozen = true, ReadOnly = true, DefaultCellStyle = _frozeCellStyle },
                new DataGridViewTextBoxColumn { SortMode = DataGridViewColumnSortMode.Automatic, HeaderText = "Output Path", DataPropertyName = "OutputPath", Frozen = false },
                new DataGridViewTextBoxColumn { SortMode = DataGridViewColumnSortMode.Automatic, HeaderText = "Actual Platform", DataPropertyName = "PlatformTarget", Frozen = false },
                new DataGridViewTextBoxColumn { SortMode = DataGridViewColumnSortMode.Automatic, HeaderText = "Debug Type", DataPropertyName = "DebugType", Frozen = false },
                new DataGridViewTextBoxColumn { SortMode = DataGridViewColumnSortMode.Automatic, HeaderText = "Debug Symbols", DataPropertyName = "DebugSymbols", Frozen = false },
                new DataGridViewTextBoxColumn { SortMode = DataGridViewColumnSortMode.Automatic, HeaderText = "Define Constants", DataPropertyName = "DefineConstants", Frozen = false },
                new DataGridViewCheckBoxColumn { HeaderText = "Optimize", DataPropertyName = "Optimize", ThreeState = true, Frozen = false, Width = 83 },
                new DataGridViewCheckBoxColumn { HeaderText = "Allow Unsafe Blocks", DataPropertyName = "AllowUnsafeBlocks", ThreeState = true, Frozen = false, Width = 84 },
                new DataGridViewCheckBoxColumn { HeaderText = "Build", DataPropertyName = "Build", ThreeState = true , Frozen = false, Width = 83 },
                new DataGridViewTextBoxColumn { DataPropertyName = "IsDeleted", Visible = false, Frozen = false },
                new DataGridViewTextBoxColumn { DataPropertyName = "IsHidden", Visible = false, Frozen = false }
            });
        }

        //public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        //{
        //    throw new NotImplementedException();
        //}

        //public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        //{
        //    throw new NotImplementedException();
        //}

        //public void PrepareEditingControlForEdit(bool selectAll)
        //{
        //    throw new NotImplementedException();
        //}

        //public DataGridView EditingControlDataGridView
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        //public object EditingControlFormattedValue
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        //public int EditingControlRowIndex
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        //public bool EditingControlValueChanged
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        //public Cursor EditingPanelCursor
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //public bool RepositionEditingControlOnValueChange
        //{
        //    get { throw new NotImplementedException(); }
        //}
    }
}
