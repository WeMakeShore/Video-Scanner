using System;
using System.Net;
using VideoChecking;

public class Plex
{
    public static void RefreshPlexLibraries()
    {
        WebRequest request = WebRequest.Create(Program.settings.PlexLibrariesRefreshString);

        // Get the response.  
        WebResponse response = request.GetResponse();

        // Display the status.  
        Console.Write("Plex Refresh All Libraries Status: ");
        Console.ForegroundColor = (((HttpWebResponse)response).StatusDescription == "OK") ? ConsoleColor.Green : ConsoleColor.Red;
        Console.Write(((HttpWebResponse)response).StatusDescription);

        Console.WriteLine(Environment.NewLine + Environment.NewLine);

        Console.ResetColor();

        // Close the response.  
        response.Close();
    }
}