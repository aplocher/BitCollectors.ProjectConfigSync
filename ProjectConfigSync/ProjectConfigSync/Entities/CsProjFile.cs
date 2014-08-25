using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace ProjectConfigSync.Entities
{
    public partial class CsProjFile
    {
        private readonly string _filename;

        private static readonly string[] _fields = { "OutputPath", "PlatformTarget", "DebugType", "DebugSymbols", "DefineConstants", "Optimize", "AllowUnsafeBlocks" };
        //

        //, 
        //"LangVersion", "GenerateSerializationAssemblies", "DocumentationFile", 
        //"TreatWarningsAsErrors", "WarningLevel", "ErrorReport"
        //};
        // "NoWarn", 
        // "WarningsAsErrors", "FileAlignment", "CheckForOverflowUnderflow", 

        private CsProjFile(string filename)
        {
            _filename = filename;
            this.PropertyGroups = this.GetProjConfigElementList(null);
        }

        public List<CsProjConfigElement> PropertyGroups { get; private set; }

        private static XmlNodeList GetXmlNodeList(string filename, XmlDocument projXmlDocument)
        {
            if (!File.Exists(filename))
            {
                // TODO Instead of throwing an error when a CSPROJ in a SLN isn't found. Add the row to the grid but highlight as red
                // so the user has the option of fixing it.

                throw new FileNotFoundException("csproj file was not found: " + filename);
            }


            projXmlDocument.Load(filename);

            var xmlNamespaceManager = new XmlNamespaceManager(projXmlDocument.NameTable);
            xmlNamespaceManager.AddNamespace("pr", "http://schemas.microsoft.com/developer/msbuild/2003");

            XmlNodeList xmlNodeList = projXmlDocument.SelectNodes("//pr:PropertyGroup", xmlNamespaceManager);

            return xmlNodeList;
        }

        private List<CsProjConfigElement> GetProjConfigElementList(ProjectConfigList replacementValues)
        {
            var projXmlDocument = new XmlDocument();
            var xmlNodeList = GetXmlNodeList(_filename, projXmlDocument);

            if (xmlNodeList == null)
            {
                return null;
            }

            var returnValue = xmlNodeList.Cast<XmlNode>()
                .Select(node => CsProjConfigElement.ParseOuterElement(node.OuterXml, this._filename, replacementValues))
                .Where(parseResult => parseResult != null)
                .ToList();

            return returnValue;
            //XDocument xDocument
            //CsProjConfigElement.ParseOuterElement()
        }

        public static CsProjFile InitCsProjFile(string filename)
        {
            return new CsProjFile(filename);
        }

        public static void SaveCsProjFile(string filename, string backupPath, ProjectConfigList project)
        {
            var projXmlDocument = new XmlDocument();
            var xmlNodeList = GetXmlNodeList(filename, projXmlDocument);
            bool changed = false;

            for (int i = xmlNodeList.Count - 1; i >= 0; i--)
            {
                XmlNode node = xmlNodeList[i];
                if (node == null || node.Attributes == null || node.Attributes["Condition"] == null)
                {
                    continue;
                }

                var conditionAttribute = node.Attributes["Condition"];
                var conditionAttrMatch = Regex.Match(conditionAttribute.Value, ProjectConfigList.ConditionAttrRegexMatch);
                if (conditionAttrMatch.Groups.Count <= 1)
                {
                    continue;
                }

                string configuration = conditionAttrMatch.Groups[1].Value;
                string platform = conditionAttrMatch.Groups[2].Value;

                var filteredProjects = project.Where(x =>
                        x.ConfigurationName.Trim().Equals(configuration.Trim(), StringComparison.InvariantCultureIgnoreCase) &&
                        x.PlatformName.Equals(platform, StringComparison.InvariantCultureIgnoreCase) &&
                        x.ProjectFullFilename.Equals(filename, StringComparison.InvariantCultureIgnoreCase));

                foreach (var proj in filteredProjects)
                {
                    if (proj.IsDeleted)
                    {
                        if (node.ParentNode != null)
                        {
                            node.ParentNode.RemoveChild(node);
                        }

                        changed = true;
                    }
                    else
                    {
                        foreach (var field in _fields)
                        {
                            bool found = false;
                            object newValueObj = proj.GetType().GetProperty(field).GetValue(proj, null);
                            if (newValueObj == null)
                            {
                                continue;
                            }

                            string newValue = newValueObj.ToString();
                            if (newValueObj is bool)
                            {
                                newValue = newValue.ToLower();
                            }

                            foreach (XmlNode childNode in node.ChildNodes)
                            {
                                if (field.Trim().Equals(childNode.LocalName.Trim(), StringComparison.InvariantCultureIgnoreCase))
                                {
                                    found = true;
                                    if (childNode.InnerText != newValue)
                                    {
                                        childNode.InnerText = newValue;
                                        changed = true;
                                    }
                                    break;
                                }
                            }

                            if (!found && !string.IsNullOrWhiteSpace(newValue))
                            {
                                XmlElement element = projXmlDocument.CreateElement(field, "http://schemas.microsoft.com/developer/msbuild/2003");
                                element.InnerText = newValue;
                                node.AppendChild(element);
                                changed = true;
                            }
                        }
                    }
                }
            }

            if (changed)
            {
                var fileInfo = new FileInfo(filename);
                var backupFilePath = Path.Combine(backupPath, fileInfo.Name);
                int i = 1;
                while (File.Exists(backupFilePath))
                {
                    string newFile = Path.GetFileNameWithoutExtension(fileInfo.Name) + "_" + i;
                    if (!string.IsNullOrEmpty(fileInfo.Extension))
                    {
                        newFile += fileInfo.Extension;
                    }

                    backupFilePath = Path.Combine(backupPath, newFile);

                    i++;
                }

                File.Copy(filename, backupFilePath, true);

                projXmlDocument.Save(filename);
            }
        }
    }
}
