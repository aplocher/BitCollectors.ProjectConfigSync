using System.Collections.Generic;
using System.IO;

namespace ProjectConfigSync.Entities
{
    public partial class ProjectConfigList
    {
        public const string AllConfigurationsComboText = "(All Configurations)";
        public const string AllProjectsComboText = "(All Projects)";
        public const string AllPlatformsComboText = "(All Platforms)";

        public const string ConditionAttrRegexMatch = @"^\s*'\$\(Configuration\)\|\$\(Platform\)' == '([^\|']+)\|([^\|']+)'\s*$";

        private static ProjectConfigList _current = null;

        public static ProjectConfigList Current
        {
            get
            {
                return _current ?? (_current = new ProjectConfigList());
            }
        }

        public void FindUniqueFields()
        {
            this.UniqueProjects = new List<string> { AllProjectsComboText };
            this.UniqueConfigurations = new List<string> { AllConfigurationsComboText };
            this.UniquePlatforms = new List<string> { AllPlatformsComboText };

            if (this.Count <= 0)
            {
                return;
            }

            foreach (var projectConfig in this)
            {
                string shortFilename = projectConfig.ProjectFullFilename == null ? null : new FileInfo(projectConfig.ProjectFullFilename).Name;

                if (shortFilename != null && !this.UniqueProjects.Contains(shortFilename))
                {
                    this.UniqueProjects.Add(shortFilename);
                }

                if (!this.UniqueConfigurations.Contains(projectConfig.ConfigurationName))
                {
                    this.UniqueConfigurations.Add(projectConfig.ConfigurationName);
                }

                if (!this.UniquePlatforms.Contains(projectConfig.PlatformName))
                {
                    this.UniquePlatforms.Add(projectConfig.PlatformName);
                }
            }

            this.UniqueProjects.Sort();
            this.UniqueConfigurations.Sort();
            this.UniquePlatforms.Sort();
        }

        public void Save(string originalFilename, string newFilename)
        {
            CsProjFileList.GetFromSolutionFile(originalFilename).Save(originalFilename, newFilename, this);

        }

        public CsProjFileList SavableList
        {
            get { return null; }
        }
    }
}
