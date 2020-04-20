using System;
using System.Net;
using VideoChecking;

public class Plex
{
    public static void RefreshPlexLibraries()
    {
        WebRequest request = WebRequest.Create(Program.settings.PlexLibrariesRefreshString);

        request.Timeout = 10000;

        try
        {
            // Get the response.  
            WebResponse response = request.GetResponse();

            // Display the status.  
            Console.Write("\nPlex Refresh All Libraries Status: ");
            Console.ForegroundColor = (((HttpWebResponse)response).StatusDescription == "OK") ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write(((HttpWebResponse)response).StatusDescription);

            // Close the response.  
            response.Close();

        } catch (WebException UnableToRefreshPlexLibrariesException)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Unable to connect to Plex API. " + "\n" + UnableToRefreshPlexLibrariesException);
        }

        Console.ResetColor();
        Console.WriteLine(Environment.NewLine);
    }
}