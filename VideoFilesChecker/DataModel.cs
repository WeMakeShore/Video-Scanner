using VideoFilesChecker;

namespace VideoChecking
{
    public class DataModel
    {
        public Video[] Movies;
        public Show[] TvShows;
        public Video[] DocumentaryMovies;
        public Show[] DocumentaryTv;
        public DriveModel[] Drives;

        public DataModel(Video[] movies, Show[] tvShows, Video[] documentaryMovies, Show[] documentaryTv, DriveModel[] drives)
        {
            this.Movies = movies;
            this.TvShows = tvShows;
            this.DocumentaryMovies = documentaryMovies;
            this.DocumentaryTv = documentaryTv;
            this.Drives = drives;
        }
    }
}
