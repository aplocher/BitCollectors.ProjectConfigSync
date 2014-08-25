using System.Collections.Generic;

namespace ProjectConfigSync.Entities
{
    public partial class ProjectConfigList : List<ProjectConfig>
    {
        public List<string> UniqueProjects { get; set; }

        public List<string> UniqueConfigurations { get; set; }

        public List<string> UniquePlatforms { get; set; }
    }
}
