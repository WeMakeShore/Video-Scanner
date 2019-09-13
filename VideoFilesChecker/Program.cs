using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

// TODO: Update text files after deletion.
// TODO: Remove instance requirement for GenerateTextDocuments.

namespace VideoFilesChecker
{
    class Program
    {
        public static string moviesInDockPath = @"E:\Media\Movies";
        public static string moviesInExternalDrivePath = @"D:\Plex (External Hard Drive)\Movies";
        public static string tvShowsInDockPath = @"E:\Media\TV Shows";
        public static string tvShowsInExternalDrivePath = @"D:\Plex (External Hard Drive)\TV Shows";
        public static string documentaryMoviesInExternalDrivePath = @"D:\Plex (External Hard Drive)\Documentary Movies";
        public static string documentaryTvInExternalDrivePath = @"D:\Plex (External Hard Drive)\Documentary TV";

        public static List<string> listOfMovies = new List<string>();
        public static List<string> listOfTvShows = new List<string>();
        public static List<string> listOfDocumentaryMovies = new List<string>();
        public static List<string> listOfDocumentaryTv = new List<string>();

        static void Main(string[] args)
        {
            RunProgram();

            if (!VideoDeletion.videosDeleted)
            {
                Console.WriteLine("\nNo videos were deleted.");
            }
            if (VideoDeletion.filenameLengthWarningEnabled == true)
            {
                Console.ReadKey();
            }
            if (VideoDeletion.videosDeleted == true && VideoDeletion.filenameLengthWarningEnabled == false)
            {
                GenerateTextDocuments gtx = new GenerateTextDocuments(listOfMovies, listOfTvShows, listOfDocumentaryMovies, listOfDocumentaryTv);
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                RunProgram();
            }
        }

        private static void RunProgram()
        {
            UpdateVideosAndDirectories();

            GenerateTextDocuments gtd = new GenerateTextDocuments(listOfMovies, listOfTvShows, listOfDocumentaryMovies, listOfDocumentaryTv);

            VideoDeletion.CheckForVideosToDelete();

            VideoDeletion.DeleteVideos();
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
                    listOfMovies.Add(movieTempString.Trim());
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
                    listOfMovies.Add(movieTempString.Trim());
                }
            }

            Console.WriteLine(String.Empty);

            listOfMovies.Sort();
        }

        public static void GetTvShows()
        {
            if (Directory.Exists(tvShowsInDockPath))
            {
                var tempTvShowsInDockPath = Directory.GetDirectories(tvShowsInDockPath).Select(m => Path.GetFileNameWithoutExtension(m));

                foreach (string tvShow in tempTvShowsInDockPath)
                {
                    Console.WriteLine("Tv Show (Dock): " + tvShow);
                    listOfTvShows.Add(tvShow);
                }
            }

            Console.WriteLine(String.Empty);

            if (Directory.Exists(tvShowsInExternalDrivePath))
            {
                var temptvShowsInExternalDrivePath = Directory.GetDirectories(tvShowsInExternalDrivePath).Select(m => Path.GetFileNameWithoutExtension(m));

                foreach (string tvShow in temptvShowsInExternalDrivePath)
                {
                    Console.WriteLine("Tv Show (External Drive): " + tvShow);
                    listOfTvShows.Add(tvShow);
                }
            }

            Console.WriteLine(String.Empty);

            listOfTvShows.Sort();
        }

        public static void GetDocumentaryMovies()
        {
            if(Directory.Exists(documentaryMoviesInExternalDrivePath))
            {
                var tempDocumentaryMoviesInExternalDrivePath = Directory.GetFiles(documentaryMoviesInExternalDrivePath).Where(m => !m.EndsWith(".srt")).Select(m => Path.GetFileNameWithoutExtension(m));

                foreach (string documentaryMovie in tempDocumentaryMoviesInExternalDrivePath)
                {
                    string documentaryMovieTempString = Regex.Replace(documentaryMovie, @"\(.*?\)", String.Empty);
                    Console.WriteLine("Documentary Movie (External Drive): " + documentaryMovieTempString);
                    listOfDocumentaryMovies.Add(documentaryMovieTempString);
                }
            }

            Console.WriteLine(String.Empty);

            listOfDocumentaryMovies.Sort();
        }

        public static void GetDocumentaryTv()
        {
            if(Directory.Exists(documentaryTvInExternalDrivePath))
            {
                var tempDocumentaryTvInExternalDrivePath = Directory.GetDirectories(documentaryTvInExternalDrivePath).Select(m => Path.GetFileNameWithoutExtension(m));

                foreach (string documentaryTv in tempDocumentaryTvInExternalDrivePath)
                {
                    Console.WriteLine(@"Documentary Tv (External Drive): " + documentaryTv);
                    listOfDocumentaryTv.Add(documentaryTv);
                }
            }

            Console.WriteLine(String.Empty);

            listOfDocumentaryTv.Sort();
        }
    }
}
