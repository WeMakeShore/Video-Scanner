using System;
using System.Collections.Generic;
using ExceptionHandler;
using System.Threading.Tasks;
using GetVideoData;

namespace VideoChecking
{
    class Program
    {
        public static Settings settings = Settings.GetSettings();

        public static string moviesDockPath = settings.MoviesDockPath;
        public static string moviesExtDrivePath = settings.MoviesExtDrivePath;
        public static string tvShowsDockPath = settings.TvShowsDockPath;
        public static string tvShowsExtDrivePath = settings.TvShowsExtDrivePath;
        public static string docMoviesExtDrivePath = settings.DocMoviesExtDrivePath;
        public static string docTvShowsExtDrivePath = settings.DocTvExtDrivePath;

        public static List<Video> listOfMovies = new List<Video>();
        public static List<Video> listOfTvShows = new List<Video>();
        public static List<Video> listOfDocumentaryMovies = new List<Video>();
        public static List<Video> listOfDocumentaryTv = new List<Video>();

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
            if (Exceptions.checkForPreviousException())
            {
                Console.WriteLine("Warning - Unexpected closure on previous run. Not getting videos before update.\n");
            } else
            {
                //Generate.GetVideosBeforeUpdate();
            }

            GetFilesAndDirectories.UpdateVideosAndDirectories();

            //if (!VideoChecker.VideoChangesFound())
            //{
            //    Console.WriteLine("No Changes found.\n");

            //    Task.Run(async () => { await CreateHttpRequest.CreateGETRequest(); }).Wait();

            //    PrintGETData();

            //    VideoDeletion.CheckForVideosToDelete();

            //    VideoDeletion.DeleteVideos();

            //    Exceptions.RemovePreviousExceptionLog();

            //    if (VideoDeletion.videosDeleted)
            //    {
            //        Console.ReadKey();
            //    }

            //    Environment.Exit(0);
            //}

            Task.Run(async () => { await CreateHttpRequest.CreatePOSTRequest(GetFilesAndDirectories.GetSerializedJsonVideoFileData());}).Wait();

            Task.Run(async () => { await CreateHttpRequest.CreateGETRequest(); }).Wait();

            Logger.PrintGETData();

            VideoDeletion.CheckForVideosToDelete();

            VideoDeletion.DeleteVideos();

            Exceptions.RemovePreviousExceptionLog();

            Console.ReadKey();
        }
    }
}
