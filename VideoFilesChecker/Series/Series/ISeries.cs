using System;
using System.Collections.Generic;

namespace VideoFilesChecker
{
    interface ISeries
    {
        string SeriesName { get; set; }
        DateTime CreationTime { get; set; } 
        List<Episode> Episodes { get; set; }
    }
}
