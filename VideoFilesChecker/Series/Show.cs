using System;
using System.Collections.Generic;
using VideoFilesChecker;

namespace VideoChecking
{
    public class Show : IShow
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public long Size { get; set; }
        public string DriveLocation { get; set; }
        public List<Series> Series { get; set; }

        public Show(string title, int year, long size, string driveLocation, List<Series> series)
        {
            this.Title = title;
            this.Year = year;
            this.Size = size;
            this.DriveLocation = driveLocation;
            this.Series = series;
        }
    }
}
