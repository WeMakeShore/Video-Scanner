using VideoFilesChecker;

namespace VideoChecking
{
    public class DataModel
    {
        public Video[] Movies;
        public Show[] TvShows;
        public Video[] DocumentaryMovies;
        public Show[] DocumentaryTv;

        public DataModel(Video[] movies, Show[] tvShows, Video[] documentaryMovies, Show[] documentaryTv)
        {
            this.Movies = movies;
            this.TvShows = tvShows;
            this.DocumentaryMovies = documentaryMovies;
            this.DocumentaryTv = documentaryTv;
        }
    }
}