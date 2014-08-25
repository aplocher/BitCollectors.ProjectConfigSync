using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Build.Evaluation;

namespace ProjectConfigSync.SolutionParser
{
    //// Many thanks to John Leidegren for this class
    //// Read about this hack at: http://stackoverflow.com/questions/707107/library-for-parsing-visual-studio-solution-files#4634505
    //// 
    //// internal class SolutionParser
    //// Name: Microsoft.Build.Construction.SolutionParser
    //// Assembly: Microsoft.Build, Version=4.0.0.0

    [DebuggerDisplay("{ProjectName}, {RelativePath}, {ProjectGuid}")]
    public class SolutionProject
    {
        internal static readonly Type ProjectInSolution;
        internal static readonly PropertyInfo ProjectInSolutionProjectName;
        internal static readonly PropertyInfo ProjectInSolutionRelativePath;
        internal static readonly PropertyInfo ProjectInSolutionProjectGuid;

        static SolutionProject()
        {
            ProjectInSolution = Type.GetType("Microsoft.Build.Construction.ProjectInSolution, Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false, false);
            if (ProjectInSolution == null)
            {
                return;
            }

            ProjectInSolutionProjectName = ProjectInSolution.GetProperty("ProjectName", BindingFlags.NonPublic | BindingFlags.Instance);
            ProjectInSolutionRelativePath = ProjectInSolution.GetProperty("RelativePath", BindingFlags.NonPublic | BindingFlags.Instance);
            ProjectInSolutionProjectGuid = ProjectInSolution.GetProperty("ProjectGuid", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public SolutionProject(object solutionProject)
        {
            this.ProjectName = ProjectInSolutionProjectName.GetValue(solutionProject, null) as string;
            this.RelativePath = ProjectInSolutionRelativePath.GetValue(solutionProject, null) as string;
            this.ProjectGuid = ProjectInSolutionProjectGuid.GetValue(solutionProject, null) as string;
        }

        public string ProjectName { get; private set; }

        public string RelativePath { get; private set; }

        public string ProjectGuid { get; private set; }

        public Project Project { get; set; }

        [DefaultValue(false)]
        public bool IsDirty { get; set; }
    }
}
