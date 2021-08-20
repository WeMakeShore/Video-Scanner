using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoFilesChecker
{
    public class MessagePropsModel
    {
        public Boolean Successful { get; set; }
        public string Message { get; set; }

        public MessagePropsModel(Boolean successful, string message)
        {
            this.Successful = successful;
            this.Message = message;
        }
    }
    public class CreateMoviesResponseModel
    {
        public MessagePropsModel CreateMovies { get; set; }

        public CreateMoviesResponseModel(MessagePropsModel CreateMovies)
        {
            this.CreateMovies = CreateMovies;
        }
    }

    public class CreateTvShowsResponseModel
    {
        public MessagePropsModel CreateTvShows { get; set; }

        public CreateTvShowsResponseModel(MessagePropsModel createTvShows)
        {
            this.CreateTvShows = createTvShows;
        }
    }

    public class CreateDocumentaryMoviesResponseModel
    {
        public MessagePropsModel CreateDocumentaryMovies { get; set; }

        public CreateDocumentaryMoviesResponseModel(MessagePropsModel createDocumentaryMovies)
        {
            this.CreateDocumentaryMovies = createDocumentaryMovies;
        }
    }

    public class CreateDocumentaryTvResponseModel
    {
        public MessagePropsModel CreateDocumentaryTv { get; set; }

        public CreateDocumentaryTvResponseModel(MessagePropsModel createDocumentaryTvShows)
        {
            this.CreateDocumentaryTv = createDocumentaryTvShows;
        }
    }
    public class CreateDrivesResponseModel
    {
        public MessagePropsModel CreateDrives { get; set; }

        public CreateDrivesResponseModel(MessagePropsModel createDrives)
        {
            this.CreateDrives = createDrives;
        }
    }

    public class LogoutResponseModel
    {
        public MessagePropsModel Logout { get; set; }

        public LogoutResponseModel(MessagePropsModel logout)
        {
            this.Logout = logout;
        }
    }
}
