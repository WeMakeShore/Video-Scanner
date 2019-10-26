﻿using System;
using System.IO;
using ExceptionHandler;

namespace VideoChecking
{
    public static class Generate
    {
        public const string documentsLocation = @"C:\X230 File Share\";
        public const string previousDocumentsLocation = @"C:\X230 File Share\before-scan\";

        public static void GetVideosBeforeUpdate()
        {
            try
            {
                File.Copy(documentsLocation + "Movies.txt", previousDocumentsLocation + "Movies.txt", true);
                File.Copy(documentsLocation + "TV Shows.txt", previousDocumentsLocation + "TV Shows.txt", true);
                File.Copy(documentsLocation + "Documentary Movies.txt", previousDocumentsLocation + "Documentary Movies.txt", true);
                File.Copy(documentsLocation + "Documentary TV.txt", previousDocumentsLocation + "Documentary TV.txt", true);
            } catch (Exception UnableToGetVideosBeforeUpdateException)
            {
                Console.WriteLine("An Error occurred attempting copying previously created files.\n" + UnableToGetVideosBeforeUpdateException);
                Exceptions.LogException(UnableToGetVideosBeforeUpdateException);
            }
        }

        public static void GenerateDocuments()
        {
            GenerateMovieDocument();

            GenerateTvShowsDocument();

            GenerateDocumentaryMovieDocument();

            GenerateDocumentaryTvDocument();
        }

        private static void GenerateMovieDocument()
        {
            string movieDocumentLocation = documentsLocation + "Movies.txt";

            using (StreamWriter sw = new StreamWriter(movieDocumentLocation))
            {
                foreach(string movie in Program.listofMoviesWithoutFilePath)
                {
                    sw.WriteLine(movie);
                }
            }
        }

        private static void GenerateTvShowsDocument()
        {
            string tvShowsDocumentLocation = documentsLocation + "TV Shows.txt";

            using (StreamWriter sw = new StreamWriter(tvShowsDocumentLocation))
            {
                foreach (string tvShow in Program.listOfTvShowsWithoutFilePath)
                {
                    sw.WriteLine(tvShow);
                }
            }
        }

        private static void GenerateDocumentaryMovieDocument()
        {
            string documentaryDocumentLocation = documentsLocation + "Documentary Movies.txt";

            using (StreamWriter sw = new StreamWriter(documentaryDocumentLocation))
            {
                foreach(string documentaryMovie in Program.listOfDocumentaryMoviesWithoutFilePath)
                {
                    sw.WriteLine(documentaryMovie);
                }
            }
        }

        private static void GenerateDocumentaryTvDocument()
        {
            string documentaryTvLocation = documentsLocation + "Documentary TV.txt";

            using (StreamWriter sw = new StreamWriter(documentaryTvLocation))
            {
                foreach(string documentaryTv in Program.listOfDocumentaryTvShowsWithoutFilePath)
                {
                    sw.WriteLine(documentaryTv);
                }
            }
        }

    }
}
