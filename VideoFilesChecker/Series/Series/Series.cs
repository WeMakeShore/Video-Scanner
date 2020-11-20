namespace VideoFilesChecker
{
    public class Series
    {
        public string SeriesName { get; set; }
        public Episode[] Episodes { get; set; }
        public Series(string seriesName, Episode[] episodes)
        {
            this.SeriesName = seriesName;
            this.Episodes = episodes;
        } 
    }
}
