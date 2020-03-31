using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using VideoChecking;
using Newtonsoft.Json;
using VideoFilesChecker;

/* Get files and directories and stores the results to videos.json */

namespace GetVideoData
{
    public static class GetFilesAndDirectories
    {
        private static readonly string yearRegex = @"(.+)\s\((\d{4})\)\.";

        public static void UpdateVideosAndDirectories()
        {
            GetMovies();

            GetTvShows();

            GetDocumentaryMovies();

            GetDocumentaryTv();

            OrderEntries();

            GenerateJsonVideoFile();
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

        private static void ScanDirectoryForFiles(string directoryPath)
        {
            dynamic itemData = GetItemLocationAndType(directoryPath);
            string driveLocation = itemData.driveLocation;
            string category = itemData.category;

            if (Directory.Exists(directoryPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);
                foreach (FileInfo f in dirInfo.GetFiles().Where(m => m.Extension != ".srt"))
                {
                    string title = String.Empty;
                    string year = String.Empty;

                    MatchCollection regexYearResult = Regex.Matches(f.Name, yearRegex);

                    foreach (Match match in regexYearResult)
                    {
                        title = match.Groups[1].Value;
                        year = match.Groups[2].Value;
                    }

                    if (String.IsNullOrEmpty(title))
                    {
                        Console.WriteLine($"Warning - Title is null or empty. ({directoryPath}\\{f.Name})\n");
                        title = Path.GetFileNameWithoutExtension(f.Name);
                    }

                    switch (category)
                    {
                        case "Movie": Program.listOfMovies.Add(new Video(title, year, f.Length, driveLocation, f.CreationTime)); break;
                        case "Documentary Movie": Program.listOfDocumentaryMovies.Add(new Video(title, year, f.Length, driveLocation, f.CreationTime)); break;
                    }
                }
            }
        }

        private static void ScanDirectory(string directoryPath) // TODO: Add driveLocation and category to Video object?
        {
            dynamic itemData = GetItemLocationAndType(directoryPath);
            string driveLocation = itemData.driveLocation;
            string category = itemData.category;

            if (Directory.Exists(directoryPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);
                foreach (DirectoryInfo d in dirInfo.GetDirectories().Where(m => m.Extension != ".srt"))
                {
                    string title = d.Name;
                    string year = String.Empty;
                    long size = d.EnumerateFiles("*", SearchOption.AllDirectories).Sum(fi => fi.Length);

                    string[] subdirectoryEntries = Directory.GetDirectories(d.FullName);

                    switch (category)
                    {
                        case "TV Show": Program.listOfTvShows.Add(new Video(title, year, size, driveLocation, d.CreationTime)); break;
                        case "Documentary TV": Program.listOfDocumentaryTv.Add(new Video(title, year, size, driveLocation, d.CreationTime)); break;
                    }
                }
            }
        }

        private static object GetItemLocationAndType(string directoryPath)
        {
            string driveLocation = String.Empty;
            string category = String.Empty;

            switch (directoryPath)
            {
                case var moviesDockPath when directoryPath == Program.settings.MoviesDockPath:
                    driveLocation = "Movies (Dock)"; category = "Movie"; break;
                case var moviesExtDrivePath when directoryPath == Program.settings.MoviesExtDrivePath:
                    driveLocation = "Movies (External Hard Drive)"; category = "Movie"; break;
                case var tvShowsDockPath when directoryPath == Program.settings.TvShowsDockPath:
                    driveLocation = "TV Shows (Dock)"; category = "TV Show"; break;
                case var tvShowsExtPath when directoryPath == Program.settings.TvShowsExtDrivePath:
                    driveLocation = "TV Shows (External Hard Drive)"; category = "TV Show"; break;
                case var documentaryMoviesExtPath when directoryPath == Program.settings.DocMoviesExtDrivePath:
                    driveLocation = "Documentary Movies (External Hard Drive)"; category = "Documentary Movie"; break;
                case var documentaryTvExtPath when directoryPath == Program.settings.DocTvExtDrivePath:
                    driveLocation = "Documentary TV (External Hard Drive)"; category = "Documentary TV"; break;
            }

            return new
            {
                driveLocation = driveLocation,
                category = category
            };
        }

        private static void GetMovies()
        {
            Console.WriteLine(Program.moviesDockPath);

            ScanDirectoryForFiles(Program.moviesDockPath);
            ScanDirectoryForFiles(Program.moviesExtDrivePath);
        }

        private static void GetTvShows()
        {
            ScanDirectory(Program.tvShowsDockPath);
            ScanDirectory(Program.tvShowsExtDrivePath);
        }

        private static void GetDocumentaryMovies()
        {
            ScanDirectoryForFiles(Program.docMoviesExtDrivePath);
        }

        private static void GetDocumentaryTv()
        {
            ScanDirectory(Program.docTvShowsExtDrivePath);
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
            Drives drives = Drives.GetAvailableDriveSpace();;

            string videoData = JsonConvert.SerializeObject(new
            {
                DockHardDrive = drives.DockHardDrive,
                ExternalHardDrive = drives.ExternalHardDrive,
                Movies = Program.listOfMovies,
                TvShows = Program.listOfTvShows,
                DocumentaryMovies = Program.listOfDocumentaryMovies,
                DocumentaryTv = Program.listOfDocumentaryTv
            }, Formatting.Indented);

            if (Directory.Exists(Path.GetDirectoryName(Program.settings.VideosJsonPath)))
            {
                File.WriteAllText(Program.settings.VideosJsonPath, videoData);
            } else
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

        public static Videos GetDeserializedJsonVideoFileData()
        {
            string serializedData = String.Empty;

            if (File.Exists(Program.settings.VideosJsonPath))
            {
                serializedData = File.ReadAllText(Program.settings.VideosJsonPath);
            } else
            {
                throw new FileNotFoundException();
            }

            var deserializedData = JsonConvert.DeserializeObject<Videos>(serializedData);

            return deserializedData;
        }
    }
}