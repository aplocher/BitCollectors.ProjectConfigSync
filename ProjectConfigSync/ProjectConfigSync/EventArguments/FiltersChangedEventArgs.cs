using System;

namespace ProjectConfigSync.EventArguments
{
    public class FiltersChangedEventArgs : EventArgs
    {
        public string ProjectFilter { get; set; }

        public string ConfigurationFilter { get; set; }

        public string PlatformFilter { get; set; }
    }
}
