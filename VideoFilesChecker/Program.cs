using System;
using System.Collections.Generic;
using ExceptionHandler;
using System.Threading.Tasks;
using GetVideoData;
using VideoFilesChecker;
using VideoFilesChecker.Drive;

namespace VideoChecking
{
    class Program
    {
        public static Settings settings = Settings.GetSettings();

        public static List<string> moviePaths = new List<string>();
        public static List<string> tvShowPaths = new List<string>();
        public static List<string> docMoviePaths = new List<string>();
        public static List<string> docTvShowPaths = new List<string>();

        public static List<Video> listOfMovies = new List<Video>();
        public static List<Show> listOfTvShows = new List<Show>();
        public static List<Video> listOfDocumentaryMovies = new List<Video>();
        public static List<Show> listOfDocumentaryTv = new List<Show>();
        public static List<DriveModel> listOfDrives = DrivesModel.GetDrives();

        static void Main(string[] args)
        {
            RunProgram();

            if (VideoDeletion.filenameLengthWarningEnabled == true)
            {
                Console.ReadKey();
            }

            if (VideoDeletion.videosDeleted == true && VideoDeletion.filenameLengthWarningEnabled == false)
            {
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                GetFilesAndDirectories.UpdateVideosAndDirectories();
            }
        }

        private static void RunProgram()
        {
            //if (Exceptions.checkForPreviousException())
            //{
            //    Console.WriteLine("Warning - Unexpected closure on previous run. Not getting videos before update.\n");
            //}
            //else
            //{
            //    Generate.GetVideosBeforeUpdate();
            //}

            GetFilesAndDirectories.UpdateVideosAndDirectories();
            
            Task.Run(async () => { await CreateHttpRequest.Login(); }).Wait();

            Task.Run(async () => { await CreateHttpRequest.CreateGETRequest(); }).Wait();

            VideoDeletion.CheckForVideosToDelete();

            VideoDeletion.DeleteVideos();

            Task.Run(async () => { await CreateHttpRequest.CreatePOSTRequest(); }).Wait();

            Task.Run(async () => { await CreateHttpRequest.Logout(); }).Wait();

            if (settings.RefreshLibraries == true)
            {
                Plex.RefreshPlexLibraries();
            }

            Exceptions.RemovePreviousExceptionLog();

            Console.ReadKey();
        }
    }
}
