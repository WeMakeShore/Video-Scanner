using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;
using ExceptionHandler;

namespace VideoChecking
{
    static class VideoDeletion
    {
        public static DataModel listOfDeletionRequests;

        private static List<string> movies = new List<string>();
        private static List<string> tvShows = new List<string>();
        private static List<string> documentaryMovies = new List<string>();
        private static List<string> documentaryTv = new List<string>();

        public static bool videosDeleted = false;
        public static bool filenameLengthWarningEnabled = false;

        public static void CheckForVideosToDelete()
        {
            for (int i = 0; i <= listOfDeletionRequests.Movies.Length - 1; i++)
            {
                for (int j = 0; j < Program.moviePaths.Count; j++)
                {
                    string[] tempMovies = Directory.GetFiles(Program.moviePaths[j] + @"\", listOfDeletionRequests.Movies[i].Title + "*.*");
                    movies.AddRange(tempMovies);
                }
            }

            for (int i = 0; i <= listOfDeletionRequests.DocumentaryMovies.Length - 1; i++)
            {
                for (int j = 0; j < Program.docMoviesPath.Count; j++)
                {
                    string[] tempDocMovies = Directory.GetFiles(Program.docMoviesPath[j] + @"\", listOfDeletionRequests.DocumentaryMovies[i].Title + "*.*");
                    documentaryMovies.AddRange(tempDocMovies);
                }
            }

            for (int i = 0; i < listOfDeletionRequests.TvShows.Length; i++)
            {
                for (int j = 0; j < Program.tvShowPaths.Count; j++)
                {
                    if (Directory.Exists(Program.tvShowPaths[j] + "\\" + listOfDeletionRequests.TvShows[i].Title))
                    {
                        tvShows.Add(Program.tvShowPaths[j] + "\\" + listOfDeletionRequests.TvShows[i].Title);
                    }
                }
            }

            for (int i = 0; i < listOfDeletionRequests.DocumentaryTv.Length; i++)
            {
                for (int j = 0; j < Program.docTvShowsPath.Count; j++)
                {
                    if (Directory.Exists(Program.docTvShowsPath[j] + "\\" + listOfDeletionRequests.DocumentaryTv[i].Title))
                    {
                        tvShows.Add(Program.docTvShowsPath[j] + "\\" + listOfDeletionRequests.DocumentaryTv[i].Title);
                    }
                }
            }
        }

        public static void DeleteVideos()
        {
            for (int i = 0; i < movies.Count; i++)
            {
                for (int j = 0; j < listOfDeletionRequests.Movies.Length; j++)
                {
                    // Removes year of movie, e.g (2019).
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
                    if (new DirectoryInfo(tvShows[i]).Name == listOfDeletionRequests.TvShows[j].Title)
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
                    if (new DirectoryInfo(documentaryTv[i]).Name == listOfDeletionRequests.DocumentaryTv[j].Title)
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
