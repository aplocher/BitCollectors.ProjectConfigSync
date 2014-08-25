using System.IO;
using ProjectConfigSync.Entities;

namespace ProjectConfigSync.Helpers
{
    public static class FileHelper
    {
        static FileHelper()
        {
            CurrentProjectFiles = null;
        }

        public enum FileTypes
        {
            CsProj,
            Sln,
            Unknown
        }

        public static string CurrentFilename { get; set; }

        public static FileTypes CurrentFileType
        {
            get { return GetFileType(CurrentFilename); }
        }

        public static CsProjFileList CurrentProjectFiles { get; set; }

        //public static void SaveFile(string filename, ProjectConfigList projectConfigList)
        //{
        //    if (CurrentFileType == FileTypes.Sln)
        //    {

        //        //projectConfigList.ForEach(x =>
        //        //    {

        //        //    });

        //    }
        //    else if (CurrentFileType == FileTypes.CsProj)
        //    {

        //    }
        //}

        public static ProjectConfigList LoadFile(string filename)
        {
            ProjectConfigList.Current.Clear();
            CurrentFilename = filename;

            CurrentProjectFiles = new CsProjFileList();
            CurrentProjectFiles.Clear();

            switch (CurrentFileType)
            {
                case FileTypes.CsProj:
                    CurrentProjectFiles = CsProjFileList.GetFromCsProjFile(filename);
                    return CurrentProjectFiles.BindableList;
                    //Solution sln3 = new Solution(@"C:\lifemed\main\lifemed\lifemed.sln");
                    //object obj3 = sln3.Projects;

                    //Project project = new Project(filename);
                    //object obj2 = project;
                    //Console.WriteLine(obj2);
                    break;

                case FileTypes.Sln:
                    CurrentProjectFiles = CsProjFileList.GetFromSolutionFile(filename);
                    return CurrentProjectFiles.BindableList;
                    //Solution sln = new Solution(filename);
                    //object obj = sln.Projects;
                    //Console.WriteLine(obj);
                    break;

                case FileTypes.Unknown:
                    throw new FileLoadException("Filename is incorrect");
            }

            return null;
        }

        public static FileTypes GetFileType(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                var fileInfo = new FileInfo(filename);

                switch (fileInfo.Extension.ToLower())
                {
                    case ".csproj":
                        return FileTypes.CsProj;

                    case ".sln":
                        return FileTypes.Sln;
                }
            }

            return FileTypes.Unknown;
        }

        public static void SaveFile(string originalFilename, string newFilename, ProjectConfigList projectConfigList)
        {
            projectConfigList.Save(originalFilename, newFilename);

            CurrentFilename = newFilename;

            //CurrentProjectFiles.ForEach(x => x.);

            //CurrentProjectFiles.ForEach(x =>
            //    {

            //    });


            //switch (CurrentFileType)
            //{
            //    case FileTypes.CsProj:

            //        break;

            //    case FileTypes.Sln:
            //        break;

            //    default:
            //        throw new FileLoadException("Filename is incorrect");
            //}
        }
    }
}
