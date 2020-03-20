
namespace VideoChecking
{
    public class Videos
    {
        public Video[] Movies;
        public Video[] TvShows;
        public Video[] DocumentaryMovies;
        public Video[] DocumentaryTv;

        public Videos(Video[] movies, Video[] tvShows, Video[] documentaryMovies, Video[] documentaryTv)
        {
            this.Movies = movies;
            this.TvShows = tvShows;
            this.DocumentaryMovies = documentaryMovies;
            this.DocumentaryTv = documentaryTv;
        }
    }
}
