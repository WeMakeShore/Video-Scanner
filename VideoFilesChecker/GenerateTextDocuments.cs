using System;
using System.IO;

namespace VideoFilesChecker
{
    public static class Generate
    {
        private const string documentsLocation = @"C:\X230 File Share\";

        public static void GenerateDocuments()
        {
            GenerateMovieDocument();

            GenerateTvShowsDocument();

            GenerateDocumentaryMovieDocument();

            GenerateDocumentaryTvDocument();
        }

        private static void GenerateMovieDocument()
        {
            string movieDocumentLocation = documentsLocation + @"Movies.txt";

            using (StreamWriter sw = new StreamWriter(movieDocumentLocation))
            {
                foreach(string movie in Program.listOfMovies)
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
                foreach (string tvShow in Program.listOfTvShows)
                {
                    sw.WriteLine(tvShow);
                }
            }
        }

        private static void GenerateDocumentaryMovieDocument()
        {
            string documentaryDocumentLocation = documentsLocation + @"Documentary Movies.txt";

            using (StreamWriter sw = new StreamWriter(documentaryDocumentLocation))
            {
                foreach(string documentaryMovie in Program.listOfDocumentaryMovies)
                {
                    sw.WriteLine(documentaryMovie);
                }
            }
        }

        private static void GenerateDocumentaryTvDocument()
        {
            string documentaryTvLocation = documentsLocation + @"Documentary TV.txt";

            using (StreamWriter sw = new StreamWriter(documentaryTvLocation))
            {
                foreach(string documentaryTv in Program.listOfDocumentaryTv)
                {
                    sw.WriteLine(documentaryTv);
                }
            }
        }

    }
}
