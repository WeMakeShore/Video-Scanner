
using VideoFilesChecker;

namespace VideoChecking
{
    public class Videos
    {
        public Video[] Movies;
        public Video[] TvShows;
        public Video[] DocumentaryMovies;
        public Video[] DocumentaryTv;
        public Drives DockHardDirve { get; set; }
        public Drives ExternalHardDrive { get; set; }

        public Videos(Video[] movies, Video[] tvShows, Video[] documentaryMovies, Video[] documentaryTv)
        {
            this.Movies = movies;
            this.TvShows = tvShows;
            this.DocumentaryMovies = documentaryMovies;
            this.DocumentaryTv = documentaryTv;
        }
    }
}
