using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace ProjectConfigSync
{
    public partial class Form1 : Form
    {
        private const string FormTitle = "Project Config Sync";
        private const string FormTitleWithFile = FormTitle + " - {0}";

        private string _filename = string.Empty;
        private XmlDocument _xmlDocument = null;
        private DataTable _dataTable = null;

        private List<string> _projectList = new List<string>();
        private List<string> _configurationList = new List<string>();
        private List<string> _platformList = new List<string>();

        public Form1()
        {
            InitializeComponent();

            ChangeFormTitle();

            cboConfigurations.Items.Add("(All Configurations)");
            cboConfigurations.SelectedIndex = 0;

            cboPlatforms.Items.Add("(All Platforms)");
            cboPlatforms.SelectedIndex = 0;

            cboProjects.Items.Add("(All Projects)");
            cboProjects.SelectedIndex = 0;
        }

        #region Event Handlers
        private void tsmnuOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "(*.sln;*.csproj)|*.sln;*.csproj";
                //dialog.Filter = "C# Project Files (*.csproj)|*.csproj";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fileInfo = new FileInfo(dialog.FileName);

                    switch (fileInfo.Extension.ToLower())
                    {
                        case ".csproj":
                            ClearDataSource();
                            OpenCsProj(dialog.FileName);
                            ChangeFormTitle(dialog.FileName);
                            BindDataSource();
                            break;

                        case ".sln":
                            ClearDataSource();
                            OpenSln(dialog.FileName);
                            ChangeFormTitle(dialog.FileName);
                            BindDataSource();
                            break;
                    }
                }
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.ColumnIndex > 2)
            {
                dataGridView1.CurrentCell = dataGridView1[e.ColumnIndex, e.RowIndex];
                contextMenuStrip1.Show(dataGridView1, dataGridView1.PointToClient(Cursor.Position));
            }
        }

        private void tmnuCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(dataGridView1.CurrentCell.Value.ToString());
        }

        private void tmnuPaste_Click(object sender, EventArgs e)
        {
            dataGridView1.CurrentCell.Value = Clipboard.GetText();
        }

        private void tsmnuSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void tsmnuSaveAs_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "(*.csproj)|*.csproj";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string filename = dialog.FileName;

                    SaveCsProj(filename);

                    ChangeFormTitle(filename);
                }
            }
        }

        private void cboFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterGrid();
        }
        #endregion

        #region Private Methods
        private void ChangeFormTitle(string filename = null)
        {
            if (filename == null)
            {
                this.Text = FormTitle;
            }
            else
            {
                FileInfo fileInfo = new FileInfo(filename);
                _filename = filename;
                this.Text = string.Format(FormTitleWithFile, fileInfo.Name);
            }
        }

        private void ClearDataSource()
        {
            tsmnuSave.Enabled = false;

            _projectList.Clear();
            _projectList.Add("(All Projects)");

            _platformList.Clear();
            _platformList.Add("(All Platforms)");

            _configurationList.Clear();
            _configurationList.Add("(All Configurations)");

            _dataTable = new DataTable();
            _dataTable.Columns.Add("Project");
            _dataTable.Columns.Add("Configuration");
            _dataTable.Columns.Add("Platform");
            _dataTable.Columns.Add("OutputPath");
            _dataTable.Columns.Add("PlatformTarget");
            _dataTable.Columns.Add("DebugType");
            _dataTable.Columns.Add("DebugSymbols");
            _dataTable.Columns.Add("DefineConstants");
            _dataTable.Columns.Add("AllowUnsafeBlocks");
        }

        private void BindDataSource()
        {
            cboPlatforms.DataSource = _platformList;
            cboProjects.DataSource = _projectList;
            cboConfigurations.DataSource = _configurationList;

            dataGridView1.DataSource = _dataTable;

            tsmnuSave.Enabled = true;
        }

        private void OpenCsProj(string filename)
        {
            FileInfo projFileInfo = new FileInfo(filename);

            _xmlDocument = new XmlDocument();
            _xmlDocument.Load(filename);

            XmlNamespaceManager xnManager = new XmlNamespaceManager(_xmlDocument.NameTable);
            xnManager.AddNamespace("pr", "http://schemas.microsoft.com/developer/msbuild/2003");

            XmlNodeList xmlNodeList = _xmlDocument.SelectNodes("//pr:PropertyGroup", xnManager);
            Regex configNodeTest = new Regex(@"^\s*'\$\(Configuration\)\|\$\(Platform\)' == '([A-Za-z0-9]+)\|([A-Za-z0-9]+)'\s*$");

            foreach (XmlNode node in xmlNodeList)
            {
                var conditionAttr = node.Attributes["Condition"];

                if (conditionAttr != null && conditionAttr.Value != null) //configNodeTest.IsMatch(conditionAttr.Value))
                {
                    var matches = configNodeTest.Match(conditionAttr.Value);
                    if (matches.Groups.Count > 1)
                    {
                        string configuration = matches.Groups[1].Value;
                        string platform = matches.Groups[2].Value;

                        if (!_projectList.Contains(projFileInfo.Name))
                            _projectList.Add(projFileInfo.Name);

                        if (!_platformList.Contains(platform))
                            _platformList.Add(platform);

                        if (!_configurationList.Contains(configuration))
                            _configurationList.Add(configuration);

                        DataRow dataRow = _dataTable.NewRow();
                        dataRow["Project"] = projFileInfo.Name;
                        dataRow["Configuration"] = configuration;
                        dataRow["Platform"] = platform;

                        foreach (XmlNode childNode in node.ChildNodes)
                        {
                            switch (childNode.LocalName.ToLower())
                            {
                                case "outputpath":
                                case "allowunsafeblocks":
                                case "defineconstants":
                                case "debugsymbols":
                                case "platformtarget":
                                case "debugtype":
                                    dataRow[childNode.LocalName] = childNode.InnerText;
                                    break;

                            }
                        }

                        _dataTable.Rows.Add(dataRow);
                    }
                }
            }
        }

        private void OpenSln(string filename)
        {
            List<string> projects = GetProjectsFromSolution(filename);

            foreach (string project in projects)
            {
                    OpenCsProj(project);
            }
        }

        private List<string> GetProjectsFromSolution(string filename)
        {
            List<string> returnValue = new List<string>();

            string[] fileText = File.ReadAllLines(filename);

            FileInfo slnFileInfo = new FileInfo(filename);
            string slnDirectory = slnFileInfo.DirectoryName;

            Regex projectLineTest = new Regex("^\\s*Project\\(.*?\\=\\s*\\\".*?\\\",\\s*\\\"(.*?)\\\"");

            foreach (string line in fileText)
            {
                var matches = projectLineTest.Match(line);

                if (matches.Groups.Count > 1)
                {
                    string projPath = Path.Combine(slnDirectory, matches.Groups[1].Value);
                    returnValue.Add(projPath);
                }
            }

            return returnValue;
        }

        private void SaveSln(string filename)
        {
            List<string> projects = GetProjectsFromSolution(filename);

            foreach (string project in projects)
            {
                SaveCsProj(project);
            }
        }

        private void Save()
        {
            DialogResult dialogResult = MessageBox.Show("Warning! By continuing, you are going to be modifying your CSPROJ/SLN files.\r\n\r\nIf these are checked into source control, it's probably a good idea to check them out first (especially if you use TFS or something that causes the files to be marked as read-only).\r\n\r\nAlso, if these files are open in Visual Studio, you should close them.", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dialogResult != DialogResult.OK)
                return;

            FileInfo fileInfo = new FileInfo(_filename);

            switch (fileInfo.Extension.ToLower())
            {
                case ".csproj":
                    SaveCsProj(_filename);
                    break;

                case ".sln":
                    SaveSln(_filename);
                    break;
            }
        }

        //private void SaveCsProj(string filename)
        //{
        //    FileInfo projFileInfo = new FileInfo(filename);

        //    XmlNamespaceManager xnManager = new XmlNamespaceManager(_xmlDocument.NameTable);
        //    xnManager.AddNamespace("pr", "http://schemas.microsoft.com/developer/msbuild/2003");

        //    XmlNodeList xmlNodeList = _xmlDocument.SelectNodes("//pr:PropertyGroup", xnManager);
        //    Regex configNodeTest = new Regex(@"^\s*'\$\(Configuration\)\|\$\(Platform\)' == '([A-Za-z0-9]+)\|([A-Za-z0-9]+)'\s*$");

        //    bool fileChanged = false;

        //    foreach (XmlNode node in xmlNodeList)
        //    {
        //        var conditionAttr = node.Attributes["Condition"];

        //        if (conditionAttr != null && conditionAttr.Value != null)
        //        {
        //            var matches = configNodeTest.Match(conditionAttr.Value);
        //            if (matches.Groups.Count > 1)
        //            {
        //                string configuration = matches.Groups[1].Value;
        //                string platform = matches.Groups[2].Value;
        //                int index = GetConfigRowIndex(projFileInfo.Name, configuration, platform);

        //                if (index > -1)
        //                {
        //                    foreach (XmlNode childNode in node.ChildNodes)
        //                    {
        //                        string newValue = null;

        //                        switch (childNode.LocalName.ToLower())
        //                        {
        //                            case "outputpath":
        //                                newValue = _dataTable.Rows[index]["OutputPath"].ToString();
        //                                break;

        //                            case "allowunsafeblocks":
        //                                newValue = _dataTable.Rows[index]["AllowUnsafeBlocks"].ToString();
        //                                break;

        //                            case "defineconstants":
        //                                newValue = _dataTable.Rows[index]["DefineConstants"].ToString();
        //                                break;

        //                            case "debugsymbols":
        //                                newValue = _dataTable.Rows[index]["DebugSymbols"].ToString();
        //                                break;

        //                            case "platformtarget":
        //                                newValue = _dataTable.Rows[index]["PlatformTarget"].ToString();
        //                                break;

        //                            case "debugtype":
        //                                newValue = _dataTable.Rows[index]["DebugType"].ToString();
        //                                break;

        //                        }

        //                        if (newValue != null && childNode.InnerText != newValue)
        //                        {
        //                            childNode.InnerText = newValue;
        //                            fileChanged = true;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    if (fileChanged)
        //    {
        //        _xmlDocument.Save(filename);
        //    }
        //}

        private void SaveCsProj(string filename)
        {
            _xmlDocument.Load(filename);

            FileInfo projFileInfo = new FileInfo(filename);

            XmlNamespaceManager xnManager = new XmlNamespaceManager(_xmlDocument.NameTable);
            xnManager.AddNamespace("pr", "http://schemas.microsoft.com/developer/msbuild/2003");

            XmlNodeList xmlNodeList = _xmlDocument.SelectNodes("//pr:PropertyGroup", xnManager);
            Regex configNodeTest = new Regex(@"^\s*'\$\(Configuration\)\|\$\(Platform\)' == '([A-Za-z0-9]+)\|([A-Za-z0-9]+)'\s*$");

            bool fileChanged = false;

            foreach (XmlNode node in xmlNodeList)
            {
                var conditionAttr = node.Attributes["Condition"];

                if (conditionAttr != null && conditionAttr.Value != null)
                {
                    var matches = configNodeTest.Match(conditionAttr.Value);
                    if (matches.Groups.Count > 1)
                    {
                        string configuration = matches.Groups[1].Value;
                        string platform = matches.Groups[2].Value;
                        
                        int index = GetConfigRowIndex(projFileInfo.Name, configuration, platform);

                        if (index > -1)
                        {
                            foreach (DataColumn column in _dataTable.Columns)
                            {
                                if (column.ColumnName != "Project" && column.ColumnName != "Platform" && column.ColumnName != "Configuration")
                                {
                                    bool found = false;
                                    string newValue = _dataTable.Rows[index][column.ColumnName].ToString();

                                    foreach (XmlNode childNode in node.ChildNodes)
                                    {
                                        if (column.ColumnName == childNode.LocalName)
                                        {
                                            found = true;

                                            if (childNode.InnerText != newValue)
                                            {
                                                childNode.InnerText = newValue;
                                                fileChanged = true;
                                            }

                                            break;
                                        }
                                    }

                                    if (!found && !string.IsNullOrWhiteSpace(newValue))
                                    {
                                        XmlElement element = _xmlDocument.CreateElement(column.ColumnName, "http://schemas.microsoft.com/developer/msbuild/2003");
                                        element.InnerText = _dataTable.Rows[index][column.ColumnName].ToString();
                                        node.AppendChild(element);
                                        fileChanged = true;
                                    }
                                }
                            }
                        }
                        

                        //if (index > -1)
                        //{
                        //    foreach (XmlNode childNode in node.ChildNodes)
                        //    {
                        //        string newValue = null;

                        //        switch (childNode.LocalName.ToLower())
                        //        {
                        //            case "outputpath":
                        //                newValue = _dataTable.Rows[index]["OutputPath"].ToString();
                        //                break;

                        //            case "allowunsafeblocks":
                        //                newValue = _dataTable.Rows[index]["AllowUnsafeBlocks"].ToString();
                        //                break;

                        //            case "defineconstants":
                        //                newValue = _dataTable.Rows[index]["DefineConstants"].ToString();
                        //                break;

                        //            case "debugsymbols":
                        //                newValue = _dataTable.Rows[index]["DebugSymbols"].ToString();
                        //                break;

                        //            case "platformtarget":
                        //                newValue = _dataTable.Rows[index]["PlatformTarget"].ToString();
                        //                break;

                        //            case "debugtype":
                        //                newValue = _dataTable.Rows[index]["DebugType"].ToString();
                        //                break;

                        //        }

                        //        if (newValue != null && childNode.InnerText != newValue)
                        //        {
                        //            childNode.InnerText = newValue;
                        //            fileChanged = true;
                        //        }
                        //    }
                        //}
                    }
                }
            }

            if (fileChanged)
            {
                _xmlDocument.Save(filename);
            }
        }


        private int GetConfigRowIndex(string project, string configuration, string platform)
        {
            for (int i = 0; i < _dataTable.Rows.Count; i++)
            {
                DataRow row = _dataTable.Rows[i];

                if (row["Project"].ToString() == project && row["Configuration"].ToString() == configuration && row["Platform"].ToString() == platform)
                {
                    return i;
                }
            }

            return -1;
        }

        //private int GetTableRowIndex(string project, string configuration, string platform)
        //{
            
        //}

        private void FilterGrid()
        {
            if (_dataTable == null)
                return;

            EnumerableRowCollection<DataRow> rows = _dataTable.AsEnumerable();

            if (cboProjects.SelectedIndex > 0)
            {
                rows = from d in rows
                       where d.Field<string>("Project") == cboProjects.SelectedValue.ToString()
                       select d;
            }

            if (cboConfigurations.SelectedIndex > 0)
            {
                rows = from d in rows
                       where d.Field<string>("Configuration") == cboConfigurations.SelectedValue.ToString()
                       select d;
            }

            if (cboPlatforms.SelectedIndex > 0)
            {
                rows = from d in rows
                       where d.Field<string>("Platform") == cboPlatforms.SelectedValue.ToString()
                       select d;
            }

            dataGridView1.DataSource = rows.AsDataView();
        }
        #endregion

        private void tmnuCopyToAllRows_Click(object sender, EventArgs e)
        {
            string currentValue = dataGridView1.CurrentCell.Value.ToString();
            string currentConfig = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Index != dataGridView1.CurrentRow.Index)
                {
                    row.Cells[dataGridView1.CurrentCell.ColumnIndex].Value = currentValue;
                }
            }
        }

        private void tsmnuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
