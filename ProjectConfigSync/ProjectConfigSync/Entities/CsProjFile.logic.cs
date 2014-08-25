
using System.IO;
using System.Linq;

namespace ProjectConfigSync.Entities
{
    public partial class CsProjFile
    {
        public ProjectConfigList ConvertToBindable()
        {
            var returnValue = new ProjectConfigList();
            returnValue.AddRange(
                this.PropertyGroups.Select(element =>
                    {
                        bool? allowUnsafeBlocks = null;
                        if (element.AllowUnsafeBlocks.IsValueExplicitlySet)
                        {
                            bool allowUnsafeBlocksTmp;
                            allowUnsafeBlocks = bool.TryParse(element.AllowUnsafeBlocks.Value, out allowUnsafeBlocksTmp) ? allowUnsafeBlocksTmp : (bool?)null;
                        }

                        bool? optimize = null;
                        if (element.Optimize.IsValueExplicitlySet)
                        {
                            bool optimizeTmp;
                            optimize = bool.TryParse(element.Optimize.Value, out optimizeTmp) ? optimizeTmp : (bool?)null;
                        }

                        var fileInfo = new FileInfo(element.ProjectFilename);
                        return new ProjectConfig
                               {
                                   ProjectName = fileInfo.Name,
                                   ConfigurationName = element.ConfigurationName,
                                   PlatformName = element.PlatformName,
                                   AllowUnsafeBlocks = allowUnsafeBlocks,
                                   Optimize = optimize,
                                   DebugType = element.DebugType.IsValueExplicitlySet ? element.DebugType.Value : null,
                                   DebugSymbols = element.DebugSymbols.IsValueExplicitlySet ? element.DebugSymbols.Value : null,
                                   DefineConstants = element.DefineConstants.IsValueExplicitlySet ? element.DefineConstants.Value : null,
                                   OutputPath = element.OutputPath.IsValueExplicitlySet ? element.OutputPath.Value : null,
                                   PlatformTarget = element.PlatformTarget.IsValueExplicitlySet ? element.PlatformTarget.Value : null,
                                   ProjectFullFilename = element.ProjectFilename,
                                   Build = true
                               };
                    }));

            return returnValue;
        }
    }
}
