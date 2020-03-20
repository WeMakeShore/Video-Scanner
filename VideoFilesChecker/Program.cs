using System;
using System.Collections.Generic;
using ExceptionHandler;
using System.Threading.Tasks;
using GetVideoData;

namespace VideoChecking
{
    class Program
    {
        //public static string moviesInDockPath = @"C:\Video-Test\Movies Dock";
        //public static string moviesInExternalDrivePath = @"C:\Video-Test\Movies";
        //public static string tvShowsInDockPath = @"C:\Video-Test\TV Shows";
        //public static string tvShowsInExternalDrivePath = @"C:\Video-Test\TV Shows Dock";
        //public static string documentaryMoviesInExternalDrivePath = @"C:\Video-Test\Documentary Movies";
        //public static string documentaryTvInExternalDrivePath = @"C:\Video-Test\Documentary TV";

        public static string moviesInDockPath = @"E:\Media\Movies";
        public static string moviesInExternalDrivePath = @"D:\Plex (External Hard Drive)\Movies";
        public static string tvShowsInDockPath = @"E:\Media\TV Shows";
        public static string tvShowsInExternalDrivePath = @"D:\Plex (External Hard Drive)\TV Shows";
        public static string documentaryMoviesInExternalDrivePath = @"D:\Plex (External Hard Drive)\Documentary Movies";
        public static string documentaryTvInExternalDrivePath = @"D:\Plex (External Hard Drive)\Documentary TV";

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

            PrintGETData();

            VideoDeletion.CheckForVideosToDelete();

            VideoDeletion.DeleteVideos();

            Exceptions.RemovePreviousExceptionLog();

            Console.ReadKey();
        }

        public static void PrintGETData() // TODO: Move into a logger.
        {
            // Movies
            foreach (Video movie in VideoDeletion.listOfDeletionRequests.Movies)
            {
                Console.WriteLine("GET [movie]: " + movie.Title);
            }

            if (VideoDeletion.listOfDeletionRequests.Movies.Length > 0)
            {
                Console.WriteLine();
            }

            // TV Shows
            foreach (Video tvShow in VideoDeletion.listOfDeletionRequests.TvShows)
            {
                Console.WriteLine("GET [TV Show]: " + tvShow);
            }

            if (VideoDeletion.listOfDeletionRequests.TvShows.Length > 0)
            {
                Console.WriteLine();
            }

            // Documentary Movies
            foreach (Video documentaryMovies in VideoDeletion.listOfDeletionRequests.DocumentaryMovies)
            {
                Console.WriteLine("GET [Documentary Movies]: " + documentaryMovies);
            }

            if (VideoDeletion.listOfDeletionRequests.DocumentaryMovies.Length > 0)
            {
                Console.WriteLine();
            }

            // Documentary TV
            foreach (Video documentaryTv in VideoDeletion.listOfDeletionRequests.DocumentaryTv)
            {
                Console.WriteLine("GET [Documentary TV]: " + documentaryTv);
            }

            if (VideoDeletion.listOfDeletionRequests.DocumentaryTv.Length > 0)
            {
                Console.WriteLine();
            }
        } 
    }
}
