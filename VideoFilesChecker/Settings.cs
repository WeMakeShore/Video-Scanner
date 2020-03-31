using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class Settings
{
    public string MoviesDockPath { get; set; }
    public string MoviesExtDrivePath { get; set; }
    public string TvShowsDockPath { get; set; }
    public string TvShowsExtDrivePath { get; set; }
    public string DocMoviesExtDrivePath { get; set; }
    public string DocTvExtDrivePath { get; set; }
    public string VideosJsonPath { get; set; }
    public string ExceptionLoggerPath { get; set; }
    public string PostUrl { get; set; }
    public string GetUrl { get; set; }
    public string BearerToken { get; set; }
    public string PlexLibrariesRefreshString { get; set; }

    public Settings(string moviesDockPath, string moviesExtDrivePath, string tvShowsDockPath, string tvShowsExtDrivePath, string docMoviesExtDrivePath, string docTvExtDrivePath,
        string videosJsonPath, string exceptionLoggerPath, string postUrl, string getUrl, string bearerToken, string plexLibrariesRefreshString)
    {
        this.MoviesDockPath = moviesDockPath;
        this.MoviesExtDrivePath = moviesExtDrivePath;
        this.TvShowsDockPath = tvShowsDockPath;
        this.TvShowsExtDrivePath = tvShowsExtDrivePath;
        this.DocMoviesExtDrivePath = docMoviesExtDrivePath;
        this.DocTvExtDrivePath = docTvExtDrivePath;
        this.VideosJsonPath = videosJsonPath;
        this.ExceptionLoggerPath = exceptionLoggerPath;
        this.PostUrl = postUrl;
        this.GetUrl = getUrl;
        this.BearerToken = bearerToken;
        this.PlexLibrariesRefreshString = plexLibrariesRefreshString;
    }

    public static Settings GetSettings()
    {
        string serializedData = String.Empty;

        if (File.Exists(@"C:\X230 File Share\settings.json"))
        {
            serializedData = File.ReadAllText(@"C:\X230 File Share\settings.json");
        }
        else
        {
            throw new FileNotFoundException();
        }

        var deserializedData = JsonConvert.DeserializeObject<Settings>(serializedData);

        return deserializedData;
    }
}

