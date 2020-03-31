using System;
using VideoChecking;

public class Logger
{
    public static void PrintGETData()
    {
        LogMoviesForDeletion();

        LogTvShowsForDeletion();

        LogDocumentaryMoviesForDeletion();

        LogDocumentaryTvForDeletion();
    }

    private static void LogMoviesForDeletion()
    {
        string lineBreak = VideoDeletion.listOfDeletionRequests.Movies.Length > 0 ? "\n" : "";

        foreach (Video movie in VideoDeletion.listOfDeletionRequests.Movies)
        {
            Console.WriteLine("GET [movie]: " + movie.Title);
        }
    }

    private static void LogTvShowsForDeletion()
    {
        string lineBreak = VideoDeletion.listOfDeletionRequests.TvShows.Length > 0 ? "\n" : "";

        foreach (Video tvShow in VideoDeletion.listOfDeletionRequests.TvShows)
        {
            Console.WriteLine("GET [TV Show]: " + tvShow + lineBreak);
        }
    }

    private static void LogDocumentaryMoviesForDeletion()
    {
        string lineBreak = VideoDeletion.listOfDeletionRequests.DocumentaryMovies.Length > 0 ? "\n" : "";

        foreach (Video documentaryMovies in VideoDeletion.listOfDeletionRequests.DocumentaryMovies)
        {
            Console.WriteLine("GET [Documentary Movies]: " + documentaryMovies + lineBreak);
        }
    }

    private static void LogDocumentaryTvForDeletion()
    {
        string lineBreak = VideoDeletion.listOfDeletionRequests.DocumentaryMovies.Length > 0 ? "\n" : "";

        foreach (Video documentaryTv in VideoDeletion.listOfDeletionRequests.DocumentaryTv)
        {
            Console.WriteLine("GET [Documentary TV]: " + documentaryTv + lineBreak);
        }

        if (VideoDeletion.listOfDeletionRequests.DocumentaryTv.Length > 0)
        {
            Console.WriteLine();
        }
    }
}

