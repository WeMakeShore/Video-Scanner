using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using VideoChecking;
using Newtonsoft.Json;
using VideoFilesChecker;
using VideoFilesChecker.Drive;
using System.Collections.Generic;

/* Get files and directories and stores the results to videos.json */

namespace GetVideoData
{
    public static class GetFilesAndDirectories
    {
        public const string fileRegex = @"(.+)\s\((\d{4})\)\.";
        public const string directoryRegex = @"(.+)\s\((\d{4})\)";
        public static void UpdateVideosAndDirectories()
        {
            AssignVideoPathTypes();

            GetMovies();

            GetTvShows();

            GetDocumentaryMovies();

            GetDocumentaryTv();

            OrderEntries();

            GenerateJsonVideoFile();
        }

        private static void AssignVideoPathTypes()
        {
            for (int i = 0; i < Program.settings.VideoPaths.Length; i++)
            {
                switch (new DirectoryInfo(Program.settings.VideoPaths[i]).Name)
                {
                    case "Movies": Program.moviePaths.Add(Program.settings.VideoPaths[i]); break;
                    case "TV Shows": Program.tvShowPaths.Add(Program.settings.VideoPaths[i]); break;
                    case "Documentary Movies": Program.docMoviePaths.Add(Program.settings.VideoPaths[i]); break;
                    case "Documentary TV": Program.docTvShowPaths.Add(Program.settings.VideoPaths[i]); break;
                    default: throw new Exception("Invalid VideoPath");
                }
            }
        }

        //public static void GetVideosBeforeUpdate()
        //{
        //    try
        //    {
        //        File.Copy(documentsLocation + "Movies.txt", previousDocumentsLocation + "Movies.txt", true);
        //        File.Copy(documentsLocation + "TV Shows.txt", previousDocumentsLocation + "TV Shows.txt", true);
        //        File.Copy(documentsLocation + "Documentary Movies.txt", previousDocumentsLocation + "Documentary Movies.txt", true);
        //        File.Copy(documentsLocation + "Documentary TV.txt", previousDocumentsLocation + "Documentary TV.txt", true);
        //    }
        //    catch (Exception UnableToGetVideosBeforeUpdateException)
        //    {
        //        Console.WriteLine("An Error occurred attempting copying previously created files.\n" + UnableToGetVideosBeforeUpdateException);
        //        Exceptions.LogException(UnableToGetVideosBeforeUpdateException);
        //    }
        //}

        private static void ScanDirectoryForFiles(string directoryPath, string category)
        {
            string driveLocation = directoryPath.Contains("(External Hard Drive)") ? "External Hard Drive" : "Dock";

            if (Directory.Exists(directoryPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);
                foreach (FileInfo f in dirInfo.GetFiles().Where(m => m.Extension != ".srt"))
                {
                    string title = String.Empty;
                    int year = 0;

                    MatchCollection regexYearResult = Regex.Matches(f.Name, fileRegex);

                    foreach (Match match in regexYearResult)
                    {
                        title = match.Groups[1].Value;
                        year = int.Parse(match.Groups[2].Value);
                    }

                    title = CheckTitle(title, directoryPath, f.Name);
                    CheckYear(year, directoryPath, f.Name);

                    switch (category)
                    {
                        case "Movie": Program.listOfMovies.Add(new Video(title, year, f.Length, driveLocation, f.CreationTime)); break;
                        case "Documentary Movie": Program.listOfDocumentaryMovies.Add(new Video(title, year, f.Length, driveLocation, f.CreationTime)); break;
                    }
                }
            }
        }

