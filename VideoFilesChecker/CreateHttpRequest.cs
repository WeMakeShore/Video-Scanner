using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VideoFilesChecker
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

        public static async Task<string>PostData(Videos dataToPost)
        {
            var data = new JavaScriptSerializer().Serialize(dataToPost);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "UIE#G{dQ#<xCXhQ%I.9:G#U<FzpisOxDkhS*e'L7dxes5((]TpmFi!!GvLU(X>0");

            var response = await client.PostAsync(
                "https://www.mewstech.com/plex/api/update-videos.php",
                new StringContent(data, Encoding.UTF8, "application/json") 
                );

            var responseString = await response.Content.ReadAsStringAsync();

            response.Dispose();

            return responseString + Environment.NewLine; 
        }

        private static async Task<string>GetJsonData()
        {
            Console.WriteLine("GET: Starting request.\n");
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "UIE#G{dQ#<xCXhQ%I.9:G#U<FzpisOxDkhS*e'L7dxes5((]TpmFi!!GvLU(X>0");

            var data = await client.GetAsync(
                "https://www.mewstech.com/plex/api/get-videos-for-deletion.php"
                );

            data.EnsureSuccessStatusCode();

            var responseBody = await data.Content.ReadAsStringAsync();

            var jsonData = JsonConvert.DeserializeObject<Videos>(responseBody);

            Console.WriteLine("GET: Request completed.\n");

            VideoDeletion.movieDeletionRequests.AddRange(jsonData.Movies);
            VideoDeletion.tvShowDeletionRequests.AddRange(jsonData.TvShows);
            VideoDeletion.documentaryMovieDeletionRequests.AddRange(jsonData.DocumentaryMovies);
            VideoDeletion.docummentaryTvDeletionRequests.AddRange(jsonData.DocumentaryTv);

            data.Dispose();

            return responseBody + Environment.NewLine;
        }
    }
}
