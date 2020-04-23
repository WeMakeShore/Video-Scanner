using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;
using ExceptionHandler;
using GetVideoData;

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
                for (int j = 0; j < Program.docMoviePaths.Count; j++)
                {
                    string[] tempDocMovies = Directory.GetFiles(Program.docMoviePaths[j] + @"\", listOfDeletionRequests.DocumentaryMovies[i].Title + "*.*");
                    documentaryMovies.AddRange(tempDocMovies);
                }
            }

            for (int i = 0; i < listOfDeletionRequests.TvShows.Length; i++)
            {
                for (int j = 0; j < Program.tvShowPaths.Count; j++)
                {
                    string year = listOfDeletionRequests.TvShows[i].Year != 0 ? " (" + listOfDeletionRequests.TvShows[i].Year + ")" : string.Empty;

                    if (Directory.Exists(Program.tvShowPaths[j] + @"\" + listOfDeletionRequests.TvShows[i].Title + year))
                    {
                        tvShows.Add(Program.tvShowPaths[j] + @"\" + listOfDeletionRequests.TvShows[i].Title + year);
                    }
                }
            }

            for (int i = 0; i < listOfDeletionRequests.DocumentaryTv.Length; i++)
            {
                for (int j = 0; j < Program.docTvShowPaths.Count; j++)
                {
                    string year = listOfDeletionRequests.DocumentaryTv[i].Year != 0 ? " (" + listOfDeletionRequests.DocumentaryTv[i].Year + ")" : string.Empty;

                    if (Directory.Exists(Program.docTvShowPaths[j] + @"\" + listOfDeletionRequests.DocumentaryTv[i].Title + year))
                    {
                        documentaryTv.Add(Program.docTvShowPaths[j] + @"\" + listOfDeletionRequests.DocumentaryTv[i].Title + year);
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
                    string title = String.Empty;
                    int year = 0;

                    MatchCollection matches = Regex.Matches(Path.GetFileName(movies[i]), GetFilesAndDirectories.fileRegex);

                    foreach (Match match in matches)
                    {
                        title = match.Groups[1].Value;
                        year = int.Parse(match.Groups[2].Value);
                    }

                    title = GetFilesAndDirectories.CheckTitle(title, movies[i], new FileInfo(movies[i]).Name, false);

                    if (title == listOfDeletionRequests.Movies[j].Title && year == listOfDeletionRequests.Movies[j].Year)
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
                    string title = String.Empty;
                    int year = 0;

                    MatchCollection matches = Regex.Matches(Path.GetFileName(tvShows[i]), GetFilesAndDirectories.directoryRegex);

                    foreach (Match match in matches)
                    {
                        title = match.Groups[1].Value;
                        year = int.Parse(match.Groups[2].Value);
                    }

                    title = GetFilesAndDirectories.CheckTitle(title, tvShows[i], new DirectoryInfo(tvShows[i]).Name, false);

                    if (title == listOfDeletionRequests.TvShows[j].Title && year == listOfDeletionRequests.TvShows[j].Year)
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
                    string title = String.Empty;
                    int year = 0;

                    MatchCollection matches = Regex.Matches(Path.GetFileName(documentaryMovies[i]), GetFilesAndDirectories.fileRegex);

                    foreach (Match match in matches)
                    {
                        title = match.Groups[1].Value;
                        year = int.Parse(match.Groups[2].Value);
                    }

                    title = GetFilesAndDirectories.CheckTitle(title, documentaryMovies[i], new FileInfo(documentaryMovies[i]).Name, false);

                    if (title == listOfDeletionRequests.DocumentaryMovies[j].Title && year == listOfDeletionRequests.DocumentaryMovies[j].Year)
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
                    string title = String.Empty;
                    int year = 0;

                    MatchCollection matches = Regex.Matches(Path.GetFileName(documentaryTv[i]), GetFilesAndDirectories.directoryRegex);

                    foreach (Match match in matches)
                    {
                        title = match.Groups[1].Value;
                        year = int.Parse(match.Groups[2].Value);
                    }

                    title = GetFilesAndDirectories.CheckTitle(title, documentaryTv[i], new DirectoryInfo(documentaryTv[i]).Name, false);

                    if (title == listOfDeletionRequests.DocumentaryTv[j].Title && year == listOfDeletionRequests.DocumentaryTv[j].Year)
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
