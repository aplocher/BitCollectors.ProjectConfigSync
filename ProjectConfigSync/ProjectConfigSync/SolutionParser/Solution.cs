using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    public class Solution
    {
        internal static readonly Type SolutionParser;
        internal static readonly PropertyInfo SolutionParserSolutionReader;
        internal static readonly MethodInfo SolutionParserParseSolution;
        internal static readonly PropertyInfo SolutionParserProjects;

        private bool _isSolutionDataDirty = false;

        static Solution()
        {
            SolutionParser = Type.GetType("Microsoft.Build.Construction.SolutionParser, Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false, false);
            if (SolutionParser == null)
            {
                return;
            }

            SolutionParserSolutionReader = SolutionParser.GetProperty("SolutionReader", BindingFlags.NonPublic | BindingFlags.Instance);
            SolutionParserProjects = SolutionParser.GetProperty("Projects", BindingFlags.NonPublic | BindingFlags.Instance);
            SolutionParserParseSolution = SolutionParser.GetMethod("ParseSolution", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public Solution(string solutionFileName)
        {
            if (SolutionParser == null)
            {
                throw new InvalidOperationException("Can not find type 'Microsoft.Build.Construction.SolutionParser' are you missing a assembly reference to 'Microsoft.Build.dll'?");
            }

            var solutionParser = SolutionParser.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).First().Invoke(null);
            using (var streamReader = new StreamReader(solutionFileName))
            {
                SolutionParserSolutionReader.SetValue(solutionParser, streamReader, null);
                SolutionParserParseSolution.Invoke(solutionParser, null);
            }

            var projects = new List<SolutionProject>();
            var array = (Array)SolutionParserProjects.GetValue(solutionParser, null);

            for (int i = 0; i < array.Length; i++)
            {
                var solutionProject = (SolutionProject)array.GetValue(i);
                solutionProject.Project = new Project(solutionProject.RelativePath);

                projects.Add(solutionProject);
            }

            this.Projects = projects;
        }

        public List<SolutionProject> Projects { get; private set; }

        public bool IsDirty
        {
            get
            {
                if (_isSolutionDataDirty)
                {
                    return true;
                }

                bool returnValue = false;
                this.Projects.ForEach(x =>
                    {
                        if (!x.IsDirty)
                        {
                            return;
                        }

                        returnValue = true;
                    });

                return returnValue;
            }
        }
    }

}
