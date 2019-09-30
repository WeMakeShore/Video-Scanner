﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;

namespace VideoFilesChecker
{
    static class VideoDeletion
    {
        private const string movieDeletionRequestsPath = @"C:\X230 File Share\Video Deletion Requests\Movies to Delete.txt";
        private const string tvDeletionRequestsPath = @"C:\X230 File Share\Video Deletion Requests\TV Shows to Delete.txt";
        private const string documentaryMovieDeletionRequestsPath = @"C:\X230 File Share\Video Deletion Requests\Documentary Movies to Delete.txt";
        private const string documentaryTvDeletionRequestsPath = @"C:\X230 File Share\Video Deletion Requests\Documentary TV to Delete.txt";

        private static List<string> movieDeletionRequests = new List<string>();
        private static List<string> tvShowDeletionRequests = new List<string>();
        private static List<string> documentaryMovieDeletionRequests = new List<string>();
        private static List<string> docummentaryTvDeletionRequests = new List<string>();

        private static List<string> movies = new List<string>();
        private static List<string> tvShows = new List<string>();
        private static List<string> documentaryMovies = new List<string>();
        private static List<string> documentaryTv = new List<string>();

        public static bool videosDeleted = false;
        public static bool filenameLengthWarningEnabled = false;

        private static bool firstScan = true;

        public static void CheckForVideosToDelete()
        {

            if (firstScan == true)
            {
                Console.Clear();
                firstScan = false;
            }

            GetVideoDeletionRequests();

            GetExtensionAndPathOfVideos();
        }

