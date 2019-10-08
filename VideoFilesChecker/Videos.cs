
namespace VideoFilesChecker
{
    public class Videos
    {
        public string[] Movies { get; set; }
        public string[] TvShows { get; set; }
        public string[] DocumentaryMovies { get; set; }
        public string[] DocumentaryTv { get; set; }

        public Videos(string[] movies, string[] tvShows, string[] documentaryMovies, string[] documentaryTv)
        {
            this.Movies = movies;
            this.TvShows = tvShows;
            this.DocumentaryMovies = documentaryMovies;
            this.DocumentaryTv = documentaryTv;
        }
    }
}
