using System;
using System.IO;
using System.Linq;
using VideoChecking;

namespace VideoFileChecker
{
    public static class VideoChecker
    {
        public static bool VideoChangesFound()
        {
            // Compare movie files
            bool movieFilesAreEqual = File.ReadAllLines(Generate.documentsLocation + "Movies.txt")
                .SequenceEqual(File.ReadAllLines(Generate.previousDocumentsLocation + "Movies.txt"));

            // Compare tv shows
            bool tvShowFilesAreEqual = File.ReadAllLines(Generate.documentsLocation + "TV Shows.txt")
                .SequenceEqual(File.ReadAllLines(Generate.previousDocumentsLocation + "TV Shows.txt"));

            // Compare documentary movies
            bool documentaryMoviesAreEqual = File.ReadAllLines(Generate.documentsLocation + "Documentary Movies.txt")
                .SequenceEqual(File.ReadAllLines(Generate.previousDocumentsLocation + "Documentary Movies.txt"));

            // Compare documentary tv shows
            bool documentaryTvShowsAreEqual = File.ReadAllLines(Generate.documentsLocation + "Documentary TV.txt")
                .SequenceEqual(File.ReadAllLines(Generate.previousDocumentsLocation + "Documentary TV.txt"));

            if (movieFilesAreEqual == true || tvShowFilesAreEqual == true || documentaryMoviesAreEqual == true || documentaryTvShowsAreEqual == true)
            {
                return false;
            } else
            {
                return true;
            }
        }
    }
}
