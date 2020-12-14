using System;
namespace VideoFilesChecker
{
    public class Series
    {
        public string SeriesName { get; set; }
        public DateTime CreationTime { get; set; }
        public Episode[] Episodes { get; set; }
        public Series(string seriesName, DateTime creationTime, Episode[] episodes)
        {
            this.SeriesName = seriesName;
            this.CreationTime = creationTime;
            this.Episodes = episodes;
        } 
    }
}
