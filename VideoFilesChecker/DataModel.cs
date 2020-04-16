using VideoFilesChecker;

namespace VideoChecking
{
    public class DataModel
    {
        public Video[] Movies;
        public Video[] TvShows;
        public Video[] DocumentaryMovies;
        public Video[] DocumentaryTv;
        public DriveModel[] Drives;

        public DataModel(Video[] movies, Video[] tvShows, Video[] documentaryMovies, Video[] documentaryTv, DriveModel[] drives)
        {
            this.Movies = movies;
            this.TvShows = tvShows;
            this.DocumentaryMovies = documentaryMovies;
            this.DocumentaryTv = documentaryTv;
            this.Drives = drives;
        }
    }
}
