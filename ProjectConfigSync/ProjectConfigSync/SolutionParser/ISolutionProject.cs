using System;
using System.Collections.Generic;
using Microsoft.Build.Evaluation;

namespace ProjectConfigSync.SolutionParser
{
    interface ISolutionProject
    {
        List<Project> Projects { get; }
    }
}
