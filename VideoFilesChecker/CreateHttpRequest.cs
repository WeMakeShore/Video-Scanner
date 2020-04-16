using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ExceptionHandler;

namespace VideoChecking
{
    public static class CreateHttpRequest
    {
        public static string postUrl = Program.settings.PostUrl;
        public static string getUrl = Program.settings.GetUrl;
        public static string bearerToken = Program.settings.BearerToken;

        private static HttpClient client = new HttpClient();

        public static async Task CreatePOSTRequest(string postData)
        {
            string postResponse = await PostData(postData);

            Console.WriteLine(postResponse);
        }

        public static async Task CreateGETRequest()
        {            
            string getResponse = await GetJsonData();

            Console.WriteLine(getResponse);
        }

        private static async Task<string>PostData(string dataToPost)
        {
            var responseString = String.Empty;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            try
            {
                var response = await client.PostAsync(postUrl, new StringContent(dataToPost, Encoding.UTF8, "application/json"));

                response.EnsureSuccessStatusCode();

                responseString = await response.Content.ReadAsStringAsync();

                response.Dispose();

            } catch (Exception UnableToPostDataException)
            {
                Console.WriteLine(responseString);
                Exceptions.LogException(UnableToPostDataException);
            }

            return responseString;          
        }

        private static async Task<string>GetJsonData()
        {
            Console.WriteLine("GET: Starting request.\n");

            var responseBody = String.Empty;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            try
            {
                var data = await client.GetAsync(getUrl);

                data.EnsureSuccessStatusCode();

                responseBody = await data.Content.ReadAsStringAsync();

                VideoDeletion.listOfDeletionRequests = JsonConvert.DeserializeObject<DataModel>(responseBody);

                data.Dispose();

            } catch (Exception UnableToGetDataException)
            {
                Exceptions.LogException(UnableToGetDataException);
            }

            return responseBody + Environment.NewLine;
        }
    }
}
