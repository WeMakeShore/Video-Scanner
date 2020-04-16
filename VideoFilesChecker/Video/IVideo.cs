using System;
using VideoFilesChecker.Drive;

namespace VideoFilesChecker
{
    interface IVideo : IDriveModel
    {
        string Title { get; set; }
        string Year { get; set; }
        string DriveLocation { get; set; }
        long Size { get; set; }
        DateTime CreationTime { get; set; }
    }
}