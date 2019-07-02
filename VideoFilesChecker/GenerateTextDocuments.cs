using System;
using System.Collections.Generic;
using System.IO;

namespace VideoFilesChecker
{
    public class GenerateTextDocuments
    {
        private const string documentsLocation = @"C:\X230 File Share\";

        private List<string> listOfMovies = new List<string>();
        private List<string> listOfDocumentaryMovies = new List<string>();
        private List<string> listOfDocumentaryTv = new List<string>();
        private List<string> listOfTvShows = new List<string>();

        public GenerateTextDocuments(List<string> listOfMovies, List<string> listOfTvShows, List<string> listOfDocumentaryMovies, List<string> listOfDocumentaryTv)
        {
            this.listOfMovies = listOfMovies;
            this.listOfTvShows = listOfTvShows;
            this.listOfDocumentaryMovies = listOfDocumentaryMovies;
            this.listOfDocumentaryTv = listOfDocumentaryTv;

            GenerateDocuments();
        }

        private void GenerateDocuments()
        {
            GenerateMovieDocument();

            GenerateTvShowsDocument();

            GenerateDocumentaryMovieDocument();

            GenerateDocumentaryTvDocument();
        }

        private void GenerateMovieDocument()
        {
            string movieDocumentLocation = documentsLocation + @"Movies.txt";

            using (StreamWriter sw = new StreamWriter(movieDocumentLocation))
            {
                foreach(string movie in listOfMovies)
                {
                    sw.WriteLine(movie);
                }
            }
        }

        private void GenerateTvShowsDocument()
        {
            string tvShowsDocumentLocation = documentsLocation + "TV Shows.txt";

            using (StreamWriter sw = new StreamWriter(tvShowsDocumentLocation))
            {
                foreach (string tvShow in listOfTvShows)
                {
                    sw.WriteLine(tvShow);
                }
            }
        }

        private void GenerateDocumentaryMovieDocument()
        {
            string documentaryDocumentLocation = documentsLocation + @"Documentary Movies.txt";

            using (StreamWriter sw = new StreamWriter(documentaryDocumentLocation))
            {
                foreach(string documentaryMovie in listOfDocumentaryMovies)
                {
                    sw.WriteLine(documentaryMovie);
                }
            }
        }

        private void GenerateDocumentaryTvDocument()
        {
            string documentaryTvLocation = documentsLocation + @"Documentary TV.txt";

            using (StreamWriter sw = new StreamWriter(documentaryTvLocation))
            {
                foreach(string documentaryTv in listOfDocumentaryTv)
                {
                    sw.WriteLine(documentaryTv);
                }
            }
        }

    }
}
