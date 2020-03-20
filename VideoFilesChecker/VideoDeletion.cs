using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;
using ExceptionHandler;

namespace VideoChecking
{
    static class VideoDeletion
    {
        //private const string movieDeletionRequestsPath = @"C:\X230 File Share\Video Deletion Requests\Movies to Delete.txt";
        //private const string tvDeletionRequestsPath = @"C:\X230 File Share\Video Deletion Requests\TV Shows to Delete.txt";
        //private const string documentaryMovieDeletionRequestsPath = @"C:\X230 File Share\Video Deletion Requests\Documentary Movies to Delete.txt";
        //private const string documentaryTvDeletionRequestsPath = @"C:\X230 File Share\Video Deletion Requests\Documentary TV to Delete.txt";

        public static Videos listOfDeletionRequests;

        private static List<string> movies = new List<string>();
        private static List<string> tvShows = new List<string>();
        private static List<string> documentaryMovies = new List<string>();
        private static List<string> documentaryTv = new List<string>();

        public static bool videosDeleted = false;
        public static bool filenameLengthWarningEnabled = false;

        public static void CheckForVideosToDelete()
        {
            GetExtensionAndPathOfVideos();
        }

        private static void GetExtensionAndPathOfVideos()
        {
            //get video files
            for (int i = 0; i <= listOfDeletionRequests.Movies.Length - 1; i++)
            {
                string[] moviesIndock = Directory.GetFiles(Program.moviesInDockPath + @"\", listOfDeletionRequests.Movies[i].Title + "*.*");
                movies.AddRange(moviesIndock);

                string[] moviesInExternal = Directory.GetFiles(Program.moviesInExternalDrivePath + @"\", listOfDeletionRequests.Movies[i].Title + "*.*");
                movies.AddRange(moviesInExternal);
            }

            movies = movies.Distinct().ToList();

            for (int i = 0; i <= listOfDeletionRequests.DocumentaryMovies.Length - 1; i++)
            {
                string[] docMoviesInExternal = Directory.GetFiles(Program.documentaryMoviesInExternalDrivePath + @"\", listOfDeletionRequests.DocumentaryMovies[i].Title + "*.*");

                documentaryMovies.AddRange(docMoviesInExternal);
            }

            //get directory folders
            foreach (Video directory in listOfDeletionRequests.TvShows)
            {
                if (Directory.Exists(Program.tvShowsInDockPath + "\\" + directory.Title))
                {
                    tvShows.Add(Program.tvShowsInDockPath + "\\" + directory.Title);
                }
                else if (Directory.Exists(Program.tvShowsInExternalDrivePath + "\\" + directory.Title))
                {
                    tvShows.Add(Program.tvShowsInExternalDrivePath + "\\" + directory.Title);
                }
            }

            foreach (Video directory in listOfDeletionRequests.DocumentaryTv)
            {
                if (Directory.Exists(Program.documentaryTvInExternalDrivePath + "\\" + directory.Title))
                {
                    documentaryTv.Add(Program.documentaryTvInExternalDrivePath + "\\" + directory.Title);
                }
            }
        }
     
        public static void DeleteVideos()
        {
            for (int i = 0; i < movies.Count; i++)
            {
                for (int j = 0; j < listOfDeletionRequests.Movies.Length; j++)
                {
                    // Removies year of movie, e.g (2019).
                    string tempStringWithRegex = Regex.Replace(Path.GetFileNameWithoutExtension(movies[i]), @" \(.*?\)", string.Empty);

                    if (tempStringWithRegex == listOfDeletionRequests.Movies[j].Title)
                    {
                        try
                        {
                            FileSystem.DeleteFile(movies[i], UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("The File: " + "'" + Path.GetFileName(movies[i]) + "'" + " has been successfully deleted.\n");
                            Console.ResetColor();
                            videosDeleted = true;
                        }
                        catch (Exception UnableToDeleteFileException)
                        {
                            Console.WriteLine("Unable to delete the file: " + "'" + movies[i] + "'" + "\n" + UnableToDeleteFileException);
                            Console.WriteLine("Attempting to delete via normal delete function.");
                            Exceptions.LogException(UnableToDeleteFileException);
                        }
                    }
                }
            }

            for (int i = 0; i < tvShows.Count; i++)
            {
                for (int j = 0; j < listOfDeletionRequests.TvShows.Length; j++)
                {
                    if (Path.GetFileNameWithoutExtension(tvShows[i]) == listOfDeletionRequests.TvShows[j].Title)
                    {
                        try
                        {
                            FileSystem.DeleteDirectory(tvShows[i], UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("The File: " + "'" + Path.GetFileName(tvShows[i]) + "'" + " has been successfully deleted.\n");
                            Console.ResetColor();
                            videosDeleted = true;
                        }
                        catch (Exception UnableToDeleteFileException)
                        {
                            Console.WriteLine("Unable to delete the directory: " + "'" + tvShows[i] + "'" + "\n" + UnableToDeleteFileException + "\n");
                            Console.WriteLine("The file may have already been deleted.\n");
                            Exceptions.LogException(UnableToDeleteFileException);
                        }
                    }
                }
            }

            for (int i = 0; i < documentaryMovies.Count; i++)
            {
                for (int j = 0; j < listOfDeletionRequests.DocumentaryMovies.Length; j++)
                {
                    string tempStringWithRegex = Regex.Replace(Path.GetFileNameWithoutExtension(documentaryMovies[i]), @" \(.*?\)", string.Empty);

                    if (tempStringWithRegex == listOfDeletionRequests.DocumentaryMovies[j].Title)
                    {
                        try
                        {
                            FileSystem.DeleteFile(documentaryMovies[i], UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("The File: " + "'" + Path.GetFileName(documentaryMovies[i]) + "'" + " has been successfully deleted.\n");
                            Console.ResetColor();
                            videosDeleted = true;
                        }
                        catch (Exception UnableToDeleteFileException)
                        {
                            Console.WriteLine("Unable to delete the file: " + "'" + documentaryMovies[i] + "'" + "\n" + UnableToDeleteFileException);
                            Exceptions.LogException(UnableToDeleteFileException);
                        }
                    }
                }
            }

            for (int i = 0; i < documentaryTv.Count; i++)
            {
                for (int j = 0; j < listOfDeletionRequests.DocumentaryTv.Length; j++)
                {
                    if (Path.GetFileNameWithoutExtension(documentaryTv[i]) == listOfDeletionRequests.DocumentaryTv[j].Title)
                    {
                        try
                        {
                            FileSystem.DeleteDirectory(documentaryTv[i], UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("The File: " + "'" + Path.GetFileName(documentaryTv[i]) + "'" + " has been successfully deleted.\n");
                            Console.ResetColor();
                            videosDeleted = true;
                        }
                        catch (Exception UnableToDeleteFileException)
                        {
                            Console.WriteLine("Unable to delete the directory: " + "'" + documentaryTv[i] + "'" + "\n" + UnableToDeleteFileException + "\n");
                            Console.WriteLine("The file may have already been deleted.\n");
                            Exceptions.LogException(UnableToDeleteFileException);
                        }
                    }
                }
            }
        }
    }
}
