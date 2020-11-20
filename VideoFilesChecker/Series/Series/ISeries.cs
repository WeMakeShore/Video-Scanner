using System.Collections.Generic;

namespace VideoFilesChecker
{
    interface ISeries
    {
        string SeriesName { get; set; }
        List<Episode> Episodes { get; set; }
    }
}
