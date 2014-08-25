using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using ProjectConfigSync.Generics;

namespace ProjectConfigSync.Entities
{
    public partial class CsProjConfigElement
    {
        public static CsProjConfigElement ParseOuterElement(string outerXml, string filename, ProjectConfigList replacementValues = null, bool throwWhenMissing = false)
        {
            XDocument xmlFragment = XDocument.Parse(outerXml);

            var outerElement = xmlFragment.Elements().FirstOrDefault();
            if (outerElement == null)
            {
                if (throwWhenMissing)
                {
                    throw new Exception("outerElement is empty");
                }

                return null;
            }

            var conditionAttribute = outerElement.Attribute("Condition");
            if (conditionAttribute == null)
            {
                if (throwWhenMissing)
                {
                    throw new Exception("conditionAttribute is missing");
                }

                return null;
            }

            string condition = outerElement.Value;

            if (string.IsNullOrEmpty(condition))
            {
                return null;
            }

            Match conditionAttrMatch = Regex.Match(conditionAttribute.Value, ProjectConfigList.ConditionAttrRegexMatch);
            if (conditionAttrMatch.Groups.Count > 1)
            {
                string configuration = conditionAttrMatch.Groups[1].Value;
                string platform = conditionAttrMatch.Groups[2].Value;

                ProjectConfig replaceWithValues = null;

                if (replacementValues != null)
                {
                    var filteredReplacementValues = replacementValues.Where(x => 
                        x.ProjectFullFilename.Equals(filename, StringComparison.InvariantCultureIgnoreCase) &&
                        x.PlatformName.Equals(platform, StringComparison.InvariantCultureIgnoreCase) &&
                        x.ConfigurationName.Equals(configuration, StringComparison.InvariantCultureIgnoreCase));

                    var projectConfigs = filteredReplacementValues as ProjectConfig[] ?? filteredReplacementValues.ToArray();
                    if (projectConfigs.Any())
                    {
                        replaceWithValues = projectConfigs.First();
                    }
                }

                var returnValue = new CsProjConfigElement
                    {
                        ProjectFilename                     = filename,
                        ConfigurationName                   = configuration,
                        PlatformName                        = platform,
                        PlatformTarget                      = new CsProjPropertyValue(outerElement.Elements().FirstOrDefault(x => x.Name.LocalName.Equals("PlatformTarget", StringComparison.CurrentCultureIgnoreCase))),
                        DebugType                           = new CsProjPropertyValue(outerElement.Elements().FirstOrDefault(x => x.Name.LocalName.Equals("DebugType", StringComparison.CurrentCultureIgnoreCase))),
                        Optimize                            = new CsProjPropertyValue(outerElement.Elements().FirstOrDefault(x => x.Name.LocalName.Equals("Optimize", StringComparison.CurrentCultureIgnoreCase))),
                        OutputPath                          = new CsProjPropertyValue(outerElement.Elements().FirstOrDefault(x => x.Name.LocalName.Equals("OutputPath", StringComparison.CurrentCultureIgnoreCase))),
                        DefineConstants                     = new CsProjPropertyValue(outerElement.Elements().FirstOrDefault(x => x.Name.LocalName.Equals("DefineConstants", StringComparison.CurrentCultureIgnoreCase))),
                        ErrorReport                         = new CsProjPropertyValue(outerElement.Elements().FirstOrDefault(x => x.Name.LocalName.Equals("ErrorReport", StringComparison.CurrentCultureIgnoreCase))),
                        DebugSymbols                        = new CsProjPropertyValue(outerElement.Elements().FirstOrDefault(x => x.Name.LocalName.Equals("DebugSymbols", StringComparison.CurrentCultureIgnoreCase))),
                        WarningLevel                        = new CsProjPropertyValue(outerElement.Elements().FirstOrDefault(x => x.Name.LocalName.Equals("WarningLevel", StringComparison.CurrentCultureIgnoreCase))),
                        TreatWarningsAsErrors               = new CsProjPropertyValue(outerElement.Elements().FirstOrDefault(x => x.Name.LocalName.Equals("TreatWarningsAsErrors", StringComparison.CurrentCultureIgnoreCase))),
                        DocumentationFile                   = new CsProjPropertyValue(outerElement.Elements().FirstOrDefault(x => x.Name.LocalName.Equals("DocumentationFile", StringComparison.CurrentCultureIgnoreCase))),
                        GenerateSerializationAssemblies     = new CsProjPropertyValue(outerElement.Elements().FirstOrDefault(x => x.Name.LocalName.Equals("GenerateSerializationAssemblies", StringComparison.CurrentCultureIgnoreCase))),
                        LangVersion                         = new CsProjPropertyValue(outerElement.Elements().FirstOrDefault(x => x.Name.LocalName.Equals("LangVersion", StringComparison.CurrentCultureIgnoreCase))),
                        CheckForOverflowUnderflow           = new CsProjPropertyValue(outerElement.Elements().FirstOrDefault(x => x.Name.LocalName.Equals("CheckForOverflowUnderflow", StringComparison.CurrentCultureIgnoreCase))),
                        FileAlignment                       = new CsProjPropertyValue(outerElement.Elements().FirstOrDefault(x => x.Name.LocalName.Equals("FileAlignment", StringComparison.CurrentCultureIgnoreCase))),
                        WarningsAsErrors                    = new CsProjPropertyValue(outerElement.Elements().FirstOrDefault(x => x.Name.LocalName.Equals("WarningsAsErrors", StringComparison.CurrentCultureIgnoreCase))),
                        AllowUnsafeBlocks                   = new CsProjPropertyValue(outerElement.Elements().FirstOrDefault(x => x.Name.LocalName.Equals("AllowUnsafeBlocks", StringComparison.CurrentCultureIgnoreCase))),
                        NoWarn                              = new CsProjPropertyValue(outerElement.Elements().FirstOrDefault(x => x.Name.LocalName.Equals("NoWarn", StringComparison.CurrentCultureIgnoreCase)))
                    };

                return returnValue;
            }

            return null;
        }

        public CsProjConfigElement OriginalValue { get; set; }

        public bool IsDirty { get; set; }
    }
}
