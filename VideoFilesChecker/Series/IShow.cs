using System;
using System.Collections.Generic;
using VideoFilesChecker;

namespace VideoChecking
{
    interface IShow
    {
        string Title { get; set; }
        int Year { get; set; }
        long Size { get; set; }
        string DriveLocation { get; set; }
        List<Series> Series { get; set; }
    }
}
