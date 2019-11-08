using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using VideoFileChecker;
using ExceptionHandler;
using System.Threading.Tasks;

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

        public static List<string> listofMoviesWithoutFilePath = new List<string>();
        public static List<string> listOfTvShowsWithoutFilePath = new List<string>();
        public static List<string> listOfDocumentaryMoviesWithoutFilePath = new List<string>();
        public static List<string> listOfDocumentaryTvShowsWithoutFilePath = new List<string>();

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
                UpdateVideosAndDirectories();
            }
        }

        private static void RunProgram()
        {
            if (Exceptions.checkForPreviousException())
            {
                Console.WriteLine("Warning - Unexpected closure on previous run. Not getting videos before update.\n");
            } else
            {
                Generate.GetVideosBeforeUpdate();
            }

            UpdateVideosAndDirectories();

            Generate.GenerateDocuments();

            if (!VideoChecker.VideoChangesFound())
            {
                Console.WriteLine("No Changes found.\n");

                Task.Run(async () => { await CreateHttpRequest.CreateGETRequest(); }).Wait();

                PrintGETData();

                VideoDeletion.CheckForVideosToDelete();

                VideoDeletion.DeleteVideos();

                Exceptions.RemovePreviousExceptionLog();

                Console.ReadKey();

                Environment.Exit(0);
            }

            Task.Run(async () => { await CreateHttpRequest.CreatePOSTRequest(new Videos(listofMoviesWithoutFilePath.ToArray(), listOfTvShowsWithoutFilePath.ToArray(), listOfDocumentaryMoviesWithoutFilePath.ToArray(), listOfDocumentaryTvShowsWithoutFilePath.ToArray())); }).Wait();

            Task.Run(async () => { await CreateHttpRequest.CreateGETRequest(); }).Wait();

            PrintGETData();

            VideoDeletion.CheckForVideosToDelete();

            VideoDeletion.DeleteVideos();

            Exceptions.RemovePreviousExceptionLog();

            Console.ReadKey();
        }

        public static void PrintGETData()
        {
            // Movies
            foreach (string movie in VideoDeletion.movieDeletionRequests)
            {
                Console.WriteLine("GET [movie]: " + movie);
            }

            if (VideoDeletion.movieDeletionRequests.Count > 0)
            {
                Console.WriteLine();
            }

            // TV Shows
            foreach (string tvShow in VideoDeletion.tvShowDeletionRequests)
            {
                Console.WriteLine("GET [TV Show]: " + tvShow);
            }

            if (VideoDeletion.tvShowDeletionRequests.Count > 0)
            {
                Console.WriteLine();
            }

            // Documentary Movies
            foreach (string documentaryMovies in VideoDeletion.documentaryMovieDeletionRequests)
            {
                Console.WriteLine("GET [Documentary Movies]: " + documentaryMovies);
            }

            if (VideoDeletion.documentaryMovieDeletionRequests.Count > 0)
            {
                Console.WriteLine();
            }

            // Documentary TV
            foreach (string documentaryTv in VideoDeletion.documentaryTvDeletionRequests)
            {
                Console.WriteLine("GET [Documentary TV]: " + documentaryTv);
            }

            if (VideoDeletion.documentaryTvDeletionRequests.Count > 0)
            {
                Console.WriteLine();
            }
        }

        public static void UpdateVideosAndDirectories()
        {
            GetMovies();

            GetTvShows();

            GetDocumentaryMovies();

            GetDocumentaryTv();
        }

        public static void GetMovies()
        {
            if (Directory.Exists(moviesInDockPath))
            {
                var tempMoviesArray = Directory.GetFiles(moviesInDockPath).Where(m => !m.EndsWith(".srt")).Select(m => Path.GetFileNameWithoutExtension(m));

                foreach (string movie in tempMoviesArray)
                {
                    string movieTempString = Regex.Replace(movie, @"\(.*?\)", String.Empty);
                    Console.WriteLine("Movie (Dock): " + movieTempString);
                    listofMoviesWithoutFilePath.Add(movieTempString.Trim());
                }
            }

            Console.WriteLine(String.Empty);

            if (Directory.Exists(moviesInExternalDrivePath))
            {
                var tempMoviesInExternalDrive = Directory.GetFiles(moviesInExternalDrivePath).Where(m => !m.EndsWith(".srt")).Select(m => Path.GetFileNameWithoutExtension(m));

                foreach (string movie in tempMoviesInExternalDrive)
                {
                    string movieTempString = Regex.Replace(movie, @"\(.*?\)", String.Empty);
                    Console.WriteLine("Movie (External Drive): " + movieTempString);
                    listofMoviesWithoutFilePath.Add(movieTempString.Trim());
                }
            }

            Console.WriteLine(String.Empty);

            listofMoviesWithoutFilePath.Sort();
        }

        public static void GetTvShows()
        {
            if (Directory.Exists(tvShowsInDockPath))
            {
                var tempTvShowsInDockPath = Directory.GetDirectories(tvShowsInDockPath).Select(m => Path.GetFileNameWithoutExtension(m));

                foreach (string tvShow in tempTvShowsInDockPath)
                {
                    Console.WriteLine("Tv Show (Dock): " + tvShow);
                    listOfTvShowsWithoutFilePath.Add(tvShow);
                }
            }

            Console.WriteLine(String.Empty);

            if (Directory.Exists(tvShowsInExternalDrivePath))
            {
                var temptvShowsInExternalDrivePath = Directory.GetDirectories(tvShowsInExternalDrivePath).Select(m => Path.GetFileNameWithoutExtension(m));

                foreach (string tvShow in temptvShowsInExternalDrivePath)
                {
                    Console.WriteLine("Tv Show (External Drive): " + tvShow);
                    listOfTvShowsWithoutFilePath.Add(tvShow);
                }
            }

            Console.WriteLine(String.Empty);

            listOfTvShowsWithoutFilePath.Sort();
        }

        public static void GetDocumentaryMovies()
        {
            if (Directory.Exists(documentaryMoviesInExternalDrivePath))
            {
                var tempDocumentaryMoviesInExternalDrivePath = Directory.GetFiles(documentaryMoviesInExternalDrivePath).Where(m => !m.EndsWith(".srt")).Select(m => Path.GetFileNameWithoutExtension(m));

                foreach (string documentaryMovie in tempDocumentaryMoviesInExternalDrivePath)
                {
                    string documentaryMovieTempString = Regex.Replace(documentaryMovie, @"\(.*?\)", String.Empty);
                    Console.WriteLine("Documentary Movie (External Drive): " + documentaryMovieTempString);
                    listOfDocumentaryMoviesWithoutFilePath.Add(documentaryMovieTempString);
                }
            }

            Console.WriteLine(String.Empty);

            listOfDocumentaryMoviesWithoutFilePath.Sort();
        }

        public static void GetDocumentaryTv()
        {
            if (Directory.Exists(documentaryTvInExternalDrivePath))
            {
                var tempDocumentaryTvInExternalDrivePath = Directory.GetDirectories(documentaryTvInExternalDrivePath).Select(m => Path.GetFileNameWithoutExtension(m));

                foreach (string documentaryTv in tempDocumentaryTvInExternalDrivePath)
                {
                    Console.WriteLine(@"Documentary Tv (External Drive): " + documentaryTv);
                    listOfDocumentaryTvShowsWithoutFilePath.Add(documentaryTv);
                }
            }

            Console.WriteLine(String.Empty);

            listOfDocumentaryTvShowsWithoutFilePath.Sort();
        }
    }
}
