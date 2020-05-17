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

        public static List<string> moviePaths = new List<string>();
        public static List<string> tvShowPaths = new List<string>();
        public static List<string> docMoviePaths = new List<string>();
        public static List<string> docTvShowPaths = new List<string>();

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
            //if (Exceptions.checkForPreviousException())
            //{
            //    Console.WriteLine("Warning - Unexpected closure on previous run. Not getting videos before update.\n");
            //} else
            //{
            //    Generate.GetVideosBeforeUpdate();
            //}

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

            Task.Run(async () => { await CreateHttpRequest.CreateGETRequest(); }).Wait();

            VideoDeletion.CheckForVideosToDelete();

            VideoDeletion.DeleteVideos();

            Task.Run(async () => { await CreateHttpRequest.CreatePOSTRequest(GetFilesAndDirectories.GetSerializedJsonVideoFileData());}).Wait();

            if (settings.RefreshLibraries == true)
            {
                Plex.RefreshPlexLibraries();
            }

            Exceptions.RemovePreviousExceptionLog();

            Console.ReadKey();
        }
    }
}
