using Newtonsoft.Json;
using System;

public class Video
{
    public string Title { get; set; }
    public int Year { get; set; }
    public long Size { get; set; }
    public string DriveLocation { get; set; }
    public DateTime CreationTime { get; set; }

    public Video(string title, int year, long size, string driveLocation, DateTime creationTime)
    {
        this.Title = title;
        this.Year = year;
        this.Size = size;
        this.DriveLocation = driveLocation;
        this.CreationTime = creationTime;
    }
}
