using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ExceptionHandler;

namespace VideoChecking
{
    public static class CreateHttpRequest
    {
        private static HttpClient client = new HttpClient();

        public static async Task CreatePOSTRequest(Videos postData)
        {
            string postResponse = await PostData(postData);

            Console.WriteLine(postResponse);
        }

        public static async Task CreateGETRequest()
        {
            string getResponse = await GetJsonData();

            Console.WriteLine(getResponse);
        }

        private static async Task<string>PostData(Videos dataToPost)
        {
            var data = new JavaScriptSerializer().Serialize(dataToPost);

            var responseString = String.Empty;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "UIE#G{dQ#<xCXhQ%I.9:G#U<FzpisOxDkhS*e'L7dxes5((]TpmFi!!GvLU(X>0");

            try
            {
                var response = await client.PostAsync(
                    "https://www.mewstech.com/plex/api/update-videos.php",
                    //"http://plex.local/api/update-videos.php",
                    new StringContent(data, Encoding.UTF8, "application/json")
                    );

                response.EnsureSuccessStatusCode();

                responseString = await response.Content.ReadAsStringAsync();

                response.Dispose();

            } catch (Exception UnableToPostDataException)
            {
                Console.WriteLine(responseString);
                Exceptions.LogException(UnableToPostDataException);
            }

            return responseString + Environment.NewLine;
            
        }

        private static async Task<string>GetJsonData()
        {
            Console.WriteLine("GET: Starting request.\n");

            var responseBody = String.Empty;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "UIE#G{dQ#<xCXhQ%I.9:G#U<FzpisOxDkhS*e'L7dxes5((]TpmFi!!GvLU(X>0");

            try
            {
                var data = await client.GetAsync(
                    "https://www.mewstech.com/plex/api/get-videos-for-deletion.php"
                    //"http://plex.local/plex/api/get-videos-for-deletion.php"
                    );

                data.EnsureSuccessStatusCode();

                responseBody = await data.Content.ReadAsStringAsync();

                var jsonData = JsonConvert.DeserializeObject<Videos>(responseBody);

                Console.WriteLine("GET: Request completed.\n");

                VideoDeletion.movieDeletionRequests.AddRange(jsonData.Movies);
                VideoDeletion.tvShowDeletionRequests.AddRange(jsonData.TvShows);
                VideoDeletion.documentaryMovieDeletionRequests.AddRange(jsonData.DocumentaryMovies);
                VideoDeletion.documentaryTvDeletionRequests.AddRange(jsonData.DocumentaryTv);

                data.Dispose();

            } catch (Exception UnableToGetDataException)
            {
                Exceptions.LogException(UnableToGetDataException);
            }

            return responseBody + Environment.NewLine;
        }
    }
}
