using System.ComponentModel;
using ProjectConfigSync.Attributes;

namespace ProjectConfigSync.Entities
{
    public partial class ProjectConfig
    {
        [DisplayName("Project Name")]
        [PropertyOrdinal(0)]
        public string ProjectName { get; set; }

        [DisplayName("Config Name")]
        [PropertyOrdinal(1)]
        public string ConfigurationName { get; set; }

        [DisplayName("Platform Name")]
        [PropertyOrdinal(2)]
        public string PlatformName { get; set; }

        [DisplayName("Output Path")]
        [PropertyOrdinal(3)]
        public string OutputPath { get; set; }

        [DisplayName("Actual Platform")]
        [PropertyOrdinal(4)]
        public string PlatformTarget { get; set; }

        [DisplayName("Debug Type")]
        [PropertyOrdinal(5)]
        public string DebugType { get; set; }

        [DisplayName("Debug Symbols")]
        [PropertyOrdinal(6)]
        public string DebugSymbols { get; set; }

        [DisplayName("Defined Constants")]
        [PropertyOrdinal(7)]
        public string DefineConstants { get; set; }

        [DisplayName("Optimized")]
        [PropertyOrdinal(8)]
        public bool? Optimize { get; set; }

        [DisplayName("Allow Unsafe Blocks")]
        [PropertyOrdinal(9)]
        public bool? AllowUnsafeBlocks { get; set; }

        [DisplayName("Build")]
        [PropertyOrdinal(10)]
        public bool? Build { get; set; }

        [Browsable(false)]
        [PropertyOrdinal(11)]
        public bool IsDeleted { get; set; }

        [Browsable(false)]
        [PropertyOrdinal(12)]
        public bool IsHidden { get; set; }

        [Browsable(false)]
        [PropertyOrdinal(12)]
        public bool IsDirty { get; set; }

        [Browsable(false)]
        [PropertyOrdinal(13)]
        public string ProjectFullFilename { get; set; }
    }
}