        private static void ScanDirectory(string directoryPath, string category)
        {
            string driveLocation = directoryPath.Contains("(External Hard Drive)") ? "External Hard Drive" : "Dock";

            List<Series> series = new List<Series>();

            if (Directory.Exists(directoryPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);
                foreach (DirectoryInfo d in dirInfo.GetDirectories().Where(m => m.Extension != ".srt"))
                {
                    string title = String.Empty;
                    int year = 0;

                    MatchCollection regexYearResult = Regex.Matches(d.Name, directoryRegex);

                    foreach (Match match in regexYearResult)
                    {
                        title = match.Groups[1].Value;
                        year = int.Parse(match.Groups[2].Value);
                    }

                    title = CheckTitle(title, directoryPath, d.Name);
                    CheckYear(year, directoryPath, d.Name);

                    long size = d.EnumerateFiles("*", SearchOption.AllDirectories).Sum(fi => fi.Length);

                    string[] subdirectoryEntries = Directory.GetDirectories(d.FullName);
                    string dirName = String.Empty;

                    foreach (var seriesDir in subdirectoryEntries)
                    {
                        List<Episode> Episodes = new List<Episode>();
                        dirName = seriesDir;

                        if (Directory.Exists(seriesDir))
                        {
                            DirectoryInfo dirEpisodeInfo = new DirectoryInfo(seriesDir);
                            foreach (FileInfo f in dirEpisodeInfo.GetFiles().Where(m => m.Extension != ".srt"))
                            {
                                string seriesTitle = CheckTitle(f.Name, directoryPath, f.Name);

                                switch (category)
                                {
                                   case "TV Show": Episodes.Add(new Episode(seriesTitle, f.Length, f.CreationTime)); break;
                                   case "Documentary TV": Episodes.Add(new Episode(seriesTitle, f.Length, f.CreationTime)); break;
                                }
                            }
                            series.Add(new Series(Path.GetFileName(seriesDir), dirEpisodeInfo.CreationTime, Episodes.ToArray()));
                        }          
                    }

                    if (category == "TV Show") {
                        Program.listOfTvShows.Add(new Show(title, year, size, driveLocation, d.CreationTime, series));
                    } else {
                        Program.listOfDocumentaryTv.Add(new Show(title, year, size, driveLocation, d.CreationTime, series));
                    }
                }
            }
        }

        public static string CheckTitle(string title, string path, string name, bool warnings = true)
        {
            if (String.IsNullOrEmpty(title))
            {
                if (warnings)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Warning - Title is null or empty. ({path}\\{name})\n");
                    Console.ResetColor();
                }
                return name;
            }
            return Path.GetFileNameWithoutExtension(title);
        }

        private static void CheckYear(int year, string path, string name, bool warnings = true)
        {
            if (year == 0 && warnings)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Waring - Year is 0. ({path}\\{name})\n");
                Console.ResetColor();
            }
        }

        private static void GetMovies()
        {
            for (int i = 0; i < Program.moviePaths.Count; i++)
            {
                ScanDirectoryForFiles(Program.moviePaths[i], "Movie");
            }
        }

        private static void GetTvShows()
        {
            for (int i = 0; i < Program.tvShowPaths.Count; i++)
            {
                ScanDirectory(Program.tvShowPaths[i], "TV Show");
            }
        }

        private static void GetDocumentaryMovies()
        {
            for (int i = 0; i < Program.docMoviePaths.Count; i++)
            {
                ScanDirectoryForFiles(Program.docMoviePaths[i], "Documentary Movie");
            }
        }

        private static void GetDocumentaryTv()
        {
            for (int i = 0; i < Program.docTvShowPaths.Count; i++)
            {
                ScanDirectory(Program.docTvShowPaths[i], "Documentary TV");
            }
        }

        private static void OrderEntries()
        {
            Program.listOfMovies = Program.listOfMovies.OrderBy(li => li.Title).ToList();
            Program.listOfTvShows = Program.listOfTvShows.OrderBy(li => li.Title).ToList();
            Program.listOfDocumentaryMovies = Program.listOfDocumentaryMovies.OrderBy(li => li.Title).ToList();
            Program.listOfDocumentaryTv = Program.listOfDocumentaryTv.OrderBy(li => li.Title).ToList();
        }

        private static void GenerateJsonVideoFile()
        {
            List<DriveModel> drives = DrivesModel.GetDrives();

            string videoData = JsonConvert.SerializeObject(new
            {
                Drives = drives,
                Movies = Program.listOfMovies,
                TvShows = Program.listOfTvShows,
                DocumentaryMovies = Program.listOfDocumentaryMovies,
                DocumentaryTv = Program.listOfDocumentaryTv
            }, Formatting.Indented);

            if (Directory.Exists(Path.GetDirectoryName(Program.settings.VideosJsonPath)))
            {
                File.WriteAllText(Program.settings.VideosJsonPath, videoData);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        public static string GetSerializedJsonVideoFileData()
        {
            string serializedData;

            if (File.Exists(Program.settings.VideosJsonPath))
            {
                serializedData = File.ReadAllText(Program.settings.VideosJsonPath);
            }
            else
            {
                throw new FileNotFoundException();
            }

            return serializedData;
        }

        public static DataModel GetDeserializedJsonVideoFileData()
        {
            string serializedData = String.Empty;

            if (File.Exists(Program.settings.VideosJsonPath))
            {
                serializedData = File.ReadAllText(Program.settings.VideosJsonPath);
            }
            else
            {
                throw new FileNotFoundException();
            }

            var deserializedData = JsonConvert.DeserializeObject<DataModel>(serializedData);

            return deserializedData;
        }
    }
}