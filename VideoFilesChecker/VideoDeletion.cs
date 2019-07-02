using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;

//TODO: Downfall is being displayed twice.
//TODO: Add a directory deletion component. Deletes after 3 days, rather than going to the recycling bin.

namespace VideoFilesChecker
{
    static class VideoDeletion
    {
        private const string movieDeletionRequests = @"C:\X230 File Share\Video Deletion Requests\Movies to Delete.txt";
        private const string tvDeletionRequests = @"C:\X230 File Share\Video Deletion Requests\TV Shows to Delete.txt";
        private const string documentaryMovieDeletionRequests = @"C:\X230 File Share\Video Deletion Requests\Documentary Movies to Delete.txt";
        private const string documentaryTvDeletionRequests = @"C:\X230 File Share\Video Deletion Requests\Documentary TV to Delete.txt";

        private static List<string> movies = new List<string>();
        private static List<string> tvShows = new List<string>();
        private static List<string> documentaryMovies= new List<string>();
        private static List<string> documentaryTv = new List<string>();
        private static List<string> videosAndDirectoriesToDelete = new List<string>(); // TODO: Create variables for each type. Should prevent the bug from occuring.

        public static bool videosDeleted = false;
        public static bool filenameLengthWarningEnabled = false;

        public static void CheckForVideosToDelete()
        {
            Console.Clear();

            GetVideoDeletionRequests();

            GetExtensionAndPathOfVideos();

            foreach(string test in documentaryTv)
            {
                Console.WriteLine("Test: " + test);
            }
            Console.ReadKey();

            DeleteVideos();
        }

        private static void GetVideoDeletionRequests()
        {
            using (StreamReader sr = new StreamReader(movieDeletionRequests, true))
            {
                string tempVidString;

                while ((tempVidString = sr.ReadLine()) != null)
                {
                    if(tempVidString.Length < 3)
                    {
                        filenameLengthWarningEnabled = true;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("The file: "); Console.ForegroundColor = ConsoleColor.Red; Console.Write($"'{tempVidString}'"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write(" was not added as a video to delete as the filename length is less than 2.\n"); Console.ResetColor();
                        break;
                    }

                    if ((!string.IsNullOrWhiteSpace(tempVidString)))
                    {
                        videosAndDirectoriesToDelete.Add(tempVidString);
                    }
                }           
            }

            using (StreamReader sr = new StreamReader(tvDeletionRequests, true))
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
                        videosAndDirectoriesToDelete.Add(tempVidString);
                    }
                }
            }

            using (StreamReader sr = new StreamReader(documentaryMovieDeletionRequests, true))
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
                        videosAndDirectoriesToDelete.Add(tempVidString);
                    }
                }
            }

            using (StreamReader sr = new StreamReader(documentaryTvDeletionRequests, true))
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
                        videosAndDirectoriesToDelete.Add(tempVidString);
                    }
                }
            }
        }

        private static void GetExtensionAndPathOfVideos()
        {
            //get video files
            for (int i = 0; i <= videosAndDirectoriesToDelete.Count - 1; i++)
            {
                string[] moviesIndock = Directory.GetFiles(Program.moviesInDockPath + @"\", videosAndDirectoriesToDelete[i] + "*.*");
                movies.AddRange(moviesIndock);

                string[] moviesInExternal = Directory.GetFiles(Program.moviesInExternalDrivePath + @"\", videosAndDirectoriesToDelete[i] + "*.*");
                movies.AddRange(moviesInExternal);

                string[] docMoviesInExternal = Directory.GetFiles(Program.documentaryMoviesInExternalDrivePath + @"\", videosAndDirectoriesToDelete[i] + "*.*");
                documentaryMovies.AddRange(docMoviesInExternal);
            }

            //get directory folders
            foreach (string directory in videosAndDirectoriesToDelete)
            {
                if (Directory.Exists(Program.tvShowsInDockPath + "\\" + directory))
                {
                    tvShows.Add(Program.tvShowsInDockPath + "\\" + directory);
                    break;
                }
                else if (Directory.Exists(Program.tvShowsInExternalDrivePath + "\\" + directory))
                {
                    tvShows.Add(Program.tvShowsInExternalDrivePath + "\\" + directory);
                    break;
                }
                else if (Directory.Exists(Program.documentaryTvInExternalDrivePath + "\\" + directory))
                {
                    documentaryTv.Add(Program.documentaryTvInExternalDrivePath + "\\" + directory);
                    break;                  
                }
            }
        }

        private static void DeleteVideos()
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
                            Console.WriteLine("True");
                            //Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(movies[i], UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                            Console.ForegroundColor = ConsoleColor.DarkGreen;

                            Console.WriteLine("The File: " + "'" + Path.GetFileName(movies[i]) + "'" + " has been succesfully deleted.\n");
                            Console.ResetColor();
                            videosDeleted = true;
                        }
                        catch (Exception UnableToDeleteFileException)
                        {
                            Console.WriteLine("Unable to delete the file: " + "'" + movies[i] + "'" + "\n" + UnableToDeleteFileException);
                        }

                        break;
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
                            Console.WriteLine("True 2");
                            //Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(tvShows[i], UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("The File: " + "'" + Path.GetFileName(tvShows[i]) + "'" + " has been succesfully deleted.\n");
                            Console.ResetColor();
                            videosDeleted = true;
                        }
                        catch (Exception UnableToDeleteFileException)
                        {
                            Console.WriteLine("Unable to delete the file: " + "'" + tvShows[i] + "'" + "\n" + UnableToDeleteFileException);
                        }

                        break;
                    }
                }
            }

            for (int i = 0; i < documentaryMovies.Count; i++)
            {
                for (int j = 0; j < Program.listOfDocumentaryMovies.Count; j++)
                {
                    string tempStringWithRegex = Regex.Replace(Path.GetFileNameWithoutExtension(movies[i]), @" \(.*?\)", string.Empty);

                    if (tempStringWithRegex == Program.listOfDocumentaryMovies[j])
                    {
                        try
                        {
                            Console.WriteLine("True 3");
                            //Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(documentaryMovies[i], UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("The File: " + "'" + Path.GetFileName(documentaryMovies[i]) + "'" + " has been succesfully deleted.\n");
                            Console.ResetColor();
                            videosDeleted = true;
                        }
                        catch (Exception UnableToDeleteFileException)
                        {
                            Console.WriteLine("Unable to delete the file: " + "'" + documentaryMovies[i] + "'" + "\n" + UnableToDeleteFileException);
                        }

                        break;
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
                            //Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(documentaryTv[i], UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("The File: " + "'" + Path.GetFileName(documentaryTv[i]) + "'" + " has been succesfully deleted.\n");
                            Console.ResetColor();
                            videosDeleted = true;
                        }
                        catch (Exception UnableToDeleteFileException)
                        {
                            Console.WriteLine("Unable to delete the file: " + "'" + documentaryTv[i] + "'" + "\n" + UnableToDeleteFileException);
                        }

                        break;
                    }
                }
            }
        }
    }
}
