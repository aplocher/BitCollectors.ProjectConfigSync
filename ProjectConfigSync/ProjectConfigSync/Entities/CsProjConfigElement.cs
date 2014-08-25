using ProjectConfigSync.Generics;

namespace ProjectConfigSync.Entities
{
    public partial class CsProjConfigElement
    {
        public string ProjectFilename { get; set; }

        public string ConfigurationName { get; set; }

        public string PlatformName { get; set; }

        public CsProjPropertyValue PlatformTarget { get; set; }

        public CsProjPropertyValue DebugType { get; set; }

        public CsProjPropertyValue DebugSymbols { get; set; }

        public CsProjPropertyValue Optimize { get; set; }

        public CsProjPropertyValue OutputPath { get; set; }

        public CsProjPropertyValue DefineConstants { get; set; }

        public CsProjPropertyValue ErrorReport { get; set; }

        public CsProjPropertyValue WarningLevel { get; set; }

        public CsProjPropertyValue TreatWarningsAsErrors { get; set; }

        public CsProjPropertyValue DocumentationFile { get; set; }

        public CsProjPropertyValue GenerateSerializationAssemblies { get; set; }

        public CsProjPropertyValue LangVersion { get; set; }

        public CsProjPropertyValue CheckForOverflowUnderflow { get; set; }

        public CsProjPropertyValue FileAlignment { get; set; }

        public CsProjPropertyValue WarningsAsErrors { get; set; }

        public CsProjPropertyValue AllowUnsafeBlocks { get; set; }

        public CsProjPropertyValue NoWarn { get; set; }
    }
}
