using System;

namespace VideoFilesChecker
{
    interface IVideo
    {
        string Title { get; set; }
        string Year { get; set; }
        string DriveLocation { get; set; }
        long Size { get; set; }
        DateTime CreationTime { get; set; }
    }
}