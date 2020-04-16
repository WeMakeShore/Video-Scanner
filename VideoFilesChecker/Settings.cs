using System;
using System.IO;
using Newtonsoft.Json;

public class Settings
{
    public string[] VideoPaths { get; set; }
    public string VideosJsonPath { get; set; }
    public string ExceptionLoggerPath { get; set; }
    public string PostUrl { get; set; }
    public string GetUrl { get; set; }
    public string BearerToken { get; set; }
    public string PlexLibrariesRefreshString { get; set; }
    public bool RefreshLibraries { get; set; }

    public Settings(string[] videoPaths, string videosJsonPath, string exceptionLoggerPath, string postUrl, string getUrl, string bearerToken, string plexLibrariesRefreshString
        , bool refreshLibraries)
    {
        this.VideoPaths = videoPaths;
        this.VideosJsonPath = videosJsonPath;
        this.ExceptionLoggerPath = exceptionLoggerPath;
        this.PostUrl = postUrl;
        this.GetUrl = getUrl;
        this.BearerToken = bearerToken;
        this.PlexLibrariesRefreshString = plexLibrariesRefreshString;
        this.RefreshLibraries = refreshLibraries;
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

