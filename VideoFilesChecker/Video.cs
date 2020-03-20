using System;
using VideoFilesChecker;


public class Video : IVideo
{
    public string Title { get; set; }
    public string Year { get; set; }
    public long Size { get; set; }

    public string DriveLocation { get; set; }
    public DateTime CreationTime { get; set; }

    public Video(string title, string year, long size, string DriveLocation, DateTime CreationTime)
    {
        this.Title = title;
        this.Year = year;
        this.Size = size;
        this.DriveLocation = DriveLocation;
        this.CreationTime = CreationTime;
    }
}