        private static void GetVideoDeletionRequests()
        {
            using (StreamReader sr = new StreamReader(movieDeletionRequestsPath, true))
            {
                string tempVidString;

                while ((tempVidString = sr.ReadLine()) != null)
                {
                    if (tempVidString.Length < 3)
                    {
                        filenameLengthWarningEnabled = true;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("The file: "); Console.ForegroundColor = ConsoleColor.Red; Console.Write($"'{tempVidString}'"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write(" was not added as a video to delete as the filename length is less than 2.\n"); Console.ResetColor();
                        break;
                    }

                    if ((!string.IsNullOrWhiteSpace(tempVidString)))
                    {
                        movieDeletionRequests.Add(tempVidString);
                    }
                }
            }

            using (StreamReader sr = new StreamReader(tvDeletionRequestsPath, true))
            {
                string tempVidString;

                while ((tempVidString = sr.ReadLine()) != null)
                {
                    if (tempVidString.Length < 3)
                    {
                        filenameLengthWarningEnabled = true;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("The file: "); Console.ForegroundColor = ConsoleColor.Red; Console.Write($"'{tempVidString}'"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write(" was not added as a video to delete as the filename length is less than 2.\n"); Console.ResetColor();
                        break;
                    }

                    if ((!string.IsNullOrWhiteSpace(tempVidString)))
                    {
                        tvShowDeletionRequests.Add(tempVidString);
                    }
                }
            }

            using (StreamReader sr = new StreamReader(documentaryMovieDeletionRequestsPath, true))
            {
                string tempVidString;

                while ((tempVidString = sr.ReadLine()) != null)
                {
                    if (tempVidString.Length < 3)
                    {
                        filenameLengthWarningEnabled = true;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("The file: "); Console.ForegroundColor = ConsoleColor.Red; Console.Write($"'{tempVidString}'"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write(" was not added as a video to delete as the filename length is less than 2.\n"); Console.ResetColor();
                        break;
                    }

                    if ((!string.IsNullOrWhiteSpace(tempVidString)))
                    {
                        documentaryMovieDeletionRequests.Add(tempVidString);
                    }
                }
            }

            using (StreamReader sr = new StreamReader(documentaryTvDeletionRequestsPath, true))
            {
                string tempVidString;

                while ((tempVidString = sr.ReadLine()) != null)
                {
                    if (tempVidString.Length < 3)
                    {
                        filenameLengthWarningEnabled = true;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("The file: "); Console.ForegroundColor = ConsoleColor.Red; Console.Write($"'{tempVidString}'"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write(" was not added as a video to delete as the filename length is less than 2.\n"); Console.ResetColor();
                        break;
                    }

                    if ((!string.IsNullOrWhiteSpace(tempVidString)))
                    {
                        docummentaryTvDeletionRequests.Add(tempVidString);
                    }
                }
            }
        }

        private static void GetExtensionAndPathOfVideos()
        {
            //get video files
            for (int i = 0; i <= movieDeletionRequests.Count - 1; i++)
            {
                string[] moviesIndock = Directory.GetFiles(Program.moviesInDockPath + @"\", movieDeletionRequests[i] + "*.*");
                movies.AddRange(moviesIndock);

                string[] moviesInExternal = Directory.GetFiles(Program.moviesInExternalDrivePath + @"\", movieDeletionRequests[i] + "*.*");
                movies.AddRange(moviesInExternal);
            }

            for (int i = 0; i <= documentaryMovieDeletionRequests.Count - 1; i++)
            {
                string[] docMoviesInExternal = Directory.GetFiles(Program.documentaryMoviesInExternalDrivePath + @"\", documentaryMovieDeletionRequests[i] + "*.*");
                documentaryMovies.AddRange(docMoviesInExternal);
            }

            //get directory folders
            foreach (string directory in tvShowDeletionRequests)
            {
                if (Directory.Exists(Program.tvShowsInDockPath + "\\" + directory))
                {
                    tvShows.Add(Program.tvShowsInDockPath + "\\" + directory);
                }
                else if (Directory.Exists(Program.tvShowsInExternalDrivePath + "\\" + directory))
                {
                    tvShows.Add(Program.tvShowsInExternalDrivePath + "\\" + directory);
                }
            }

            foreach (string directory in docummentaryTvDeletionRequests)
            {
                if (Directory.Exists(Program.documentaryTvInExternalDrivePath + "\\" + directory))
                {
                    documentaryTv.Add(Program.documentaryTvInExternalDrivePath + "\\" + directory);
                }
            }
        }
     
        public static void DeleteVideos()
        {
            for (int i = 0; i < movies.Count; i++)
            {
                for (int j = 0; j < Program.listOfMovies.Count; j++)
                {
                    // Removies year of movie, e.g (2019).
                    string tempStringWithRegex = Regex.Replace(Path.GetFileNameWithoutExtension(movies[i]), @" \(.*?\)", string.Empty);

                    if (tempStringWithRegex == Program.listOfMovies[j])
                    {
                        try
                        {
                            Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(movies[i], UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("The File: " + "'" + Path.GetFileName(movies[i]) + "'" + " has been succesfully deleted.\n");
                            Console.ResetColor();
                            videosDeleted = true;
                        }
                        catch (Exception UnableToDeleteFileException)
                        {
                            Console.WriteLine("Unable to delete the file: " + "'" + movies[i] + "'" + "\n" + UnableToDeleteFileException);
                            Console.WriteLine("Attempting to delete via normal delete function.");
                        }
                    }
                }
            }

            for (int i = 0; i < tvShows.Count; i++)
            {
                for (int j = 0; j < Program.listOfTvShows.Count; j++)
                {
                    if (Path.GetFileNameWithoutExtension(tvShows[i]) == Program.listOfTvShows[j])
                    {
                        try
                        {
                            Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(tvShows[i], UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("The File: " + "'" + Path.GetFileName(tvShows[i]) + "'" + " has been succesfully deleted.\n");
                            Console.ResetColor();
                            videosDeleted = true;
                        }
                        catch (Exception UnableToDeleteFileException)
                        {
                            Console.WriteLine("Unable to delete the directory: " + "'" + tvShows[i] + "'" + "\n" + UnableToDeleteFileException + "\n");
                            Console.WriteLine("The file may have already been deleted.\n");
                        }
                    }
                }
            }

            for (int i = 0; i < documentaryMovies.Count; i++)
            {
                for (int j = 0; j < Program.listOfDocumentaryMovies.Count; j++)
                {
                    string tempStringWithRegex = Regex.Replace(Path.GetFileNameWithoutExtension(documentaryMovies[i]), @" \(.*?\)", string.Empty);

                    if (tempStringWithRegex == Program.listOfDocumentaryMovies[j])
                    {
                        try
                        {
                            Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(documentaryMovies[i], UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("The File: " + "'" + Path.GetFileName(documentaryMovies[i]) + "'" + " has been succesfully deleted.\n");
                            Console.ResetColor();
                            videosDeleted = true;
                        }
                        catch (Exception UnableToDeleteFileException)
                        {
                            Console.WriteLine("Unable to delete the file: " + "'" + documentaryMovies[i] + "'" + "\n" + UnableToDeleteFileException);
                        }
                    }
                }
            }

            for (int i = 0; i < documentaryTv.Count; i++)
            {
                for (int j = 0; j < Program.listOfDocumentaryTv.Count; j++)
                {
                    if (Path.GetFileNameWithoutExtension(documentaryTv[i]) == Program.listOfDocumentaryTv[j])
                    {
                        try
                        {
                            Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(documentaryTv[i], UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("The File: " + "'" + Path.GetFileName(documentaryTv[i]) + "'" + " has been succesfully deleted.\n");
                            Console.ResetColor();
                            videosDeleted = true;
                        }
                        catch (Exception UnableToDeleteFileException)
                        {
                            Console.WriteLine("Unable to delete the directory: " + "'" + documentaryTv[i] + "'" + "\n" + UnableToDeleteFileException + "\n");
                            Console.WriteLine("The file may have already been deleted.\n");
                        }
                    }
                }
            }
        }
    }
}
