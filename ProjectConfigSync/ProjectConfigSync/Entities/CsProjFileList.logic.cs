
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProjectConfigSync.Entities
{
    public partial class CsProjFileList
    {
        public static CsProjFileList CurrentProjects { get; set; }

        public CsProjFileList()
        {
        }

        public static CsProjFileList GetFromSolutionFile(string solutionFilename)
        {
            CurrentProjects = new CsProjFileList();
            CurrentProjects.Clear();

            ForEachProjectInSolution(
                solutionFilename,
                projectFilename =>
                    CurrentProjects.Add(CsProjFile.InitCsProjFile(projectFilename)));

            return CurrentProjects;
        }

        public static CsProjFileList GetFromCsProjFile(string csprojFilename)
        {
            CsProjFile csproj = CsProjFile.InitCsProjFile(csprojFilename);

            CurrentProjects = new CsProjFileList();
            CurrentProjects.Clear();

            CurrentProjects.Add(csproj);

            return CurrentProjects;
        }

        private static void ForEachProjectInSolution(string filename, Action<string> action)
        {
            IEnumerable<string> projectFiles = GetProjectFileListFromSolution(filename);

            foreach (string projectFile in projectFiles)
            {
                action(projectFile);
            }
        }

        private static IEnumerable<string> GetProjectFileListFromSolution(string solutionFilename)
        {
            var returnValue = new List<string>();

            string[] fileText = File.ReadAllLines(solutionFilename);

            var solutionFileInfo = new FileInfo(solutionFilename);
            string solutionDirectory = solutionFileInfo.DirectoryName;

            var projectLineTest = new Regex(@"^\s*Project\(.*?\=\s*\"".*?"",\s*""(.*?\.[Cc][Ss][Pp][Rr][Oo][Jj])\""");

            foreach (string line in fileText)
            {
                var matches = projectLineTest.Match(line);

                if (matches.Groups.Count > 1)
                {
                    string projPath = Path.Combine(solutionDirectory, matches.Groups[1].Value);
                    returnValue.Add(projPath);
                }
            }

            return returnValue;
        }

        public ProjectConfigList BindableList
        {
            get
            {
                var bindableList = new ProjectConfigList();
                this.ForEach(d => bindableList.AddRange(d.ConvertToBindable()));

                var returnValue = new ProjectConfigList();
                returnValue.AddRange(bindableList.OrderBy(x => x.ProjectName).ThenBy(x => x.ConfigurationName).ThenBy(x => x.PlatformName).ThenBy(x => x.OutputPath).ThenBy(x => x.PlatformTarget).ToList());

                //bindableList = bindableList.OrderBy(x => x.ProjectName).ThenBy(x => x.ConfigurationName).ToList();
                returnValue.FindUniqueFields();

                return returnValue;
            }
        }

        public bool IsDirty { get; set; }

        public void Save(string oldFilename, string newFilename, ProjectConfigList projectConfigList)
        {
            var backupPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "AdvancedProjectConfig\\Backups",
                DateTime.Now.ToString("s").Replace(":", "_"));

            if (!Directory.Exists(backupPath))
            {
                Directory.CreateDirectory(backupPath);
            }

            ForEachProjectInSolution(oldFilename, x => CsProjFile.SaveCsProjFile(x, backupPath, projectConfigList));
        }
    }
}
